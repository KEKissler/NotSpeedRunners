using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour {
    public float minSpeed;
    private TrailRenderer tr;
    private Rigidbody rb;
	// Use this for initialization
	void Start () {
        tr = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody>();
        tr.emitting = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (tr.emitting)
        {
            if (rb.velocity.magnitude < minSpeed)
                tr.emitting = false;
        }
        else
        {
            if (rb.velocity.magnitude > minSpeed)
                tr.emitting = true;
        }
	}
}
