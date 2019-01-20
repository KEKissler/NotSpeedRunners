using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointController : MonoBehaviour {

    public KeyCode respawnButton;
    [HideInInspector]
    public Vector3 CurrentRespawnPoint;

    private GameObject player;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        CurrentRespawnPoint = transform.position;
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(respawnButton))
        {
            teleportPlayerToLastRespawnPoint();
        }
	}

    public void teleportPlayerToLastRespawnPoint()
    {
        //reset position
        player.transform.position = CurrentRespawnPoint;
        //and speed
        rb.velocity = new Vector3();
        }
}
