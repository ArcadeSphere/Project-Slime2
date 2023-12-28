using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{

    public static Particle Instance { get; private set; }
    [Header("Particle")]
    public ParticleSystem jumpParticle;
    
    private void Awake() {
        Instance = this;
    }

    public void Play(ParticleSystem particle)
    {
        particle.Play();
    }
}
