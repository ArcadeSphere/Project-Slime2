using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kupiscript : PlayerDetector
{

    public Transform firePoint;
    public GameObject projectilePrefab;
    private Animator anim;
    public float projectileSpeed = 5f;
 
   
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
    }

    public void OnPlayerIsIn()
    {

        anim.SetBool("IsShooting", true);

    }

    public void OnPlayerIsOut()
    {

        anim.SetBool("IsShooting", false);
    }


    public void ShootPlayer()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

       
        EnemyProjectiles projectileComponent = projectile.GetComponent<EnemyProjectiles>();
        if (projectileComponent != null)
        {
           
            Vector2 shootDirection = Vector2.left;
            projectileComponent.SetSpeed(projectileSpeed);
        }
    }
}