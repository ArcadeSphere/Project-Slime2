using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TutorialTexts : MonoBehaviour
{
    public enum DisplayFunction
    {
        MoveKeys,
        PlayerAttackButton,
        Jump,
        Dash,
        Encourage
          
    }

    [SerializeField] private InputManager inputManager;
    [SerializeField] private TextMeshPro textMeshPro;
    [Header("GIVE A FUNCTION")]
    public DisplayFunction displayFunction = DisplayFunction.MoveKeys;

    private void Start()
    {
        if (inputManager == null)
        {
            Debug.LogError("InputManager reference is missing!");
        }

        if (textMeshPro == null)
        {
            Debug.LogError("TextMeshPro reference is missing!");
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
            case DisplayFunction.Jump:
                DisplayJump();
                break;
            case DisplayFunction.Dash:
                DisplayDash();
                break;
            case DisplayFunction.Encourage:
                DisplayEncouragement();
                break;
            default:
                Debug.LogError("Invalid DisplayFunction selected.");
                break;
        }
    }

    private void DisplayMoveKeys()
    {
        if (inputManager != null && textMeshPro != null)
        {
            string moveLeftText = $"Move Left: {inputManager.moveLeftKey}";
            string moveRightText = $"Move Right: {inputManager.moveRightKey}";

            textMeshPro.text = $"{moveLeftText}\n{moveRightText}";
        }
    }

    private void DisplayPlayerAttackButton()
    {
        if (inputManager != null && textMeshPro != null)
        {
            string playerAttackText = $"Attack: {inputManager.playerAttackKey}";

            textMeshPro.text = playerAttackText;
        }
    }

    private void DisplayJump()
    {
        if (inputManager != null && textMeshPro != null)
        {
            string jumpText = $"Jump: {inputManager.jumpKey}";
            string jumpHoldText = $"Hold {inputManager.jumpKey} To Jump Higher";

            textMeshPro.text = $"{jumpText}\n{jumpHoldText}";
        }
    }

    private void DisplayDash()
    {
        if (inputManager != null && textMeshPro != null)
        {
            string dashText = $"Dash: {inputManager.dashKey}";

            textMeshPro.text = dashText;
        }
    }
    private void DisplayEncouragement()
    {
        if (inputManager != null && textMeshPro != null)
        {
            string encourageText = $"Save Ryan Gosling";

            textMeshPro.text = encourageText;
        }
    }

}