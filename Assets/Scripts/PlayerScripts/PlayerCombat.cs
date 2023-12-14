using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{

    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackrange;
    [SerializeField] private Transform attackpoint;
    [SerializeField] private float damageamount;
    public LayerMask enemieslayer;
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
        if (!pm.IsDashing() && Input.GetMouseButton(0) && cooldownTimer > attackCooldown)
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
    public void target_gets_damage()
    {
        
        
        Collider2D[] hittarget = Physics2D.OverlapCircleAll(attackpoint.position, attackrange, enemieslayer);

        foreach (Collider2D target in hittarget) {

            target.GetComponent<Health>().take_damage(damageamount);
        }
    }

     void OnDrawGizmosSelected()
    {
       
        Gizmos.DrawWireSphere(attackpoint.position, attackrange); 
    }
}
