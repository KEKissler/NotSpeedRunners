using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostZoneController : MonoBehaviour {

    public Vector2 setVelocity;

    private Rigidbody rb;
    private PlayerController pc;
    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerController>();
        rb = player.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            pc.releaseGrapple();
            rb.velocity = setVelocity;
        }
    }
}
