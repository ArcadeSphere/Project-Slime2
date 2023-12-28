using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeUi : MonoBehaviour
{
    public Slider soundEffectsSlider;
    public Slider backgroundMusicSlider;

    [SerializeField] private AudioManager audioManager; 

    void Start()
    {
        // Initialize sliders with current volume levels
        if (audioManager != null)
        {
            if (soundEffectsSlider != null)
            {
                soundEffectsSlider.value = audioManager.soundEffectSource.volume;
                soundEffectsSlider.onValueChanged.AddListener(ChangeSoundEffectsVolume);
            }

            if (backgroundMusicSlider != null)
            {
                backgroundMusicSlider.value = audioManager.musicSource.volume;
                backgroundMusicSlider.onValueChanged.AddListener(ChangeBackgroundMusicVolume);
            }
        }
    }

   public void ChangeSoundEffectsVolume(float volume)
    {
        audioManager.soundEffectSource.volume = volume;
    }

    public void ChangeBackgroundMusicVolume(float volume)
    {
        audioManager.musicSource.volume = volume;
    }
}
