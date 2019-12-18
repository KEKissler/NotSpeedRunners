using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterimageManager : MonoBehaviour {
    public int afterimagePoolSize;
    public float afterimageTTL;
    public float emmisionDelay;
    public GameObject AfterimagePrefab;

    public bool Emitting { set { if (!value) { timeUntilNextAfterImage = 0; } emitting = value; } }

    private bool emitting = false;
    private float timeUntilNextAfterImage;
    private Transform player;
    private int index = 0;
    private readonly List<AfterimageController> afterimages = new List<AfterimageController>();
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").transform;
        for (int i = 0; i < afterimagePoolSize; ++i)
        {
            afterimages.Add(Instantiate(AfterimagePrefab).GetComponent<AfterimageController>());
        }
        foreach(var afterimage in afterimages)
        {
            afterimage.timeToLive = afterimageTTL;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!emitting)
        {
            return;
        }
        if(timeUntilNextAfterImage <= 0)
        {
            MakeNextAfterimage();
            timeUntilNextAfterImage = emmisionDelay;
        }
        timeUntilNextAfterImage -= Time.deltaTime;
	}

    private void MakeNextAfterimage()
    {
        afterimages[index++].Reset(player);
        index %= afterimagePoolSize;
    }
}
