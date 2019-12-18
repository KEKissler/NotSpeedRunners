using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlipstreamManager : MonoBehaviour
{
    private const string ON_STATE = "SlipstreamDefault";
    private const string OFF_STATE = "SlipstreamDeactivated";

    public float activationThreshold;
    public float deactivationThreshold;

    public Material yellow;
    public Material white;

    public AfterimageManager AfterimageManager;

    private bool active = false;
    private Rigidbody player;
    private MeshRenderer MeshRenderer;
    private readonly List<BoxCollider> boxColliders = new List<BoxCollider>();
    private readonly List<Animator> animators = new List<Animator>();
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody>();
        MeshRenderer = player.GetComponent<MeshRenderer>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            var singleSlipstream = transform.GetChild(i);
            boxColliders.Add(singleSlipstream.GetComponent<BoxCollider>());
            animators.Add(singleSlipstream.GetComponent<Animator>());
        }
    }

    private void enable()
    {
        AfterimageManager.Emitting = false;
        MeshRenderer.material = white;
        foreach(BoxCollider boxCol in boxColliders)
        {
            boxCol.enabled = true;
        }
        foreach (Animator animator in animators)
        {
            animator.Play(ON_STATE);
        }
    }

    private void disable()
    {
        AfterimageManager.Emitting = true;
        MeshRenderer.material = yellow;
        foreach (BoxCollider boxCol in boxColliders)
        {
            boxCol.enabled = false;
        }
        foreach (Animator animator in animators)
        {
            animator.Play(OFF_STATE);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (player.velocity.magnitude > activationThreshold)
            {
                active = false;
                disable();
            }
        }
        else
        {
            if (player.velocity.magnitude < deactivationThreshold)
            {
                active = true;
                enable();
            }
        }
    }
}