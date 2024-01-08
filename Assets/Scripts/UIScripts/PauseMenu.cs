using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    private enum GameState
    {
        Playing,
        Paused,
        Options,
        Volume,
        Keybindings
    }

    private GameState currentState = GameState.Playing;

    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerMovement playerMove;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject volumeMenuUI;
    public GameObject keybindingsMenuUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentState == GameState.Playing)
            {
                PauseGame();
            }
            else if (currentState == GameState.Paused)
            {
                ResumeGame();
            }
            else if (currentState == GameState.Options || currentState == GameState.Volume || currentState == GameState.Keybindings)
            {
                BackToPauseMenu();
            }
        }
    }

    void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        playerCombat.enabled = false;
        playerMove.enabled = false;
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        volumeMenuUI.SetActive(false);
        keybindingsMenuUI.SetActive(false);
        playerCombat.enabled = true;
        playerMove.enabled = true;
    }

    public void OpenOptions()
    {
        currentState = GameState.Options;
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);

    }

    public void OpenVolume()
    {
        currentState = GameState.Volume;
        volumeMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void OpenKeybindings()
    {
        currentState = GameState.Keybindings;
        keybindingsMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void BackToPauseMenu()
    {
        currentState = GameState.Paused;
        optionsMenuUI.SetActive(false);
        volumeMenuUI.SetActive(false);
        keybindingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void RestartGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        CheckpointHandler.Instance.ResetCheckpointPrefValue();
        PlayerData.Instance.currentCheckpoint = null;
        SceneManager.LoadScene(currentScene.name);
    }

    public void RestartGameFromCheckPoint()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}