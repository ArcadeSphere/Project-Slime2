using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    [SerializeField] private PlayerCombat playerCombat;
    private bool isPaused = false;

    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                TogglePause();
            }
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        pauseMenuUI.SetActive(true);
       playerCombat.enabled = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        playerCombat.enabled = true;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        isPaused = false;
    }
    public void RestartGame()
    {
        playerCombat.enabled = true;
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
    public void ToMainMenu()
    {
        playerCombat.enabled = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
