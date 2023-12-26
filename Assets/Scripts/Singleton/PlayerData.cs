using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
   public static PlayerData Instance;

    public Transform currentCheckpoint;

    void Awake()
    {
        Debug.Log(currentCheckpoint);
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
