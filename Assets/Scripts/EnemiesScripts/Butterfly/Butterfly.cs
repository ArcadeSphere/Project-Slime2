using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : PlayerDetector
{
    [Header("Butterfly Parameters")]
    [SerializeField] private EnemyPatrol enemyPatrol;
    [SerializeField] private GameObject playerGameObject;
    private SpriteRenderer sprite;
    private Rigidbody2D rb;

    private void Awake() {
        sprite = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (PlayerDetected)
        {
            if (enemyPatrol != null)
            {
                enemyPatrol.patrol = false;
            }
            ChasePlayer();
        }
        else
        {
            enemyPatrol.patrol = true;
        }
    }

    void ChasePlayer()
    {
        Vector2 direction = DirectionToTarget.normalized; 
        rb.velocity = direction * (enemyPatrol.moveSpeed * 1.3f);

        if (Vector2.Distance(playerGameObject.transform.position, transform.position) < 0.5f)
        {
            rb.velocity = Vector2.zero;
            return;
        }
    }
}
