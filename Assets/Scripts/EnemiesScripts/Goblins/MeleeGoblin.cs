using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGoblin :PlayerDetector
{
    private Animator anim;

    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackPoint;
    private bool isAttacking = false;
    [SerializeField] private float attackCooldown = 2f;
    private float currentCooldown = 0f;
    public LayerMask playerLayer;
    [SerializeField] private float damageAmount;

    [Header("Patrol Settings")]
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float patrolSpeed = 2f;
    private bool isPatrolling = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PlayerDetected)
        {
            StopPatrolAndAttack();
        }
        else
        {
            if (isPatrolling)
            {
                Patrol();
            }
            else
            {
                DontAttackPlayer();
            }
        }
    }

    private void Patrol()
    {
        // Patrol logic
        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
        anim.SetFloat("moveSpeed", Mathf.Abs(patrolSpeed));

      
        if ((patrolSpeed > 0 && transform.position.x >= rightPoint.position.x) ||
            (patrolSpeed < 0 && transform.position.x <= leftPoint.position.x))
        {
            Flip();
        }
    }

    private void StopPatrolAndAttack()
    {
        isPatrolling = false;
        anim.SetFloat("moveSpeed", 0f);

        if (!isAttacking && currentCooldown <= 0)
        {
            AttackPlayer();
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    private void Flip()
    {
        patrolSpeed *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
        currentCooldown = attackCooldown;
    }

    public void AttackPlayer()
    {
        anim.SetTrigger("AttackPlayer");
        isAttacking = true;
        StartCoroutine(AttackCooldown());
    }

    public void DontAttackPlayer()
    {
        isPatrolling = true;
        isAttacking = false;
    }

    public void DamagePlayer()
    {
        Collider2D[] hittarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D target in hittarget)
        {
            target.GetComponent<Health>().TakeDamage(damageAmount);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        // Draw lines between patrol points
        Gizmos.DrawLine(leftPoint.position, rightPoint.position);
    }
}