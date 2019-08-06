using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayManager : MonoBehaviour {

    public RebindControlsOverlay RebindControlsOverlay;

	void Start()
    {
        RebindControlsOverlay.Setup();
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            RebindControlsOverlay.RestartOverlay();
        }
	}
}
