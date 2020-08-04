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
    public event System.Action OnReinstate;
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

    public void UpdateInputOnNewLevelLoad()
    {
        HorizontalAxis += Input.GetKey(InputSettings.walkLeft) ? -1 : 0;
        HorizontalAxis += Input.GetKey(InputSettings.walkRight) ? 1 : 0;

        if (Input.GetKey(InputSettings.grapple))
        {
            SafeInvoke(OnGrappleDown);
            SafeInvoke(OnAnyInputDown);
        }
        if (Input.GetKey(InputSettings.jump))
        {
            SafeInvoke(OnJump);
            SafeInvoke(OnAnyInputDown);
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
            SafeInvoke(OnAnyInputDown);
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
            SafeInvoke(OnAnyInputDown);
        }
        if (Input.GetKeyUp(InputSettings.walkRight))
        {
            HorizontalAxis -= 1;
        }
        if (Input.GetKeyDown(InputSettings.grapple))
        {
            SafeInvoke(OnGrappleDown);
            SafeInvoke(OnAnyInputDown);
        }
        if (Input.GetKeyUp(InputSettings.grapple))
        {
            SafeInvoke(OnGrappleUp);
        }
        if (Input.GetKeyDown(InputSettings.jump))
        {
            SafeInvoke(OnJump);
            SafeInvoke(OnAnyInputDown);
        }
        if (Input.GetKeyDown(InputSettings.respawn))
        {
            SafeInvoke(OnRespawn);
        }
        if (Input.GetKeyDown(InputSettings.reinstate))
        {
            SafeInvoke(OnReinstate);
            SafeInvoke(OnAnyInputDown);
        }
    }

    private void SafeInvoke(System.Action action)
    {
        if(action != null)
        {
            action.Invoke();
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
