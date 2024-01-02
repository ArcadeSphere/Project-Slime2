using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
public class KeyBindings : MonoBehaviour
{
    public InputManager inputManager;

    public InputField jumpInputField;
    public InputField moveLeftInputField;
    public InputField moveRightInputField;
    public InputField dashInputField;
    public InputField platformDisableInputField;
    public InputField playerAttackInputField;
    public InputField playerInteractInputField;

    public Button saveButton;
    public Button restoreDefaultsButton;

    void Start()
    {
        LoadKeyBindingsToUI();

        // Add listeners to input fields to capitalize the first letter
        jumpInputField.onValueChanged.AddListener(delegate { CapitalizeFirstLetter(jumpInputField); });
        moveLeftInputField.onValueChanged.AddListener(delegate { CapitalizeFirstLetter(moveLeftInputField); });
        moveRightInputField.onValueChanged.AddListener(delegate { CapitalizeFirstLetter(moveRightInputField); });
        dashInputField.onValueChanged.AddListener(delegate { CapitalizeFirstLetter(dashInputField); });
        platformDisableInputField.onValueChanged.AddListener(delegate { CapitalizeFirstLetter(platformDisableInputField); });
        playerAttackInputField.onValueChanged.AddListener(delegate { CapitalizeFirstLetter(playerAttackInputField); });
        playerInteractInputField.onValueChanged.AddListener(delegate { CapitalizeFirstLetter(playerInteractInputField); });
    }

    private void LoadKeyBindingsToUI()
    {
        // Load existing key bindings to UI
        jumpInputField.text = inputManager.jumpKey;
        moveLeftInputField.text = inputManager.moveLeftKey;
        moveRightInputField.text = inputManager.moveRightKey;
        dashInputField.text = inputManager.dashKey;
        platformDisableInputField.text = inputManager.platformDisableKey;
        playerAttackInputField.text = inputManager.playerAttackKey;
        playerInteractInputField.text = inputManager.playerInteractKey;
    }

    public void SaveKeyBindings()
    {
        // Save the updated key bindings to the InputManager
        inputManager.jumpKey = jumpInputField.text;
        inputManager.moveLeftKey = moveLeftInputField.text;
        inputManager.moveRightKey = moveRightInputField.text;
        inputManager.dashKey = dashInputField.text;
        inputManager.platformDisableKey = platformDisableInputField.text;
        inputManager.playerAttackKey = playerAttackInputField.text;
        inputManager.playerInteractKey = playerInteractInputField.text;

        // Capitalize first letter in case it was changed manually
        CapitalizeFirstLetter(jumpInputField);
        CapitalizeFirstLetter(moveLeftInputField);
        CapitalizeFirstLetter(moveRightInputField);
        CapitalizeFirstLetter(dashInputField);
        CapitalizeFirstLetter(platformDisableInputField);
        CapitalizeFirstLetter(playerAttackInputField);
        CapitalizeFirstLetter(playerInteractInputField);

        inputManager.SaveKeybindings();
    }

    public void RestoreDefaults()
    {
        inputManager.LoadDefaultKeybindings();
        LoadKeyBindingsToUI(); // Update UI with default values
    }

    private void CapitalizeFirstLetter(InputField inputField)
    {
        if (!string.IsNullOrEmpty(inputField.text) && char.IsLower(inputField.text[0]))
        {
            inputField.text = char.ToUpper(inputField.text[0]) + inputField.text.Substring(1);
        }
    }
}