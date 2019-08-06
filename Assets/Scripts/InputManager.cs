using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public InputSettings InputSettings;
    public event System.Action OnMoveLeftDown;
    public event System.Action OnMoveRightDown;
    public event System.Action OnGrappleDown;
    public event System.Action OnGrappleUp;
    public event System.Action OnJump;
    public event System.Action OnAnyInputDown;
    public event System.Action OnRespawn;
    public static InputManager instance;
    public int HorizontalAxis = 0;

    void Awake () {
		if(instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(InputSettings.walkLeft))
        {
            if(OnMoveLeftDown != null)
            {
                HorizontalAxis -= 1;
                OnMoveLeftDown.Invoke();
            }
            if(OnAnyInputDown != null)
            {
                OnAnyInputDown.Invoke();
            }
        }
        if (Input.GetKeyUp(InputSettings.walkLeft))
        {
            HorizontalAxis += 1;
        }
        if (Input.GetKeyDown(InputSettings.walkRight))
        {
            if (OnMoveRightDown != null)
            {
                HorizontalAxis += 1;
                OnMoveRightDown.Invoke();
            }
            if (OnAnyInputDown != null)
            {
                OnAnyInputDown.Invoke();
            }
        }
        if (Input.GetKeyUp(InputSettings.walkRight))
        {
            HorizontalAxis -= 1;
        }
        if (Input.GetKeyDown(InputSettings.grapple))
        {
            if (OnGrappleDown != null)
            {
                OnGrappleDown.Invoke();
            }
            if (OnAnyInputDown != null)
            {
                OnAnyInputDown.Invoke();
            }
        }
        if (Input.GetKeyUp(InputSettings.grapple))
        {
            if (OnGrappleUp != null)
            {
                OnGrappleUp.Invoke();
            }
        }
        if (Input.GetKeyDown(InputSettings.jump))
        {
            if (OnJump != null)
            {
                OnJump.Invoke();
            }
            if (OnAnyInputDown != null)
            {
                OnAnyInputDown.Invoke();
            }
        }
        if (Input.GetKeyDown(InputSettings.respawn))
        {
            if (OnRespawn != null)
            {
                OnRespawn.Invoke();
            }
        }
    }

    public void PlayerFellOutOfBounds()
    {
        if (OnRespawn != null)
        {
            OnRespawn.Invoke();
        }
    }
}
