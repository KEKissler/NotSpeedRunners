using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class ReinstateSpeed : MonoBehaviour {

    public GameObject TimerPrefab;
    public Color timerColor;
    public float timerLength;
    public float magnitudeChangeThreshold;
    public float ignoreReinstateIfFaster;
    public float maxReinstateMagnitude;

    private Vector3? reinstateVelocity = null;
    private Vector3 savedVelocity;
    private Rigidbody rb;
    private bool isTimerOn = false;
    private TimerAnimationController timer;

    void Start () {
        rb = GetComponent<Rigidbody>();
        InputManager.instance.OnReinstate += OnReinstateButtonDown;
	}

    private void OnReinstateButtonDown()
    {
        if (reinstateVelocity != null)
        {
            rb.velocity = reinstateVelocity.Value;
            reinstateVelocity = null;
            isTimerOn = false;
            if (timer != null)
            {
                Destroy(timer.gameObject);
            }
        }
    }
    

    void FixedUpdate()
    {
        //if a speed isnt already stored, the change is great enough, the new value isnt faster than max speed, and reinstating speed would actually give you speed back
        if (!isTimerOn && rb.velocity.magnitude < ignoreReinstateIfFaster && rb.velocity.magnitude < savedVelocity.magnitude  && Mathf.Abs(rb.velocity.magnitude - savedVelocity.magnitude) > magnitudeChangeThreshold)
        {
            if(savedVelocity.magnitude > maxReinstateMagnitude)
            {
                savedVelocity = savedVelocity.normalized * maxReinstateMagnitude;
            }
            reinstateVelocity = savedVelocity;
            timer = Instantiate(TimerPrefab).GetComponent<TimerAnimationController>();
            ParticleSystem particles = timer.GetComponent<ParticleSystem>();
            ParticleSystem.ShapeModule s = particles.shape;
            Vector3 velo =  reinstateVelocity.Value;
            var main = particles.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(reinstateVelocity.Value.magnitude / 5);
            float angleToUse = Mathf.Atan2(velo.y, velo.x) * Mathf.Rad2Deg;
            angleToUse = (angleToUse - 90 < 0) ? angleToUse - 90 + 360 : angleToUse - 90;//unity plz
            s.rotation = new Vector3(0, 0, angleToUse);
            timer.ClipLength = timerLength;
            timer.ToFollow = transform;
            timer.OnTimerEnd += () =>
            {
                //rb.velocity = reinstateVelocity.Value;
                reinstateVelocity = null;
                isTimerOn = false;
            };
            timer.color = timerColor;
            isTimerOn = true;
        }
        savedVelocity = rb.velocity;
    }
}
