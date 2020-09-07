using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public Transform newRespawnPoint;

    public void OnTriggerEnter(Collider other)
    {
        RespawnPointController.instance.CurrentRespawnPoint = newRespawnPoint.position;
    }
}
