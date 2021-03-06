﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextUpdater : MonoBehaviour {
    public InputSettings InputSettings;
    public TMPro.TMP_Text
        Walk_right,
        Walk_left,
        Jump,
        DoubleJump,
        Grapple;

	// Use this for initialization
	void Start () {
        foo();
	}
	
	// Update is called once per frame
	void Update () {
        foo();
	}

    private void foo()
    {
        Walk_right.text = string.Format("Press\n[{0}]\nto walk right", InputSettings.walkRight);
        Walk_left.text = string.Format("Press\n[{0}]\nto walk left", InputSettings.walkLeft);
        Jump.text = string.Format("Press\n[{0}]\nto jump", InputSettings.jump);
        DoubleJump.text = string.Format("[{0}]\nin air \nto double jump!", InputSettings.jump);
        Grapple.text = string.Format("[{0}]\nto fire your\ngrappling hook!", InputSettings.grapple);
    }
}
