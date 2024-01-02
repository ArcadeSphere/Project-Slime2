using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private PlayerCombat playerCombat;
    [SerializeField] private PlayerMovement playerMove;
    public void PlayGame()
    {
        playerCombat.enabled = true;
        playerMove.enabled = true;
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
    public void QuitTheGame()
    {
        Application.Quit();
    }
}
