using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointController : MonoBehaviour {

    public KeyCode respawnButton;
    [HideInInspector]
    public Vector3 CurrentRespawnPoint;

    private GameObject player, targets;
    private Rigidbody rb;
    private PlayerController pc;
    private JumpController jc;
    private RespawnPointController rpc;
    private TimeManager tm;

	// Use this for initialization
	void Start () {
        CurrentRespawnPoint = transform.position;
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody>();
        pc = player.GetComponent<PlayerController>();
        jc = player.GetComponent<JumpController>();
        targets = GameObject.Find("Targets");
        rpc = targets.GetComponent<RespawnPointController>();
        tm = GameObject.Find("TimeManager").GetComponent<TimeManager>();
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
        //and disconnect grapples
        pc.releaseGrapple();
        //and grant double jump back
        jc.hasDoubleJump = true;
        //and reset all targets
        foreach (Transform t in targets.transform)
        {
            if (!t.gameObject.activeSelf)
            {
                ++TargetController.RemainingTargets;
                t.gameObject.SetActive(true);
            }
        }
        //and reset the timer
        tm.resetCurrentTime();
    }
}
