using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebindControlsOverlay : MonoBehaviour {
    public RespawnPointController RespawnPointController;
    public InputSettings InputSettings;
    public TextMeshProUGUI walkLeftText;
    public TextMeshProUGUI walkRightText;
    public TextMeshProUGUI grappleText;
    public TextMeshProUGUI jumpText;
    public Button save;
    public Button cancel;
    public const int NUM_REBINDABLE_CONTROLS = 4;

    private int currentControlEntry = 0;
    private KeyCode[] kCodes = new KeyCode[5];
    private bool inputAllControls = false;

    public void Setup()
    {
        Debug.Log("awake rebind controls");
        save.onClick.AddListener(SaveControls);
        cancel.onClick.AddListener(Cancel);
    }

    public void RestartOverlay()
    {
        //RespawnPointController.blockers += 1;
        Time.timeScale = 0;
        currentControlEntry = 0;
        gameObject.SetActive(true);
        SetActiveEndButtons(false);
        inputAllControls = false;
        walkLeftText.text = string.Format("? (currently {0})", InputSettings.walkLeft);
        walkRightText.text = "";
        grappleText.text = "";
        jumpText.text = "";

    }

    private void DisableOverlay()
    {
        //RespawnPointController.blockers -= 1;
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if (inputAllControls)
        {
            return;
        }
        CheckForEscape();
        var kCode = GetKeyCodePressedThisFrame();
        if (kCode != null)
        {
            kCodes[currentControlEntry] = kCode.Value;
            switch (currentControlEntry)
            {
                case 0:
                    walkLeftText.text = kCode.ToString();
                    walkRightText.text = string.Format("? (currently {0})", InputSettings.walkLeft);
                    break;
                case 1:
                    walkRightText.text = kCode.ToString();
                    grappleText.text = string.Format("? (currently {0})", InputSettings.walkRight);
                    break;
                case 2:
                    grappleText.text = kCode.ToString();
                    jumpText.text = string.Format("? (currently {0})", InputSettings.grapple);
                    break;
                case 3:
                    jumpText.text = kCode.ToString();
                    SetActiveEndButtons(true);
                    inputAllControls = true;
                    break;
                default:
                    throw new Exception(string.Format("Invalid control rebinding index: {0}", currentControlEntry));
            }
            ++currentControlEntry;
            if(currentControlEntry > NUM_REBINDABLE_CONTROLS)
            {
                SetActiveEndButtons(true);
                return;
            }
        }
    }

    private KeyCode? GetKeyCodePressedThisFrame()
    {
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode) && kcode != KeyCode.Escape)
                return kcode;
        }
        return null;
    }

    private void SetActiveEndButtons(bool enabled)
    {
        cancel.gameObject.SetActive(enabled);
        save.gameObject.SetActive(enabled);
    }

    private void SaveControls()
    {
        InputSettings.walkLeft = kCodes[0];
        InputSettings.walkRight = kCodes[1];
        InputSettings.grapple = kCodes[2];
        InputSettings.jump = kCodes[3];
        DisableOverlay();
    }

    private void Cancel()
    {
        DisableOverlay();
    }

    private void CheckForEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cancel();
        }
    }
}
