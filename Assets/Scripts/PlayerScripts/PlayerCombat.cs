using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float damageAmount;
    public LayerMask enemiesLayer;
    private Animator anim;
    private float cooldownTimer = Mathf.Infinity;
    private PlayerMovement pm;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();

    }

    private void Update()
    {
        if (!pm.IsDashing() && (Input.GetMouseButton(0) || Input.GetKey(KeyCode.J)) && cooldownTimer > attackCooldown)
            StartCoroutine(PerformAttack());


        cooldownTimer += Time.deltaTime;
    }
    private IEnumerator PerformAttack()
    {
        pm.EnablePlayerMovement(false);

        
        anim.SetTrigger("attack");
        cooldownTimer = 0;

       
        yield return new WaitForSeconds(0.3f);

      
        pm.EnablePlayerMovement(true);
    }
    public void DamageTarget()
    {
        
        
        Collider2D[] hittarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayer);

        foreach (Collider2D target in hittarget) {

            target.GetComponent<Health>().TakeDamage(damageAmount);
        }
    }

     void OnDrawGizmosSelected()
    {
       
        Gizmos.DrawWireSphere(attackPoint.position, attackRange); 
    }
}
