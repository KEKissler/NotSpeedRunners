using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointController : MonoBehaviour {

    public float minPosition;
    [HideInInspector]
    public Vector3 CurrentRespawnPoint;
    [HideInInspector]
    public int blockers = 0;

    private bool pausedGameAtSpawn = true;
    private GameObject player;
    private Rigidbody rb;
    private PlayerController pc;
    private JumpController jc;
    private TimeManager tm;
    private TargetManager targetManager;

    // Use this for initialization
    void Start()
    {
        CurrentRespawnPoint = transform.position;
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody>();
        pc = player.GetComponent<PlayerController>();
        jc = player.GetComponent<JumpController>();
        tm = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        targetManager = GameObject.Find("TargetManager").GetComponent<TargetManager>();

        tm.enabled = false;
        rb.useGravity = false;
        InputManager.instance.OnAnyInputDown += () =>
        {
            if (pausedGameAtSpawn && blockers == 0)
            {
                UnfreezeGame();
            }
        };
        InputManager.instance.OnRespawn += () => { teleportPlayerToLastRespawnPoint(); };
    }
	
	// Update is called once per frame
	void Update () {
        if (pausedGameAtSpawn)
        {
            if(Input.GetAxisRaw("Horizontal") != 0)
            {
                UnfreezeGame();
            }
        }
        if (player.transform.position.y < minPosition)
        {
            InputManager.instance.PlayerFellOutOfBounds();
        }       
    }
	
    private void UnfreezeGame()
    {
        rb.useGravity = true;
        pausedGameAtSpawn = false;
        tm.enabled = true;
        
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
        targetManager.ResetAllTargets();
        //and reset the timer
        tm.ResetCurrentTime();
        //start timer out as stopped
        tm.enabled = false;
        pausedGameAtSpawn = true;
        rb.useGravity = false;
        //reset rotation and angular velo
        rb.angularVelocity = new Vector3();
        player.transform.rotation = Quaternion.identity;
    }
}
