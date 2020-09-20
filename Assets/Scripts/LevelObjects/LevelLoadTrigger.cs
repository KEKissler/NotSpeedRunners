using UnityEngine;


public class LevelLoadTrigger : LevelTrigger
{
    public int levelIndex;

    public void OnTriggerEnter(Collider other)
    {
        LevelLoader.instance.LoadScene(levelIndex);
    }
}
