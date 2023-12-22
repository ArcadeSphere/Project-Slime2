using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class ExplodingFruit : PlayerDetector
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] protected float enemyDamage;
    public float fruitgravity;
    public GameObject fruitimpact;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    
    private void Update()
    {
        if (PlayerDetected)
        {
            Playernearfruit();
        }
       
    }
    public void Playernearfruit()
    {
     
        rb.gravityScale = fruitgravity;

    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Health>().TakeDamage(enemyDamage);
            Instantiate(fruitimpact, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
        Instantiate(fruitimpact, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
