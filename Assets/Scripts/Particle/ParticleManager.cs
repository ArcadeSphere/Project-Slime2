using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    public static ParticleManager Instance { get; private set; }
    [Header("Particle")]
    public ParticleSystem jumpParticle;
    public ParticleSystem dashParticle;
    
    private void Awake() {
        Instance = this;
    }

    public void Play(ParticleSystem particle)
    {
        particle.Play();
    }
}
