using System.Collections;
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
}
