using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(Animator))]
public class SlipstreamBlockController : MonoBehaviour {
    private const string ON_STATE = "SlipstreamDefault";
    private const string OFF_STATE = "SlipstreamDeactivated";

    public float activationThreshold;
    public float deactivationThreshold;

    private BoxCollider boxCol;
    private SpriteRenderer background;
    private Rigidbody player;
    private Animator animator;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Rigidbody>();
        boxCol = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
	}
	
    private void enable()
    {
        boxCol.enabled = true;
        animator.Play(ON_STATE);
    }

    private void disable()
    {
        boxCol.enabled = false;
        animator.Play(OFF_STATE);
    }

	// Update is called once per frame
	void Update () {
        if(boxCol.enabled)
        {
            if(player.velocity.magnitude > activationThreshold)
            {
                disable();
            }
        }
        else
        {
            if (player.velocity.magnitude < deactivationThreshold)
            {
                enable();
            }
        }
	}
}
