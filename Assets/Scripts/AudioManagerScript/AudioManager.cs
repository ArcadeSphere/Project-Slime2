using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public AudioSource soundEffectSource;
    public AudioSource musicSource;

    private float backgroundMusicVolume = 1f;  // New variable to store background music volume

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        soundEffectSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex != 0) // Skip playing background music in the main menu scene
        {
            StopBackgroundMusic();
            PlayBackgroundMusicExclusive(musicSource.clip);  // Use the stored volume
        }
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

    public void PlayBackgroundMusic(AudioClip music, float volume = 1f)
    {
        if (music != null)
        {
            musicSource.clip = music;
            musicSource.volume = volume;
            backgroundMusicVolume = volume;  // Store the volume
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Trying to play null background music.");
        }
    }

    public void PlayBackgroundMusicExclusive(AudioClip music)
    {
        if (music != null)
        {
            musicSource.clip = music;
            musicSource.volume = backgroundMusicVolume;  // Use the stored volume
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("Trying to play null background music.");
        }
    }

    public void StopBackgroundMusic()
    {
        musicSource.Stop();
    }
}