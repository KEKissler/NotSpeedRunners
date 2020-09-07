using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int FirstSceneIndex;

    public static LevelLoader instance;
    
    private RespawnPointController RespawnPointController;
    private readonly List<Scene> ActiveAdditiveScenes = new List<Scene>();
    private Coroutine currentAsyncOperations;//acts as a semaphore, null indicates not in use. Only allowing one load or unload at a time, so callbacks execute under expected circumstances
    private readonly Queue<LevelLoadOperation> loadingOperations = new Queue<LevelLoadOperation>();
    private string[] scenePaths = new string[0];
    private AssetBundle bundle;

	void Awake ()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator Start()
    {
#if !UNITY_EDITOR
        yield return StartCoroutine(DownloadAssetBundle("levels"));

        if (bundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        scenePaths = bundle.GetAllScenePaths();
#else
        var paths = new List<string>();
        foreach (var guid in AssetDatabase.FindAssets("t:scene", new[] {"Assets/Scenes/LevelsAssetBundle"}))
        {
            paths.Add(AssetDatabase.GUIDToAssetPath(guid));
        }
        scenePaths = paths.ToArray();
#endif
        for (var i = 0; i < SceneManager.sceneCount; ++i)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.name != "Startup")
            {
                ActiveAdditiveScenes.Add(scene);
            }
        }
        LoadScene(FirstSceneIndex);
        yield break;
    }

    private IEnumerator DownloadAssetBundle(string assetBundleName)
    {
        string uri = Application.dataPath + "/AssetBundles/" + assetBundleName;
#if UNITY_EDITOR
        uri =  "file:///" + uri;  
#endif
        using (var request = UnityWebRequestAssetBundle.GetAssetBundle(uri))
        {
            yield return request.SendWebRequest(); 
            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                bundle = DownloadHandlerAssetBundle.GetContent(request);
            }
        }
    }

    public void Update () {
        if (currentAsyncOperations != null)
        {
            return;
        }
        if (loadingOperations.Count <= 0)
        {
            return;
        }
        var queuedParameters = loadingOperations.Dequeue();
        if (queuedParameters.Load)
        {
            LoadScene(queuedParameters.ScenePathIndex, queuedParameters.Finished);
        }
        else
        {
            UnloadScene(queuedParameters.ScenePathIndex, queuedParameters.Finished);
        }
    }

    public void LoadScene(int scenePathIndex, Action finished = null)
    {
        if (currentAsyncOperations != null)
        {
            loadingOperations.Enqueue(new LevelLoadOperation {Load = true, ScenePathIndex = scenePathIndex, Finished = finished});
            return;
        }

        var scenePath = scenePaths[scenePathIndex];
        var alreadyLoadedScene = SceneManager.GetSceneByName(scenePath);
        foreach (var scene in ActiveAdditiveScenes)
        {
            if (scene.path.Equals(scenePath) || scene.name.Equals(scenePath))
            {
                alreadyLoadedScene = scene;
            }
        }
        if (alreadyLoadedScene.isLoaded)
        {
            Debug.LogWarning("Ignored request to load scene " + alreadyLoadedScene.name + ", it's already loaded!");
            if (finished != null)
            {
                finished.Invoke();
            }
            return;
        }

        var cachedGravity = Physics.gravity;
        var firstSceneLoaded = false;
        if (SceneManager.sceneCount == 1)//just the Startup Scene present, no level geometry yet
        {
            firstSceneLoaded = true;
            Time.timeScale = 0;
            Physics.gravity = new Vector3();
        }
        var asyncOperations = new List<AsyncOperation> {SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive)};
        currentAsyncOperations = StartCoroutine(WaitForAsyncOperations(asyncOperations, () =>
        {
            var scene = SceneManager.GetSceneByName(scenePath);
            if (!scene.isLoaded)
            {
                scene = SceneManager.GetSceneByPath(scenePath);
            }
            ActiveAdditiveScenes.Add(scene);
            if (finished != null)
            {
                finished.Invoke();
            }
            if (firstSceneLoaded)
            {
                Time.timeScale = 1;
                Physics.gravity = cachedGravity;
            }
            currentAsyncOperations = null;
        }));
    }

    public void UnloadScene(int scenePathIndex, Action finished = null)
    {
        if (currentAsyncOperations != null)
        {
            loadingOperations.Enqueue(new LevelLoadOperation {Load = false, ScenePathIndex = scenePathIndex, Finished = finished});
            return;
        }
        
        var scenePath = scenePaths[scenePathIndex];
        var sceneToUnload = new Scene();
        foreach (var scene in ActiveAdditiveScenes)
        {
            if (scene.path.Equals(scenePath) || scene.name.Equals(scenePath))
            {
                sceneToUnload = scene;
            }
        }
        if (sceneToUnload.name == "" || !sceneToUnload.isLoaded)
        {
            Debug.LogWarning("Ignored request to unload scene at " + scenePath + ". It's already unloaded (or untracked).");
            if (finished != null)
            {
                finished.Invoke();
            }
            return;
        }

        var asyncOperations = new List<AsyncOperation> {SceneManager.UnloadSceneAsync(sceneToUnload)};
        currentAsyncOperations = StartCoroutine(WaitForAsyncOperations(asyncOperations, () =>
        {
            ActiveAdditiveScenes.Remove(sceneToUnload);
            if (finished != null)
            {
                finished.Invoke();
            }
            currentAsyncOperations = null;
        }));
    }

    private IEnumerator WaitForAsyncOperations(List<AsyncOperation> operations, Action finished)
    {
        while (!AllOperationsAreDone(operations))
        {
            yield return null;
        }
        if (finished != null)
        {
            finished.Invoke();
        }
    }

    private static bool AllOperationsAreDone(List<AsyncOperation> operations)
    {
        var allFinished = true;
        foreach (var asyncOperation in operations)
        {
            if (!asyncOperation.isDone)
            {
                allFinished = false;
                break;
            }
        }
        return allFinished;
    }
}

/// <summary>
/// Used to save parameters for LevelLoad and LevelUnload calls that could not be immediately completed, but will be queued up to execute ASAP.
/// </summary>
public class LevelLoadOperation
{
    public bool Load;
    public int ScenePathIndex;
    public Action Finished;
}
