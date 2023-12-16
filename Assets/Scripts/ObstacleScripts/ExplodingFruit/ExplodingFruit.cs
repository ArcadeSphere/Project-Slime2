using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingFruit : PlayerDetector
{
    [SerializeField] private Rigidbody2D rb;
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
        Instantiate(fruitimpact, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
