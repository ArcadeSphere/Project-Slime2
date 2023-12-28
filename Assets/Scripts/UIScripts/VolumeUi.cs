using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeUi : MonoBehaviour
{
    public Slider soundEffectsSlider;
    public Slider backgroundMusicSlider;

    void Start()
    {
        if (AudioManager.instance != null)
        {
            if (soundEffectsSlider != null)
            {
                soundEffectsSlider.value = AudioManager.instance.soundEffectSource.volume;
                soundEffectsSlider.onValueChanged.AddListener(ChangeSoundEffectsVolume);
            }

            if (backgroundMusicSlider != null)
            {
                backgroundMusicSlider.value = AudioManager.instance.musicSource.volume;
                backgroundMusicSlider.onValueChanged.AddListener(ChangeBackgroundMusicVolume);
            }
        }
    }

    public void ChangeSoundEffectsVolume(float volume)
    {
        AudioManager.instance.soundEffectSource.volume = volume;
    }

    public void ChangeBackgroundMusicVolume(float volume)
    {
        AudioManager.instance.musicSource.volume = volume;
    }
}