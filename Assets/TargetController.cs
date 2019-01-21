using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

	public static int RemainingTargets = 0;
    private TimeManager tm;

    void Start()
    {
        ++RemainingTargets;
        tm = GameObject.Find("TimeManager").GetComponent<TimeManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        --RemainingTargets;
        if(RemainingTargets == 0)
        {
            tm.OnEndCurrentTimer();
        }
    }
}
