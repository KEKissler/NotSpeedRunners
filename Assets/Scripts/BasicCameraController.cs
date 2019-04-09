using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraController : MonoBehaviour {
    //x dir is all
    public GameObject player;
    public float baseLerpAmount;
    public float lerpAmpBySpeed;
    public float velocityPredictAmount;

    private float fixedZPos;
    private GrappleMovementController gmc;
    private Rigidbody rb;
    private Vector2 veloLastFrame = new Vector2(), veloThisFrame = new Vector2();
	// Use this for initialization
	void Start () {
        fixedZPos = transform.position.z;
        rb = player.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        float lerpAmount = baseLerpAmount + lerpAmpBySpeed * Vector2.Distance((Vector2)(rb.position), (Vector2)(transform.position));
        Vector3 desiredNextCamPos = 
            Vector3.LerpUnclamped(
                new Vector3(rb.position.x, rb.position.y, fixedZPos), 
                new Vector3(rb.position[0] + rb.velocity[0] * velocityPredictAmount,
                            rb.position[1] + rb.velocity[1] * velocityPredictAmount,
                            fixedZPos),
                lerpAmount * Time.deltaTime);

        transform.position = Vector3.LerpUnclamped(transform.position, desiredNextCamPos, 0.1f);
	}
}
