﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public KeyCode leftKey, rightKey;
    [HideInInspector]
    public bool isGrappling = false;
    [HideInInspector]
    public Vector3 grapplePoint = new Vector3();
    public GameObject ProjectilePrefab;
    [HideInInspector]
    public LineRenderer lr;

    private GameObject grappleHookProjectile;
    private GrappleMovementController gmc;
    private JumpController jc;
    private GrappleProjectileController gpc;
    private Rigidbody rb;
    private KeyCode currentGrappleDir;

    // Use this for initialization
    void Start()
    {
        grappleHookProjectile = Instantiate(ProjectilePrefab);
        gmc = GetComponent<GrappleMovementController>();
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
        jc = GetComponent<JumpController>();
        gpc = grappleHookProjectile.GetComponent<GrappleProjectileController>();
        rb = GetComponent<Rigidbody>();
        currentGrappleDir = KeyCode.None;
    }
	

	// Update is called once per frame
	void Update () {
        //check for grapple release
        if (currentGrappleDir != KeyCode.None)
        {
            if(currentGrappleDir == leftKey)
            {
                if (Input.GetKeyUp(leftKey))
                {
                    releaseGrapple();
                }
            }
            else //currentGrappleDir is rightKey
            {
                if (Input.GetKeyUp(rightKey))
                {
                    releaseGrapple();
                }
            }
        }
        //check for input to start firing a grapple out
        if (currentGrappleDir == KeyCode.None)
        {
            if (Input.GetKeyDown(leftKey))
            {
                fireGrapple(true);
                currentGrappleDir = leftKey;
            }
            else if (Input.GetKeyDown(rightKey))
            {
                fireGrapple(false);
                currentGrappleDir = rightKey;
            }
        }
        //keeping the would be grapple line updated
        lr.SetPositions(new Vector3[] { GetComponent<Rigidbody>().transform.position, grappleHookProjectile.transform.position });
    }

    public void OnGrappleLatch(Vector3 grappleLoc)
    {
        jc.hasDoubleJump = true;
        lr.enabled = true;
        grapplePoint = grappleLoc;
        isGrappling = true;
        gmc.OnGrappleLatch();
    }

    //disconnects the grapple and, if the grapple had latched, calculates drop grapple
    public void releaseGrapple()
    {
        currentGrappleDir = KeyCode.None;
        grappleHookProjectile.GetComponent<GrappleProjectileController>().Reset();
        lr.enabled = false;
        if (isGrappling) {
            isGrappling = false;
            gmc.OnGrappleRelease();
        }
    }

    private void fireGrapple(bool grappleLeft)
    {
        grappleHookProjectile.GetComponent<Rigidbody>().velocity = gpc.speed * ((grappleLeft) ? new Vector3(-1, 1, 0) : new Vector3(1, 1, 0));
        grappleHookProjectile.transform.position = transform.position;
        grappleHookProjectile.SetActive(true);
        lr.enabled = true;
        gmc.setGrappleDir((grappleLeft) ? GrappleMovementController.Direction.x_left : GrappleMovementController.Direction.x_right);
    }
}