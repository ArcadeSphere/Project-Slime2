using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGoblin : MonoBehaviour
{
    [Header("Goblin Settings")]
    public Animator animGoblinMelee;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackPoint;
    private bool isAttacking = false;
    [SerializeField] private float attackCooldown = 2f;
    private float currentCooldown = 0f;
    public LayerMask playerLayer;
    [SerializeField] private float damageAmount;
    private bool isAttackAnimationInProgress = false;

    [Header("Patrol Settings")]
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float patrolSpeed = 2f;
    private bool isPatrolling = true;
    [SerializeField] private float stopDuration = 2f; 
    private Coroutine stopCoroutine;
    public bool IsPatrolling { get { return isPatrolling; } }
    private void Awake()
    {
        animGoblinMelee = GetComponent<Animator>();
    }



    public void MeleePatrol()
    {
        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
        animGoblinMelee.SetBool("isMoving", true);

        if ((patrolSpeed > 0 && transform.position.x >= rightPoint.position.x) ||
            (patrolSpeed < 0 && transform.position.x <= leftPoint.position.x))
        {
            FlipGoblin();
        }
    }

    public void StopPatrolAndAttack()
    {
        isPatrolling = false;
        animGoblinMelee.SetBool("isMoving", false);

        if (!isAttacking && currentCooldown <= 0)
        {
            AttackPlayer();
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    public void FlipGoblin()
    {
        patrolSpeed *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);

        // Enable patrolling after the attack cooldown is complete
        isPatrolling = true;
        isAttacking = false;
        currentCooldown = attackCooldown;
        isAttackAnimationInProgress = false;
    }
    private IEnumerator StopForDuration()
    {
        yield return new WaitForSeconds(stopDuration);
        isPatrolling = true;
        isAttacking = false;
        stopCoroutine = null;
    }
    public void AttackPlayer()
    {
        if (!isAttackAnimationInProgress)
        {
            animGoblinMelee.SetTrigger("AttackPlayer");
            isAttacking = true;
            isAttackAnimationInProgress = true;
            StartCoroutine(AttackCooldown());

            
            isPatrolling = false;

            if (stopCoroutine != null)
            {
                StopCoroutine(stopCoroutine);
            }
        }
    }

    public void DontAttackPlayer()
    {
        if (!isAttackAnimationInProgress)
        {
       
            isPatrolling = false;
            if (stopCoroutine == null)
            {
                stopCoroutine = StartCoroutine(StopForDuration());
            }
        }
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

        Gizmos.DrawLine(leftPoint.position, rightPoint.position);
    }
}