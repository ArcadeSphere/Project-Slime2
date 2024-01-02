using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    // Define your input here as strings
    public string jumpKey = "Jump";
    public string moveLeftKey = "a";
    public string moveRightKey = "d";
    public string dashKey = "LeftShift";
    public string platformDisableKey = "s";
    public string playerAttackKey = "j"; 

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        LoadKeybindings();
       }

    public bool GetJumpInput()
    {
        return Input.GetKeyDown(GetKeyCode(jumpKey));
    }

    public bool GetJumpInputUp()
    {
        return Input.GetKeyUp(GetKeyCode(jumpKey));
    }

    public float GetHorizontalInput()
    {
        float horizontalInput = 0f;

        if (Input.GetKey(GetKeyCode(moveLeftKey)))
        {
            horizontalInput -= 1f;
        }

        if (Input.GetKey(GetKeyCode(moveRightKey)))
        {
            horizontalInput += 1f;
        }

        return horizontalInput;
    }

    public bool GetDashInputDown()
    {
        return Input.GetKeyDown(GetKeyCode(dashKey));
    }

    public bool GetPlatformDisableInputDown()
    {
        return Input.GetKeyDown(GetKeyCode(platformDisableKey));
    }

    public bool GetPlayerAttackInputDown()
    {
        return Input.GetKeyDown(GetKeyCode(playerAttackKey));
    }

    // Save keybindings to PlayerPrefs
    public void SaveKeybindings()
    {
        PlayerPrefs.SetString("JumpKey", jumpKey);
        PlayerPrefs.SetString("MoveLeftKey", moveLeftKey);
        PlayerPrefs.SetString("MoveRightKey", moveRightKey);
        PlayerPrefs.SetString("DashKey", dashKey);
        PlayerPrefs.SetString("PlatformDisableKey", platformDisableKey);
        PlayerPrefs.SetString("PlayerAttackKey", playerAttackKey);
    }

    // Load keybindings
    public void LoadKeybindings()
    {
        jumpKey = PlayerPrefs.GetString("JumpKey", "space");
        moveLeftKey = PlayerPrefs.GetString("MoveLeftKey", "a");
        moveRightKey = PlayerPrefs.GetString("MoveRightKey", "d");
        dashKey = PlayerPrefs.GetString("DashKey", "LeftShift");
        platformDisableKey = PlayerPrefs.GetString("PlatformDisableKey", "s");
        playerAttackKey = PlayerPrefs.GetString("PlayerAttackKey", "j");
    }
    // Load the defualt keybindings
    public void LoadDefaultKeybindings()
    {
        jumpKey = "space";
        moveLeftKey = "a";
        moveRightKey = "d";
        dashKey = "LeftShift";
        platformDisableKey = "s";
        playerAttackKey = "Fire1";
    }


// Method to convert string to KeyCode
public KeyCode GetKeyCode(string keyName)
    {
        return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyName, true);
    }
}
