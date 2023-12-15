using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerDetection : PlayerDetector
{
    public abstract void OnPlayerIn(); 
    public abstract void OnPlayerOut(); 

    private void Update() {
        if (PlayerDetected) {
            OnPlayerIn();
        } 
        else {
            OnPlayerOut();
        }  
    } 
}
