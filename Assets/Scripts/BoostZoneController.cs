using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class BoostZoneController : MonoBehaviour {

    public Vector2 setVelocity;
    public float particlesPerSecondPerArea;
    public float particleVelocityMagnitudeMultiplier;
    public float particleLifetimeDistance;

    private Rigidbody rb;
    private PlayerController pc;
    private GameObject player;
    private ParticleSystem particleSystem;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player");
        pc = player.GetComponent<PlayerController>();
        rb = player.GetComponent<Rigidbody>();
        particleSystem = transform.GetChild(0).GetComponent<ParticleSystem>();
        InitializeParticleSystem();
    }

    private void InitializeParticleSystem()
    {
        var allParticleModules = particleSystem.GetComponent<ParticleSystem>();
        var shape = allParticleModules.shape;

        //velocity
        var velocityOverLifetime = allParticleModules.velocityOverLifetime;
        velocityOverLifetime.x = setVelocity.x * particleVelocityMagnitudeMultiplier;
        velocityOverLifetime.y = setVelocity.y * particleVelocityMagnitudeMultiplier;
        velocityOverLifetime.z = 0;
        
        //spawn box
        shape.shapeType = ParticleSystemShapeType.Box;
        var lossyScale = transform.lossyScale;
        shape.scale = lossyScale;

        //particle distance
        var mainModule = allParticleModules.main;
        var totalMagnitude = setVelocity.magnitude * particleVelocityMagnitudeMultiplier;
        if (particleLifetimeDistance != 0 && totalMagnitude != 0)
        {
            mainModule.startLifetime = particleLifetimeDistance / totalMagnitude;
        }
        
        //density
        var emissionModule = allParticleModules.emission;
        emissionModule.rateOverTime = lossyScale.x * lossyScale.y * particlesPerSecondPerArea;

    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            pc.releaseGrapple();
            rb.velocity = setVelocity;
        }
    }
}
