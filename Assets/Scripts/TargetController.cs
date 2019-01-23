using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

    public GameObject particleEmitterPrefab;
    public float particleTTL;
    public float particleForceMult;
    [HideInInspector]
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
        GameObject toEdit = Instantiate(particleEmitterPrefab, transform.position, Quaternion.identity);
        ParticleSystem particles = toEdit.GetComponent<ParticleSystem>();
        ParticleSystem.ShapeModule s = particles.shape;
        Vector3 velo =  particleForceMult * other.transform.GetComponent<Rigidbody>().velocity;
        float angleToUse = Mathf.Atan2(velo.y, velo.x) * Mathf.Rad2Deg;
        angleToUse = (angleToUse - 90 < 0) ? angleToUse - 90 + 360 : angleToUse - 90;//unity plz
        s.rotation = new Vector3(0, 0, angleToUse);

        Destroy(toEdit, particleTTL);
    }
}
