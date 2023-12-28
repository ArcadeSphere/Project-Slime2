using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public  AudioSource soundSource;
    public AudioSource musicSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy duplicate AudioManager
            Destroy(gameObject);
            return;
        }

        soundSource = GetComponent<AudioSource>();
        musicSource = transform.Find("Music").GetComponent<AudioSource>(); // Update the child object name accordingly

        LoadVolumesFromPlayerPrefs();
    }

    private void Start()
    {
        // No sliders, so no need for listeners here
    }

    private void LoadVolumesFromPlayerPrefs()
    {
        // Load initial volumes from PlayerPrefs
        soundSource.volume = PlayerPrefs.GetFloat("soundVolume", 1);
        musicSource.volume = PlayerPrefs.GetFloat("musicVolume", 0.3f);
    }

    public void PlaySoundEffects(AudioClip sound)
    {
        soundSource.PlayOneShot(sound);
    }

    public void ChangeSoundVolume(float value)
    {
        soundSource.volume = value;
    }

    public void ChangeMusicVolume(float value)
    {
        musicSource.volume = value;
    }
}