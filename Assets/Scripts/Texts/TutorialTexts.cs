using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTexts : MonoBehaviour
{
    public enum DisplayFunction
    {
        MoveKeys,
        PlayerAttackButton
    }

    [SerializeField] private InputManager inputManager;
    [SerializeField] private TextMesh textMesh;
    public DisplayFunction displayFunction = DisplayFunction.MoveKeys;

    private void Start()
    {
        if (inputManager == null)
        {
            Debug.LogError("InputManager reference is missing!");
        }

        if (textMesh == null)
        {
            Debug.LogError("TextMesh reference is missing!");
        }
    }

    private void Update()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        switch (displayFunction)
        {
            case DisplayFunction.MoveKeys:
                DisplayMoveKeys();
                break;
            case DisplayFunction.PlayerAttackButton:
                DisplayPlayerAttackButton();
                break;
            default:
                Debug.LogError("Invalid DisplayFunction selected.");
                break;
        }
    }

    private void DisplayMoveKeys()
    {
        if (inputManager != null && textMesh != null)
        {
            string moveLeftText = $"Move Left: {inputManager.moveLeftKey}";
            string moveRightText = $"Move Right: {inputManager.moveRightKey}";

            textMesh.text = $"{moveLeftText}\n{moveRightText}";
        }
    }

    private void DisplayPlayerAttackButton()
    {
        if (inputManager != null && textMesh != null)
        {
            string playerAttackText = $"Player Attack: {inputManager.playerAttackKey}";

            textMesh.text = playerAttackText;
        }
    }
}