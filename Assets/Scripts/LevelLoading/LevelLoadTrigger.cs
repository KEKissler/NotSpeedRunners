using UnityEngine;


public class LevelLoadTrigger : MonoBehaviour
{
    public int levelIndex;

    public void OnTriggerEnter(Collider other)
    {
        LevelLoader.instance.LoadScene(levelIndex);
    }
}
