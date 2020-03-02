using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public RespawnPointController RespawnPointController;
    public Transform newRespawnPoint;

    public void OnTriggerEnter(Collider other)
    {
        RespawnPointController.CurrentRespawnPoint = newRespawnPoint.position;
    }
}
