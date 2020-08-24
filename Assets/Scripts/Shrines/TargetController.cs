using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour {

    public GameObject particleEmitterPrefab;
    public float particleTTL;
    public float particleForceMult;

    private TargetManager TargetManager;

    void Start()
    {
        TargetManager = GameObject.Find("TargetManager").GetComponent<TargetManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        gameObject.SetActive(false);
        --TargetManager.RemainingTargets;
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
