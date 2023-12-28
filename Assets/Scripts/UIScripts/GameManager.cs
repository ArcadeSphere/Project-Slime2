using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject audioManagerPrefab;
    public Slider soundSlider;
    public Slider musicSlider;

    private AudioManager audioManager;
    private float soundVolume = 1f;
    private float musicVolume = 0.3f;

    private void Awake()
    {
        InitializeAudioManager();
        InitializeSliders();
    }

    private void InitializeAudioManager()
    {
        // Check if AudioManager.instance is null
        if (AudioManager.instance == null)
        {
            // Instantiate AudioManager prefab dynamically
            if (audioManagerPrefab != null)
            {
                GameObject audioManagerInstance = Instantiate(audioManagerPrefab);

                // Optional: Parent the AudioManager instance to the GameManager or another suitable object
                audioManagerInstance.transform.SetParent(transform);
                audioManager = audioManagerInstance.GetComponent<AudioManager>();
            }
            else
            {
                Debug.LogError("AudioManagerPrefab field is not set in the GameManager. Drag AudioManagerPrefab into this field in the Inspector.");
            }
        }
        else
        {
            audioManager = AudioManager.instance;
        }
    }

    private void InitializeSliders()
    {
        // Optionally set initial slider values
        soundSlider.value = audioManager.soundSource.volume;
        musicSlider.value = audioManager.musicSource.volume;

        // Add listeners for slider value changes
        soundSlider.onValueChanged.AddListener(ChangeSoundVolume);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
    }

    // Add methods for getting/setting sound and music volumes
    public float GetSoundVolume()
    {
        return soundVolume;
    }

    public void SetSoundVolume(float volume)
    {
        soundVolume = volume;
        PlayerPrefs.SetFloat("soundVolume", volume);
        PlayerPrefs.Save();

        // Update AudioManager volume
        if (audioManager != null)
        {
            audioManager.ChangeSoundVolume(volume);
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();

        // Update AudioManager volume
        if (audioManager != null)
        {
            audioManager.ChangeMusicVolume(volume);
        }
    }

    private void ChangeSoundVolume(float value)
    {
        SetSoundVolume(value);
    }

    private void ChangeMusicVolume(float value)
    {
        SetMusicVolume(value);
    }
}