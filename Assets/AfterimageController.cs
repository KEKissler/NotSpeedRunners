using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AfterimageController : MonoBehaviour {
    public float timeToLive;

    private float timeSinceReset;
    private SpriteRenderer sr;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        timeSinceReset = timeToLive;//invisible at start
	}
	
	// Update is called once per frame
	void Update () {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1 - Mathf.Clamp01(timeSinceReset / timeToLive));
        timeSinceReset += Time.deltaTime;
	}

    public void Reset(Transform toCopy)
    {
        Debug.Log(toCopy.name);
        timeSinceReset = 0;
        transform.position = toCopy.position;
        Quaternion newRotation = new Quaternion();
        newRotation.eulerAngles = new Vector3(0, 0, toCopy.eulerAngles.z);
        transform.rotation = newRotation;
        transform.localScale = toCopy.localScale;
    }
}
