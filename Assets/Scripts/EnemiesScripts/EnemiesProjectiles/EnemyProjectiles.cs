using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    public float destroyDelay = 2f;
    public GameObject explodeanimation;

    private float speed;
    private Vector2 direction;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    private void Update()
    {
      
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    public void SetSpeed(float projectileSpeed)
    {
        speed = projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(explodeanimation, transform.position, Quaternion.identity);
        Destroy(gameObject);

        if (other.CompareTag("Player"))
        {
            Instantiate(explodeanimation, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

   
}