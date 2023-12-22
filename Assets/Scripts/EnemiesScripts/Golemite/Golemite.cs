using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golemite : PlayerDetector
{
    private void Update() {
        if (PlayerDetected)
        {
            this.GetComponent<Animator>().SetBool("roll", true);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("roll", false);
        }
    }
}
