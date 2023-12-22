using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakingDamage : MonoBehaviour
{
    [SerializeField] protected float damage;
    [SerializeField] private PlayerDetector playerDetector;


    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            CameraShake.Instance.ShakeCamera(2f, 0.2f);
        }
    }

    protected void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.CompareTag("Player") && playerDetector.PlayerDetected)
        {
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            CameraShake.Instance.ShakeCamera(2f, 0.2f);
        }
    }
}

