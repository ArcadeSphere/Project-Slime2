using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip backgroundMusic;

    void Start()
    {
        if (AudioManager.instance != null)
        {
          
            AudioManager.instance.PlayBackgroundMusicExclusive(backgroundMusic);
        }
    }
}
