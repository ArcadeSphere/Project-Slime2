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
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (PlayerDetected)
        {
            if (!isAttacking && currentCooldown <= 0)
            {
                AttackPlayer();
            }
            else
            {
                currentCooldown -= Time.deltaTime;
            }
        }
        else
        {
            DontAttackPlayer();
        }
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
    }
}
