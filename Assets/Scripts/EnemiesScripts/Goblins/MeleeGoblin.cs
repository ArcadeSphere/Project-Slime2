using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeGoblin :PlayerDetector
{
    private Animator anim;
    // private Health healthScript;
    private bool isAttacking = false;
    public float attackCooldown = 2f; 
    private float currentCooldown = 0f;
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
            isAttacking = false;
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
   
  
}
