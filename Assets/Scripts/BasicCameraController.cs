/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraController : MonoBehaviour {
    //x dir is all
    public GameObject player;
    public float lerpAmount;
    public float velocityPredictAmount;

    private float fixedYPos;
    private float fixedZPos;
    private GrappleMovementController gmc;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        fixedZPos = transform.position.z;
        fixedYPos = transform.position.y;
        rb = player.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.LerpUnclamped(transform.position, new Vector3(player.transform.position[0] + rb.velocity[0] * velocityPredictAmount, fixedYPos, fixedZPos), lerpAmount * Time.deltaTime);
	}
}*/

 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraController : MonoBehaviour {
    //x dir is all
    public GameObject player;
    public float baseLerpAmount;
    public float lerpAmpBySpeed;
    public float velocityPredictAmount;
    public bool freezeXPos;
    public bool freezeYPos;



    private float fixedXPos;
    private float fixedZPos;
    private float fixedYPos;
    private GrappleMovementController gmc;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        fixedXPos = transform.position.x;
        fixedZPos = transform.position.z;
        fixedYPos = transform.position.y;
        rb = player.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        float lerpAmount = baseLerpAmount + lerpAmpBySpeed * Vector2.Distance((Vector2)(rb.position), (Vector2)(transform.position));
        Vector3 desiredNextCamPos =
            Vector3.LerpUnclamped(
                new Vector3(freezeXPos ? fixedXPos : rb.position.x, freezeYPos ? fixedYPos : rb.position.y, fixedZPos),

                new Vector3(freezeXPos ? fixedXPos :
                            rb.position[0] + rb.velocity[0] * velocityPredictAmount,

                            freezeYPos ? fixedYPos :
                            rb.position[1] + rb.velocity[1] * velocityPredictAmount,

                            fixedZPos),

                lerpAmount * Time.deltaTime);

        transform.position = Vector3.LerpUnclamped(transform.position, desiredNextCamPos, 0.1f);
	}

    public void updateFixedXPos(float newValue)
    {
        fixedXPos = newValue;
    }
    public void updateFixedYPos(float newValue)
    {
        fixedYPos = newValue;
    }
}

