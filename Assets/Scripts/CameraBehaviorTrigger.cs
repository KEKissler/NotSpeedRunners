using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviorTrigger : MonoBehaviour {
    public bool freezeX, freezeY;
    public float fixedX, fixedY;
    public float size = 12;
    private BasicCameraController basicCameraController;
    
    public void Start()
    {
        basicCameraController = FindObjectOfType<BasicCameraController>();
    }

    public void OnTriggerEnter(Collider other)
    {
        basicCameraController.updateFixedXPos(fixedX);
        basicCameraController.updateFixedYPos(fixedY);
        basicCameraController.freezeYPos = freezeY;
        basicCameraController.freezeXPos = freezeX;
        basicCameraController.desiredCameraSize = size;
    }
}
