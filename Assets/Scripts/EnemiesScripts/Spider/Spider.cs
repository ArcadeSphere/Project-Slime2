using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : PlayerDetector
{
    private Animator anim;


    private void Awake()
    {
        anim = GetComponent<Animator>();

    }
}
