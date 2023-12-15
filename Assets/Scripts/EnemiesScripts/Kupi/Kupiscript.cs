using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kupiscript : PlayerDetector
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
        if (PlayerDetected)
        {
            OnPlayerIsIn();
        }
        else
        {
            OnPlayerIsOut();
        }
        cooldownTimer += Time.deltaTime;
    }

public void OnPlayerIsIn()
    {
       

     

        
        if (cooldownTimer >= attackCooldown)
        {
            anim.SetBool("IsShooting", true);
        }
    }

    public void OnPlayerIsOut()
    {
       
        anim.SetBool("IsShooting", false);
    }

    private void RangedAttack()
    {
        cooldownTimer = 0;
        int fireballIndex = FindFireball();
        fireballs[fireballIndex].transform.position = firepoint.position;
        fireballs[fireballIndex].GetComponent<EnemyProjectiles>().ActivateProjectile();
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