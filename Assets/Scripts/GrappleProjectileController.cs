using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleProjectileController : MonoBehaviour {

    public float speed;

    private Rigidbody rb;
    private PlayerController pc;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        Reset();
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject && other.gameObject.tag == "Grappleable")
        {
            rb.velocity = Vector3.zero;
            pc.OnGrappleLatch(transform.position);
        }
        else
        {
            Reset();
            pc.lr.enabled = false;
        }
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }
}
