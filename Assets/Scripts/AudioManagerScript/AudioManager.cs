using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    private AudioSource soundEffectSource;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        soundEffectSource = gameObject.AddComponent<AudioSource>();
    }
    public void PlaySoundEffects(AudioClip soundEffect)
    {
        if (soundEffect != null)
        {
            soundEffectSource.PlayOneShot(soundEffect);
        }
        else
        {
            Debug.LogWarning("No sound");
        }
    }
}
