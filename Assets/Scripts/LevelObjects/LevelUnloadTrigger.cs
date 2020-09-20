using UnityEngine;


public class LevelUnloadTrigger : LevelTrigger
{
    public int levelIndex;

    public void OnTriggerEnter(Collider other)
    {
        LevelLoader.instance.UnloadScene(levelIndex);
    }
}
