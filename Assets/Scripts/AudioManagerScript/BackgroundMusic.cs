using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
   [SerializeField] private AudioClip backgroundMusic;

    void Start()
    {
        AudioManager.instance.PlayBackgroundMusic(backgroundMusic);
    }


}
