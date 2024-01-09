using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerMovement playerMove;
    public GameObject optionsMenuUI;
    public GameObject volumeMenuUI;
    public GameObject keybindingsMenuUI;

    private enum MainMenuState
    {
        Main,
        Options,
        Volume,
        Keybindings
    }

    private MainMenuState currentState = MainMenuState.Main;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == MainMenuState.Options || currentState == MainMenuState.Volume || currentState == MainMenuState.Keybindings)
            {
                BackToMainMenu();
            }
        }
    }

    public void PlayGame()
    {
        playerCombat.enabled = true;
        playerMove.enabled = true;
        PlayerData.Instance.currentCheckpoint = null;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void QuitTheGame()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        currentState = MainMenuState.Options;
        optionsMenuUI.SetActive(true);
    }

    public void OpenVolume()
    {
        currentState = MainMenuState.Volume;
        volumeMenuUI.SetActive(true);
    }

    public void OpenKeybindings()
    {
        currentState = MainMenuState.Keybindings;
        keybindingsMenuUI.SetActive(true);
    }

    public void BackToMainMenu()
    {
        currentState = MainMenuState.Main;
        optionsMenuUI.SetActive(false);
        volumeMenuUI.SetActive(false);
        keybindingsMenuUI.SetActive(false);
    }
}
