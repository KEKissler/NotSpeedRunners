 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCameraController : MonoBehaviour {
    public GameObject player;
    public float baseLerpAmount;
    public float lerpAmpBySpeed;
    public float velocityPredictAmount;
    public bool freezeXPos;
    public bool freezeYPos;
    public float desiredCameraSize;
    public float maxSizeChangePercentPerSecond;


    private float fixedXPos;
    private float fixedZPos;
    private float fixedYPos;
    private GrappleMovementController gmc;
    private Rigidbody rb;
    private Camera camera;

    void Start () {
        fixedXPos = transform.position.x;
        fixedZPos = transform.position.z;
        fixedYPos = transform.position.y;
        rb = player.GetComponent<Rigidbody>();
        camera = GetComponent<Camera>();
    }
	
	void Update () {
        float lerpAmount = baseLerpAmount + lerpAmpBySpeed * Vector2.Distance((Vector2)(rb.position), (Vector2)(transform.position));
        Vector3 desiredNextCamPos =
            Vector3.LerpUnclamped(
                new Vector3(freezeXPos ? fixedXPos : rb.position.x, freezeYPos ? fixedYPos : rb.position.y, fixedZPos),

                new Vector3(freezeXPos ? fixedXPos :
                            rb.position[0] + rb.velocity[0] * velocityPredictAmount,

                            freezeYPos ? fixedYPos :
                            rb.position[1] + rb.velocity[1] * velocityPredictAmount,

                            fixedZPos),

                lerpAmount * Time.deltaTime);

        transform.position = Vector3.LerpUnclamped(transform.position, desiredNextCamPos, 0.1f);
        UpdateCameraOrthographicSize();
	}

    private void UpdateCameraOrthographicSize()
    {
        var currentCamSize = camera.orthographicSize;
        if (desiredCameraSize == currentCamSize)
        {
            return;
        }
        var maxChangeThisFrame = currentCamSize * ((maxSizeChangePercentPerSecond / 100) * Time.deltaTime);
        if (Mathf.Abs(desiredCameraSize - currentCamSize) < maxChangeThisFrame)
        {
            camera.orthographicSize = desiredCameraSize;
            return;
        }

        var shouldIncreaseSize = desiredCameraSize > currentCamSize;
        camera.orthographicSize += (shouldIncreaseSize ? 1 : -1) * maxChangeThisFrame;
    }
    
    public void updateFixedXPos(float newValue)
    {
        fixedXPos = newValue;
    }
    
    public void updateFixedYPos(float newValue)
    {
        fixedYPos = newValue;
    }
}

