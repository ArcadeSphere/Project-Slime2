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
        Options
    }

    private GameState currentState = GameState.Playing;

    [SerializeField] private PlayerCombat playerCombat;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;

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
            else if (currentState == GameState.Options)
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
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        playerCombat.enabled = true;
    }

    public void OpenOptions()
    {
        currentState = GameState.Options;
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }

    public void BackToPauseMenu()
    {
        currentState = GameState.Paused;
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void RestartGame()
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