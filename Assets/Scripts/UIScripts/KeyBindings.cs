using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System;
public class KeyBindings : MonoBehaviour
{
    public InputManager inputManager;

    public Button jumpButton;
    public Button moveLeftButton;
    public Button moveRightButton;
    public Button dashButton;
    public Button platformDisableButton;
    public Button playerAttackButton;
    public Button playerInteractButton;

    public Button saveButton;
    public Button restoreDefaultsButton;

    private Button currentButton;

    void Start()
    {
       //add listner here
        AddButtonListener(jumpButton);
        AddButtonListener(moveLeftButton);
        AddButtonListener(moveRightButton);
        AddButtonListener(dashButton);
        AddButtonListener(platformDisableButton);
        AddButtonListener(playerAttackButton);
        AddButtonListener(playerInteractButton);

    

        LoadKeyBindingsToButtons();
    }

    void Update()
    {
        if (currentButton != null)
        {
            // Check for any key press
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode) && keyCode != KeyCode.None)
                {  
                    currentButton.GetComponentInChildren<Text>().text = keyCode.ToString();
                    currentButton = null;
                }
            }
        }
    }

    private void AddButtonListener(Button button)
    {
        button.onClick.AddListener(() => SetCurrentButton(button));
    }

    // Save the updated key bindings to the InputManager
    public void SaveKeyBindings()
    {
        
        inputManager.jumpKey = jumpButton.GetComponentInChildren<Text>().text;
        inputManager.moveLeftKey = moveLeftButton.GetComponentInChildren<Text>().text;
        inputManager.moveRightKey = moveRightButton.GetComponentInChildren<Text>().text;
        inputManager.dashKey = dashButton.GetComponentInChildren<Text>().text;
        inputManager.platformDisableKey = platformDisableButton.GetComponentInChildren<Text>().text;
        inputManager.playerAttackKey = playerAttackButton.GetComponentInChildren<Text>().text;
        inputManager.playerInteractKey = playerInteractButton.GetComponentInChildren<Text>().text;
        inputManager.SaveKeybindings();
    }


    public void RestoreDefaults()
    {
        inputManager.LoadDefaultKeybindings();
        LoadKeyBindingsToButtons();
    }

    private void SetCurrentButton(Button button)
    {
        currentButton = button;

        
        currentButton.GetComponentInChildren<Text>().text = "Waiting";
    }

    private void LoadKeyBindingsToButtons()
    {
        // Load existing key bindings to buttons
        jumpButton.GetComponentInChildren<Text>().text = inputManager.jumpKey;
        moveLeftButton.GetComponentInChildren<Text>().text = inputManager.moveLeftKey;
        moveRightButton.GetComponentInChildren<Text>().text = inputManager.moveRightKey;
        dashButton.GetComponentInChildren<Text>().text = inputManager.dashKey;
        platformDisableButton.GetComponentInChildren<Text>().text = inputManager.platformDisableKey;
        playerAttackButton.GetComponentInChildren<Text>().text = inputManager.playerAttackKey;
        playerInteractButton.GetComponentInChildren<Text>().text = inputManager.playerInteractKey;
    }
}