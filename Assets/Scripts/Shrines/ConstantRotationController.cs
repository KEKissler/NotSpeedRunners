using UnityEngine;

public class ConstantRotationController : MonoBehaviour
{
    public Vector3 rotationAngleChangePerSecond;
    public float speedMultiplier;
    
    private Quaternion rotation;
    
    public void Start()
    {
        rotation.eulerAngles = Time.fixedDeltaTime * rotationAngleChangePerSecond;
    }

    public void FixedUpdate()
    {
        transform.rotation *= Quaternion.LerpUnclamped(Quaternion.identity, rotation, speedMultiplier);
    }
}
