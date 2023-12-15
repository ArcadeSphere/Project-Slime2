using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    public float destroyDelay = 2f;
    public Animator hitAnimator;

    private float speed;

    private void Start()
    {
        // Destroy the projectile after a delay if it doesn't hit anything
        Destroy(gameObject, destroyDelay);
    }

    private void Update()
    {
        // Move the projectile to the right
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
  

    public void SetSpeed(float projectileSpeed)
    {
        speed = projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.CompareTag("Player")) 
        {
            
            if (hitAnimator != null)
            {
                hitAnimator.SetTrigger("explode");
            }

        
        }
    }

    public void destroybullet()
    {
        Destroy(this.gameObject);
    }
}

