using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody), typeof(TrailRenderer))]
public class TrailController : MonoBehaviour {
    public float activationThreshold;
    public float deactivationThreshold;
    public float alphaIncreasePerSecond;
    public float alphaDecreasePerSecond;
    public float maxAlpha;

    private Image slipstreamVignette;
    private Rigidbody player;
    private bool active = false;
    private TrailRenderer tr;
    private float alpha;

    // Use this for initialization
    void Start () {
        slipstreamVignette = GameObject.Find("SlipstreamVignette").GetComponent<Image>();
        player = GetComponent<Rigidbody>();
        tr = GetComponent<TrailRenderer>();
        tr.emitting = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (!active)
        {
            if (player.velocity.magnitude > activationThreshold)
            {
                active = true;
                enable();
            }
        }
        else
        {
            if (player.velocity.magnitude < deactivationThreshold)
            {
                active = false;
                disable();
            }
        }
        manageAlpha();
    }

    private void manageAlpha()
    {
        alpha += (active ? alphaIncreasePerSecond : -alphaDecreasePerSecond) * Time.deltaTime;
        alpha = Mathf.Clamp(alpha, 0 , maxAlpha);
        slipstreamVignette.color = new Color(
                slipstreamVignette.color.r,
                slipstreamVignette.color.g,
                slipstreamVignette.color.b,
                alpha);
    }

    private void enable()
    {
        tr.emitting = true;
    }

    private void disable()
    {
        tr.emitting = false;
    }
}
