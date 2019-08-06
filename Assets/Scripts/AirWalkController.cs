using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirWalkController : MonoBehaviour {
    public float accel, decel, maxAirSpeed, frictionDecel;
    public InputManager InputManager;

    private Rigidbody rb;
    private JumpController jc;
    private PlayerController pc;

	void Start ()
    {
        rb = GetComponent<Rigidbody>();
        jc = GetComponent<JumpController>();
        pc = GetComponent<PlayerController>();
    }

	// Update is called once per frame
	void Update () {
        if (pc.isGrappling || jc.isGrounded)
            return;
        //friction calcs
        float speedReductionThisFrame = Time.deltaTime * frictionDecel;
        if (Mathf.Abs(rb.velocity.x) > speedReductionThisFrame)
        {
            rb.velocity += -speedReductionThisFrame * rb.velocity.normalized;
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        //movement calcs
        float oldSpeed = rb.velocity.x;//grounded can only occur against flat surfaces below player, speed should only be in x dir
        float input = (Input.GetKey(InputManager.walkLeft) ? -1 : 0) +
                (Input.GetKey(InputManager.walkRight) ? 1 : 0);
        //if no input, friction will come for its due and player will slow due to collisions normally
        //but if there is input...
        if (!Mathf.Approximately(input, 0))
        { //in same direction as velocity
            if ((Mathf.Sign(oldSpeed) == Mathf.Sign(input)) || Mathf.Approximately(oldSpeed, 0))
            {
                //if applying max accel would not put speed above target limit
                if (Mathf.Abs(oldSpeed + (input * accel * Time.deltaTime)) < maxAirSpeed)
                {
                    rb.velocity = new Vector3(oldSpeed + (input * accel * Time.deltaTime), rb.velocity.y, 0);
                }
                //would go beyond limit
                else
                {
                    //so set velocity to either the targetWalkSpeed or leave it untouched if player was already traveling faster
                    if (Mathf.Abs(rb.velocity.x) < maxAirSpeed)
                    {
                        Debug.Log("Set to target air speed!");
                        rb.velocity = new Vector3(Mathf.Sign(oldSpeed) * maxAirSpeed, rb.velocity.y, 0);
                    }
                }
            }
            //fighting velocity
            else
            {
                //no check needed, losing speed and minimum is zero.
                rb.velocity = new Vector3(oldSpeed + (input * decel * Time.deltaTime), rb.velocity.y, 0);
            }
        }
    }

}
