using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kupiscript : PlayerDetection
{
 
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;
    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;

    

    private void Awake()
    {
        anim = GetComponent<Animator>();
     
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

    }
    override public void OnPlayerIn() {
        
       
    }
    override public void OnPlayerOut() { 
    
    }
    private void RangedAttack()
    {
        cooldownTimer = 0;
        fireballs[FindFireball()].transform.position = firepoint.position;
        fireballs[FindFireball()].GetComponent<EnemyProjectiles>().ActivateProjectile();
    }
    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

  
 
}
