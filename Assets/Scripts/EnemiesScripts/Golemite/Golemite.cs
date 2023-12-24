using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golemite : PlayerDetector
{
    [Header("Golemite Parameters")]
    [SerializeField] private EnemyPatrol enemyPatrol;
    private Animator golemiteAnimator;

    private void Awake() {
        golemiteAnimator = this.GetComponent<Animator>();
    }
    private void Update() 
    {
        if (PlayerDetected)
        {
            if (enemyPatrol.onEdge) 
            {
                golemiteAnimator.SetBool("roll", false);
                return;
            }
            golemiteAnimator.SetBool("roll", true);
        }
        else
        {
            if (golemiteAnimator.GetBool("roll") && !enemyPatrol.onEdge)
            {
                golemiteAnimator.SetBool("roll", true);
                return;
            }
            golemiteAnimator.SetBool("roll", false);
        }
    }
    
}
