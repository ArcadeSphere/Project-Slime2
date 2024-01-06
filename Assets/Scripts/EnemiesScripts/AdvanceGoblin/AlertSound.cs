using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class AlertSound : MonoBehaviour
{
    public GameObject alertObject;
    private bool hasPlayedSound = false;
    [SerializeField] AudioClip alertSound;


    void Update()
    {
        // Check if the target object is active
        if (alertObject.activeSelf)
        {
            if (!hasPlayedSound)
            {
                AudioManager.instance.PlaySoundEffects(alertSound);
                hasPlayedSound = true;
            }
        }
        else
        {
        
            hasPlayedSound = false;
        }
    }
}