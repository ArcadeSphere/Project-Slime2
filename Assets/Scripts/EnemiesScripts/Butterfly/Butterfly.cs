using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : PlayerDetector
{
    [Header("Butterfly Parameters")]
    [SerializeField] private EnemyPatrol enemyPatrol;
    [SerializeField] private GameObject playerGameObject;
    [SerializeField] private float chaseDistance = 0.5f;
    private SpriteRenderer sprite;
    private Animator beeAnim;
    private Rigidbody2D rb;
    private bool playerCaught = false;

    private void Awake() {
        sprite = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        beeAnim = this.GetComponent<Animator>();
    }

    private void Update() {
        if (PlayerDetected)
        {
            if (enemyPatrol != null)
            {
                enemyPatrol.patrol = false;
            }
            ChasePlayer();
            LaunchAttack();
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

        if (Vector2.Distance(playerGameObject.transform.position, transform.position) < chaseDistance)
        {
            rb.velocity = Vector2.zero;
            playerCaught = true;
            return;
        }
    }

    void LaunchAttack()
    {
        if (playerCaught)
        {
            if (enemyPatrol.IsFacingRight())
                beeAnim.SetBool("attack", true);
            else
                beeAnim.SetBool("attack", true);
        }
    }

    void EndAttack()
    {
        if (playerCaught)
        {
            // play attack anim
            // hurt player
            beeAnim.SetBool("attack", false);
            playerCaught = false;
        }
    }
}
