using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPointController : MonoBehaviour {

    public Transform player;
    public Rigidbody playerRigidbody;
    public PlayerController playerController;
    public JumpController jumpController;
    public TimeManager timeManager;
    [HideInInspector]
    public Vector3 CurrentRespawnPoint;

    public static RespawnPointController instance;

    private ShrineController shrineController;
    private bool pausedGameAtSpawn = true;
    
    private bool initialized;
    private bool triedToTeleportToRespawnPointBeforeInitialized;

    public void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        
        CurrentRespawnPoint = transform.position;

        timeManager.enabled = false;
        playerRigidbody.useGravity = false;
        InputManager.instance.OnAnyInputDown += () =>
        {
            if (pausedGameAtSpawn)
            {
                UnfreezeGame();
            }
        };
        InputManager.instance.OnRespawn += () => { teleportPlayerToLastRespawnPoint(); };
        initialized = true;
        if (triedToTeleportToRespawnPointBeforeInitialized)
        {
            teleportPlayerToLastRespawnPoint();
        }
    }
	
	void Update () {
        if (pausedGameAtSpawn)
        {
            if(Input.GetAxisRaw("Horizontal") != 0)
            {
                UnfreezeGame();
            }
        }
    }
	
    private void UnfreezeGame()
    {
        playerRigidbody.useGravity = true;
        pausedGameAtSpawn = false;
        timeManager.enabled = true;
    }

    public void SetActiveShrine(ShrineController newActiveShrine)
    {
        shrineController = newActiveShrine;
    }
    
    public void teleportPlayerToLastRespawnPoint()
    {
        if (!initialized)
        {
            Debug.LogError("called teleportPlayerToLastRespawnPoint before singleton initialized!");
            triedToTeleportToRespawnPointBeforeInitialized = true;
            return;
        }
        //reset position
        player.position = CurrentRespawnPoint;
        //and speed
        playerRigidbody.velocity = new Vector3();
        //and disconnect grapples
        playerController.releaseGrapple();
        //and grant double jump back
        jumpController.hasDoubleJump = true;
        //reset targets if active shrine
        if(shrineController != null)
        {
            shrineController.ResetPlayer();
        }
        //start timer out as stopped
        // tm.ResetCurrentTime();
        pausedGameAtSpawn = true;
        playerRigidbody.useGravity = false;
        //reset rotation and angular velo
        playerRigidbody.angularVelocity = new Vector3();
        player.rotation = Quaternion.identity;
    }
}
