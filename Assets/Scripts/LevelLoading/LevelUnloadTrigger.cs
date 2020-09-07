using UnityEngine;


public class LevelUnloadTrigger : MonoBehaviour
{
    public int levelIndex;

    public void OnTriggerEnter(Collider other)
    {
        LevelLoader.instance.UnloadScene(levelIndex);
    }
}
