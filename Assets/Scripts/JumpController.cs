using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpController : MonoBehaviour {
    public float jumpForce;
    [HideInInspector]
    public bool isGrounded = false;
    [HideInInspector]
    public bool hasDoubleJump = true;
    public LayerMask groundedCheck;
    public PhysicMaterial grounded, unGrouned;
    
    private Rigidbody rb;
    private PlayerController pc;
    private BoxCollider bc;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<PlayerController>();
        bc = GetComponent<BoxCollider>();
        InputManager.instance.OnJump += () => { Jump(); };
	}
	
	// Update is called once per frame
	void Update () {
        //this check occurs every frame and is the only way to change is grounded
        isGrounded = Physics.OverlapSphere(pc.transform.position + (0.5f * Vector3.down), 0.2f, groundedCheck).Length > 0;
        //midair collisions maintain momentum better when a different, less friction applying physic material is used
        bc.material = (isGrounded && !pc.isGrappling) ? grounded : unGrouned;
        //need to check if grounded and should grant double jump again
        if (isGrounded) 
            hasDoubleJump = true;
    }

    public void Jump()
    {
        if (!pc.isGrappling && !(isGrounded || hasDoubleJump))
        {
            Debug.Log("Jump attempted but not grounded and no double jump available");
            return;
        }
        pc.releaseGrapple();
        rb.velocity = new Vector3(rb.velocity.x, Mathf.Max(rb.velocity.y, jumpForce), rb.velocity.z);
        if (!isGrounded)
            hasDoubleJump = false;
    }
}
