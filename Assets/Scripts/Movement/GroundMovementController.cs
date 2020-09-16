using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMovementController : MonoBehaviour {
    public float targetWalkSpeed, walkAccel, walkDecel, frictionDecel;

    private Rigidbody rb;
    private JumpController jc;

	void Start () {
        rb = GetComponent<Rigidbody>();
        jc = GetComponent<JumpController>();
	}
	
	// Update is called once per frame
	void Update () {
        //Ground movement only applied when grounded and not currently swinging
        if (!jc.isGrounded)
            return;
        
        //friction calcs
        float speedReductionThisFrame = Time.deltaTime * frictionDecel;
        if (Mathf.Abs(rb.velocity.x) > speedReductionThisFrame)
        {
            rb.velocity += new Vector3(-1 * Mathf.Sign(rb.velocity.x) * speedReductionThisFrame, 0, 0);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        //movement calcs
        float oldSpeed = rb.velocity.x;//grounded can only occur against flat surfaces below player, speed should only be in x dir
        float input = InputManager.instance.HorizontalAxis;
        //if no input, friction will come for its due and player will slow due to collisions normally
        //but if there is input...
        if (!Mathf.Approximately(input, 0))
        { //in same direction as velocity
            if ((Mathf.Sign(oldSpeed) == Mathf.Sign(input)) || Mathf.Approximately(oldSpeed, 0))
            {
                //if applying max accel would not put speed above target limit
                if (Mathf.Abs(oldSpeed + (input * walkAccel * Time.deltaTime)) < targetWalkSpeed)
                {
                    rb.velocity = new Vector3(oldSpeed + (input * walkAccel * Time.deltaTime), rb.velocity.y, 0);
                }
                //would go beyond limit
                else
                {
                    //so set velocity to either the targetWalkSpeed or leave it untouched if player was already traveling faster
                    if (Mathf.Abs(rb.velocity.x) < targetWalkSpeed)
                    {
                        rb.velocity = new Vector3(Mathf.Sign(oldSpeed) * targetWalkSpeed, rb.velocity.y, 0);
                    }
                }
            }
            //fighting velocity
            else
            {
                //no check needed, losing speed and minimum is zero.
                rb.velocity = new Vector3(oldSpeed + (input * walkDecel * Time.deltaTime), rb.velocity.y, 0);
            }
        }        
	}
}
