using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointController : MonoBehaviour {

    public KeyCode respawnButton;
    public float minPosition;
    [HideInInspector]
    public Vector3 CurrentRespawnPoint;

    private bool pausedGameAtSpawn = true;
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
        tm.enabled = false;
        rb.useGravity = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(respawnButton) || player.transform.position.y < minPosition)
        {
            teleportPlayerToLastRespawnPoint();
        }
        else if (pausedGameAtSpawn && (
            Input.GetKeyDown(KeyCode.Q)
            || Input.GetKeyDown(KeyCode.E) 
            || Input.GetKeyDown(KeyCode.Space)|| Input.GetAxisRaw("Horizontal") != 0))
        {
            Debug.Log(Input.GetKey(KeyCode.Alpha2   ));
            rb.useGravity = true;
            pausedGameAtSpawn = false;
            tm.enabled = true;
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
        //start timer out as stopped
        tm.enabled = false;
        pausedGameAtSpawn = true;
        rb.useGravity = false;
        //reset rotation and angular velo
        rb.angularVelocity = new Vector3();
        player.transform.rotation = Quaternion.identity;
    }
}
