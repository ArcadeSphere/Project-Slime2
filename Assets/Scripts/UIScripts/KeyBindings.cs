using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeyBindings : MonoBehaviour
{
    public InputManager inputManager;

    public InputField jumpInputField;
    public InputField moveLeftInputField;
    public InputField moveRightInputField;
    public InputField dashInputField;
    public InputField platformDisableInputField;
    public InputField playerAttackInputField;

    public Button saveButton;
    public Button restoreDefaultsButton;

    void Start()
    {
        LoadKeyBindingsToUI();
    }

    private void LoadKeyBindingsToUI()
    {
        
        jumpInputField.text = inputManager.jumpKey;
        moveLeftInputField.text = inputManager.moveLeftKey;
        moveRightInputField.text = inputManager.moveRightKey;
        dashInputField.text = inputManager.dashKey;
        platformDisableInputField.text = inputManager.platformDisableKey;
        playerAttackInputField.text = inputManager.playerAttackKey;
    }

    public void SaveKeyBindings()
    {
       
        inputManager.jumpKey = jumpInputField.text;
        inputManager.moveLeftKey = moveLeftInputField.text;
        inputManager.moveRightKey = moveRightInputField.text;
        inputManager.dashKey = dashInputField.text;
        inputManager.platformDisableKey = platformDisableInputField.text;
        inputManager.playerAttackKey = playerAttackInputField.text;

        inputManager.SaveKeybindings();
    }

    public void RestoreDefaults()
    {
        inputManager.LoadDefaultKeybindings();
        LoadKeyBindingsToUI(); 
    }
}