using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingFruit : PlayerDetector
{
    [SerializeField] private Rigidbody2D rb;
    public float fruitgravity;

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
        else
        {
            PlayerfarFruit();
        }
    }
    public void Playernearfruit()
    {
        Debug.Log("PLAYER NEAR");
        rb.gravityScale = fruitgravity;

    }
    public void PlayerfarFruit()
    {
        Debug.Log("so far away");
    }
 
}
