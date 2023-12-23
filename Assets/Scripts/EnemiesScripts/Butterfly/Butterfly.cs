using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : PlayerDetector
{
    [Header("Butterfly Parameters")]
    [SerializeField] private EnemyPatrol enemyPatrol;
    private Rigidbody2D rb;

    private void Awake() {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (PlayerDetected)
        {
            if (enemyPatrol != null)
            {
                enemyPatrol.patrol = false;
            }
            Vector2 direction = DirectionToTarget.normalized; 
            rb.velocity = direction * (enemyPatrol.moveSpeed * 1.3f);
        }
        else
        {
            enemyPatrol.patrol = true;
        }
    }


}
