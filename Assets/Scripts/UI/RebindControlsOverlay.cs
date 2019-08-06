using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RebindControlsOverlay : MonoBehaviour {
    public RespawnPointController RespawnPointController;
    public InputManager InputManager;
    public TextMeshProUGUI walkLeftText;
    public TextMeshProUGUI walkRightText;
    public TextMeshProUGUI grappleLeftText;
    public TextMeshProUGUI grappleRightText;
    public TextMeshProUGUI jumpText;
    public Button save;
    public Button cancel;
    public const int NUM_REBINDABLE_CONTROLS = 5;

    private int currentControlEntry = 0;
    private KeyCode[] kCodes = new KeyCode[5];
    private bool inputAllControls = false;

    public void Awake()
    {
        Debug.Log("awake rebind controls");
        RestartOverlay();
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
        var kCode = GetKeyCodePressedThisFrame();
        if (kCode != null)
        {
            kCodes[currentControlEntry] = kCode.Value;
            switch (currentControlEntry)
            {
                case 0:
                    walkLeftText.text = kCode.ToString();
                    walkRightText.text = string.Format("? (currently {0})", InputManager.walkLeft);
                    break;
                case 1:
                    walkRightText.text = kCode.ToString();
                    grappleLeftText.text = string.Format("? (currently {0})", InputManager.walkRight);
                    break;
                case 2:
                    grappleLeftText.text = kCode.ToString();
                    grappleRightText.text = string.Format("? (currently {0})", InputManager.grappleLeft);
                    break;
                case 3:
                    grappleRightText.text = kCode.ToString();
                    jumpText.text = string.Format("? (currently {0})", InputManager.grappleRight);
                    break;
                case 4:
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
            if (Input.GetKeyDown(kcode))
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
        InputManager.walkLeft = kCodes[0];
        InputManager.walkRight = kCodes[1];
        InputManager.grappleLeft = kCodes[2];
        InputManager.grappleRight = kCodes[3];
        InputManager.jump = kCodes[4];
        DisableOverlay();
    }

    private void Cancel()
    {
        DisableOverlay();
    }
}
