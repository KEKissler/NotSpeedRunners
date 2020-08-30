using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int FirstSceneIndex;

    private static LevelLoader instance;
    private RespawnPointController RespawnPointController;
    private readonly List<Scene> ActiveAdditiveScenes = new List<Scene>();
    private Coroutine currentAsyncOperations;
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
        LoadScene(scenePaths[FirstSceneIndex]);
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
        for (var i = 0; i < scenePaths.Length; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                LoadScene(scenePaths[i]);
                break;
            }
        }
    }

    private void LoadScene(string sceneName, Action finished = null)
    {
        if (currentAsyncOperations != null)
        {
            Debug.LogWarning("Ignored request to load scene " + sceneName + ", still processing last load.");
            if (finished != null)
            {
                finished.Invoke();
            }
            return;
        }

        var alreadyLoadedScene = SceneManager.GetSceneByName(sceneName);
        foreach (var scene in ActiveAdditiveScenes)
        {
            if (scene.path.Equals(sceneName) || scene.name.Equals(sceneName))
            {
                alreadyLoadedScene = scene;
            }
        }
        if (alreadyLoadedScene.isLoaded)
        {
            Debug.LogWarning("Ignored request to load scene " + alreadyLoadedScene.name + ", it's already loaded!\nStill initializing scene from current state.");
            InitializeScene(alreadyLoadedScene, finished);
            if (finished != null)
            {
                finished.Invoke();
            }
            return;
        }
        Time.timeScale = 0;
        var asyncOperations = new List<AsyncOperation>();
        asyncOperations.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
        //for now, unload all but the Startup Scene so that there is always Startup and the Additive Scene present
        var toRemove = new List<Scene>();
        foreach (var additiveScene in ActiveAdditiveScenes)
        {
            asyncOperations.Add(SceneManager.UnloadSceneAsync(additiveScene));
            toRemove.Add(additiveScene);
        }
        foreach (var scene in toRemove)
        {
            ActiveAdditiveScenes.Remove(scene);
        }
        currentAsyncOperations = StartCoroutine(WaitForAsyncOperations(asyncOperations, () =>
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (!scene.isLoaded)
            {
                scene = SceneManager.GetSceneByPath(sceneName);
            }
            ActiveAdditiveScenes.Add(scene);
            InitializeScene(scene, finished);
            Time.timeScale = 1;
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

    private void InitializeScene(Scene scene, Action finished = null)
    {
        if (scene.isLoaded == false)
        {
            Debug.LogError("Cannot initialize unloaded scene \"" + scene.name + "\".");
            if (finished != null)
            {
                finished.Invoke();
            }
            return;
        }
        TargetManager.instance.targets = GameObject.Find("Targets").transform;
        RespawnPointController = FindObjectOfType<RespawnPointController>();
        RespawnPointController.teleportPlayerToLastRespawnPoint();
        if (finished != null)
        {
            finished.Invoke();
        }
    }
}
