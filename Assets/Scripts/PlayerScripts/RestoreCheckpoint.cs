using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestoreCheckpoint : MonoBehaviour
{
    public GameObject gameOverScreen;

    public void CallGameOverScreen()
    {
        gameOverScreen.SetActive(true);
    }
    // not yet implemented, right now it just restarts the game
    void RestartFromCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
