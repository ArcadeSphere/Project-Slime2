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
    private bool caughtPlayer;

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
            if (caughtPlayer)
                LaunchAttack();
            else
                beeAnim.SetInteger("state", 0);

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
            caughtPlayer = true;
            return;
        }
    }

    void LaunchAttack()
    {
        if (enemyPatrol.IsFacingRight())
            beeAnim.SetInteger("state", 2);
        else
            beeAnim.SetInteger("state", 1);
        caughtPlayer = false;
    }

    void DestroyEnemy()
    {
        Debug.Log("deleted");
        Destroy(this.gameObject);
    }
}
