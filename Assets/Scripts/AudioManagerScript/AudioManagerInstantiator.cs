using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerInstantiator : MonoBehaviour
{
    public GameObject audioManagerPrefab;

    void Awake()
    {
        if (AudioManager.instance == null)
        {
            Instantiate(audioManagerPrefab);
        }
    }
}