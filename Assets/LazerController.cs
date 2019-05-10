using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LazerController : MonoBehaviour {

    private GameObject player;
    private Rigidbody rb;
    private SpriteRenderer sr;
    private RespawnPointController rpc;

    public float secondsToPredict;
    public float lazerRadius;
    public float firingFrequencyInSeconds;

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
        sr.transform.localScale = Vector3.one * lazerRadius * 2f;
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody>();
        rpc = GameObject.Find("RespawnPoint").GetComponent<RespawnPointController>();

        //start the infinite coroutine
        StartCoroutine(FireLazer());
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private IEnumerator FireLazer()
    {
        while (true)
        {
            //predict location and set lazer to fire there
            Vector3 predictedLocation = player.transform.position + secondsToPredict * rb.velocity;
            //teleport the sprite there as a warning to player
            sr.enabled = true;
            transform.position = predictedLocation;
            yield return new WaitForSeconds(secondsToPredict);
            //check if player is too close to lazer, if they are respawn them
            if (Vector2.Distance(transform.position, player.transform.position) < lazerRadius)
            {
                //player was hit by lazer, respawn them
                rpc.teleportPlayerToLastRespawnPoint();
            }
            //disable the lazer's renderer again
            sr.enabled = false;
            //wait fixed amount of time until next firing, maintaining a fire rate specified above, unless its less than 0 seconds
            yield return new WaitForSeconds(Mathf.Max(0, firingFrequencyInSeconds - secondsToPredict));
        }
    }
}
