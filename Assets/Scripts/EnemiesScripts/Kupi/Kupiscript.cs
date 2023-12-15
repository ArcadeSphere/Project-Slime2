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
        else
        {
            OnPlayerIsOut();
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



    public void shoot_at_player()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Access the Projectile script and set its direction and speed
        EnemyProjectiles projectileComponent = projectile.GetComponent<EnemyProjectiles>();
        if (projectileComponent != null)
        {
            // Set the shoot direction to always be towards the right
            Vector2 shootDirection = Vector2.right;

            projectileComponent.SetDirection(shootDirection);
            projectileComponent.SetSpeed(projectileSpeed);
        }
    }
}