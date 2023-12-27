using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    private AudioSource soundEffectSource;
    private AudioSource musicSource;
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
        musicSource = gameObject.AddComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
        StopBackgroundMusic();

       
        PlayBackgroundMusic(musicSource.clip, musicSource.volume);
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
