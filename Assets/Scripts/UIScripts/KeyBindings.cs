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

    private Button currentButton; // To track the currently selected button

    void Start()
    {
        // Add listeners to buttons
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
                    // Assign the key to the selected button's text
                    currentButton.GetComponentInChildren<Text>().text = keyCode.ToString();

                    // Reset the waiting state
                    currentButton = null;
                }
            }
        }
    }

    private void AddButtonListener(Button button)
    {
        button.onClick.AddListener(() => SetCurrentButton(button));
    }

    public void SaveKeyBindings()
    {
        // Save the updated key bindings to the InputManager
        inputManager.jumpKey = jumpButton.GetComponentInChildren<Text>().text;
        inputManager.moveLeftKey = moveLeftButton.GetComponentInChildren<Text>().text;
        inputManager.moveRightKey = moveRightButton.GetComponentInChildren<Text>().text;
        inputManager.dashKey = dashButton.GetComponentInChildren<Text>().text;
        inputManager.platformDisableKey = platformDisableButton.GetComponentInChildren<Text>().text;
        inputManager.playerAttackKey = playerAttackButton.GetComponentInChildren<Text>().text;
        inputManager.playerInteractKey = playerInteractButton.GetComponentInChildren<Text>().text;

        // Save to PlayerPrefs
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

        // Change the text to indicate that the button is waiting for input
        currentButton.GetComponentInChildren<Text>().text = "Awaiting input...";
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