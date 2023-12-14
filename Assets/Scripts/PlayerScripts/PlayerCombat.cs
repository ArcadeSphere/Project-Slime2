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

    private void Awake()
    {
        anim = GetComponent<Animator>();
        
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && cooldownTimer > attackCooldown)
            player_attack_anim();

        cooldownTimer += Time.deltaTime;
    }
    private void player_attack_anim() {

        anim.SetTrigger("attack");
        cooldownTimer = 0;
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
