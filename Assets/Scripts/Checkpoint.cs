using UnityEngine;

public class Checkpoint : LevelTrigger
{
    public Transform newRespawnPoint;

    public void OnTriggerEnter(Collider other)
    {
        RespawnPointController.instance.CurrentRespawnPoint = newRespawnPoint.position;
    }
}
