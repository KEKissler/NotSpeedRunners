using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleMovementController : MonoBehaviour {
    public float minimumGrappleSpeed, maximumGrappleSpeed;
    public float maxPositiveGrappleAccel, maxNegativeGrappleAccel;//maxGrAccel needs to be like, 30% more than what you expect
    //because im not executing this speed checking thing in the physics update, errors in measuring acceleration
    //positive is checked when gaining speed, negative is checked when losing speed
    public float grappleSpeedMultiplier;
    public float frameWindowSize;//this value is how frequently to poll, in seconds, for different angle offsets for the release timer
    public int specialDropFrames;//total number of frames where special behavior occurs on grapple release

    private PlayerController pc;
    private Rigidbody rb;
    private Quaternion grappleToVelocityDirection;
    private JumpController jc;
    public enum Direction {x_left, x_right, y_left, y_right, z_left, z_right}
    private float timeSinceGrappleLatch = 0.0f;
    private float speedLastUpdate = 0f;
    
	// Use this for initialization
	void Start () {
        pc = GetComponent<PlayerController>();//for passing along information about grapples hitting
        rb = GetComponent<Rigidbody>();//for actually changing the velocity
        jc = GetComponent<JumpController>();//for checking grounded state, in case a grapple scrapes the ground
        grappleToVelocityDirection.eulerAngles = new Vector3(0, 0, 270);
	}
	
	// Update is called once per frame
	void Update () {
        if (!pc.isGrappling)
            return;
        float oldSpeed = rb.velocity.magnitude;
        float magnitude = Mathf.Clamp(rb.velocity.magnitude, minimumGrappleSpeed, maximumGrappleSpeed);
        //Debug.Log("acceleration = " + (magnitude - speedLastUpdate) / Time.deltaTime);

        bool wasNotClamped = Mathf.Approximately(oldSpeed, magnitude);
        
        //only do this if not clamped, since clamping is an instant change that is seen as near infinite acceleration :/
        //if speedchange since last frame is too great, reduce the change to the maximum allowed acceleration
        if (wasNotClamped)
        {
            //different speed checks for losing vs gaining speed
            if (magnitude > speedLastUpdate)
            {
                //if gaining speed too fast
                if ((magnitude - speedLastUpdate) / Time.deltaTime > maxPositiveGrappleAccel)
                {
                    //set speed to max allowed difference from last update's measured speed
                    magnitude = speedLastUpdate + (maxPositiveGrappleAccel * Time.deltaTime);
                    //Debug.Log("Reduced Speed Gain");
                }
            }
            else
            {
                //if losing speed too fast
                if ((speedLastUpdate - magnitude) / Time.deltaTime > maxNegativeGrappleAccel)
                {
                    //set speed to max allowed difference from last update's measured speed
                    magnitude = speedLastUpdate - (maxNegativeGrappleAccel * Time.deltaTime);
                    //Debug.Log("Reduced Speed Loss");
                }
            }
        }
        rb.velocity = grappleToVelocityDirection * ((pc.grapplePoint - transform.position).normalized * magnitude);
        timeSinceGrappleLatch += Time.deltaTime;
        speedLastUpdate = magnitude;
	}

    public void OnGrappleLatch()
    {
        rb.velocity *= grappleSpeedMultiplier;
        timeSinceGrappleLatch = 0.0f;
        speedLastUpdate = rb.velocity.magnitude;
        //Physics.gravity *= 2;
    }

    public void OnGrappleRelease()
    {
        int frame = Mathf.FloorToInt(timeSinceGrappleLatch / frameWindowSize);
        //only do special things if the frame this was called is in the specialDropFrame range
        if(frame <= specialDropFrames)
        {
            //redirect velocity down, down amount scaled to how close to frame 0 the release was
            rb.velocity = rb.velocity.magnitude * Vector3.Lerp(Vector3.down, rb.velocity.normalized, (1.0f * frame) / specialDropFrames);
        }
        //Physics.gravity /= 2;
    }

    public void setGrappleDir(Direction dir)
    {
        Vector3 eulerAnglesToUse = new Vector3();
        switch (dir)
        {
            case Direction.x_left:
                eulerAnglesToUse = new Vector3(0, 0, 90);
                break;
            case Direction.x_right:
                eulerAnglesToUse = new Vector3(0, 0, 270);
                break;
            case Direction.y_left:
                break;
            case Direction.y_right:
                break;
            case Direction.z_left:
                break;
            case Direction.z_right:
                break;
        }
        grappleToVelocityDirection.eulerAngles = eulerAnglesToUse;
    }

    void OnCollisionStay(Collision other)
    {
        if(!jc.isGrounded && pc.isGrappling)
        {
            pc.releaseGrapple();
        }
    }
}
