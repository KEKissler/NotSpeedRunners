using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int FirstSceneIndex;
    public List<string> TrackedSceneNames = new List<string>();

    private static LevelLoader instance;
    private RespawnPointController RespawnPointController;
    private TargetManager TargetManager;
    private List<Scene> ActiveAdditiveScenes = new List<Scene>();
    private Coroutine currentAsyncOperations;
    private string[] scenePaths;
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
        TargetManager = GameObject.Find("TargetManager").GetComponent<TargetManager>();
        yield return StartCoroutine(DownloadAssetBundle("levels"));

        if (bundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }
        scenePaths = bundle.GetAllScenePaths();
        Time.timeScale = 0;
        LoadScene(scenePaths[FirstSceneIndex], () =>
        {
            Time.timeScale = 1;
        });
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

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(i))
                {
                    if (RespawnPointController == null)
                    {
                        RespawnPointController = GameObject.Find("RespawnPoint").GetComponent<RespawnPointController>();
                    }
                    RespawnPointController.teleportPlayerToLastRespawnPoint();
                    break;
                }
                LoadScene(TrackedSceneNames[i]);
                break;
            }
        }
    }

    private void LoadScene(string sceneName, Action finished = null)
    {
        var asyncOperations = new List<AsyncOperation>();
        asyncOperations.Add(SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive));
        //for now, unload all but the Startup Scene so that there is always Startup and the Additive Scene present
        foreach (var additiveScene in ActiveAdditiveScenes)
        {
            asyncOperations.Add(SceneManager.UnloadSceneAsync(additiveScene));
        }

        if (currentAsyncOperations != null)
        {
            Debug.LogWarning("Ignored request to load scene" + sceneName + ", still processing last load.");
            return;
        }
        currentAsyncOperations = StartCoroutine(WaitForAsyncOperations(asyncOperations, () =>
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            TargetManager.Initialize();
            NewSceneFinishedLoading(scene);
            currentAsyncOperations = null;
            if (finished != null)
            {
                finished.Invoke();
            }
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

    private void NewSceneFinishedLoading(Scene scene)
    {
        ActiveAdditiveScenes.Add(scene);
        RespawnPointController = GameObject.Find("RespawnPoint").GetComponent<RespawnPointController>();
        RespawnPointController.teleportPlayerToLastRespawnPoint();
        var targetParent = GameObject.Find("Targets").transform;
        var targetManager = GameObject.Find("TargetManager").GetComponent<TargetManager>();
        targetManager.targets = targetParent;
    }
}
