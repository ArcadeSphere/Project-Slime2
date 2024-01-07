using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreCheckpoint : MonoBehaviour
{
    void Start()
    {
        if (PlayerData.Instance.currentCheckpoint != null)
            transform.position = PlayerData.Instance.currentCheckpoint.position + new Vector3(0, 2, 0);
    }
    
    // called from dead animation
    void RestartFromCheckpoint()
    {
        if (SceneTransition.Instance){
            SceneTransition.Instance.TransitionToActiveScene();
        }
    }
}
