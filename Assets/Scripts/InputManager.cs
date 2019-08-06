using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public InputSettings InputSettings;
    public event System.Action OnMoveLeftDown;
    public event System.Action OnMoveRightDown;
    public static InputManager instance;

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
                OnMoveLeftDown.Invoke();
            }
        }
        if (Input.GetKeyDown(InputSettings.walkRight))
        {
            if(OnMoveRightDown != null)
            {
                OnMoveRightDown.Invoke();
            }
        }
	}
}
