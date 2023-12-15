using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : TakingDamage
{
    public float destroyDelay = 2f;
    public Animator hitAnimator;

    private Vector2 direction;
    private float speed;

    private void Start()
    {
        // Destroy the projectile after a delay if it doesn't hit anything
        Destroy(gameObject, destroyDelay);
    }

    private void Update()
    {
        // Move the projectile in its set direction
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    public void SetSpeed(float projectileSpeed)
    {
        speed = projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.CompareTag("Player")) 
        {
            // Play hit animation
            if (hitAnimator != null)
            {
                hitAnimator.SetTrigger("explode");
            }
            Destroy(gameObject, 0.2f);
        }
    }
}

