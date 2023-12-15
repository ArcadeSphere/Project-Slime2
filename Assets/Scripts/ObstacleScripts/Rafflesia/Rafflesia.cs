using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rafflesia : PlayerDetection
{
    override public void OnPlayerIn() {
        Debug.Log("shoot poison");
    }

    override public void OnPlayerOut() {
        Debug.Log("stop shoot poison");
    }
}
