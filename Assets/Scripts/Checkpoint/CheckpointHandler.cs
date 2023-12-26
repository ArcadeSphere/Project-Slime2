using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    private bool isActivated = false;

    private void Awake() {
        DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
        {
            if (!isActivated)
            {
                isActivated = true;
                PlayerData.Instance.currentCheckpoint = this.transform;
                Debug.Log("Checkpoint Set");
            }
        }
    }
}
