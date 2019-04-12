using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public static LevelLoader instance;

	void Awake () {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; ++i)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                TargetController.RemainingTargets = 0;
                RespawnPointController test = GameObject.Find("RespawnPoint").GetComponent<RespawnPointController>();
                test.teleportPlayerToLastRespawnPoint();
                Debug.Log("1");
                SceneManager.LoadScene(i, LoadSceneMode.Single);
                break;
            }
        }
    }
}
