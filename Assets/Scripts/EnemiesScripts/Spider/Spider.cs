using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class Spider : PlayerDetector
{
    [Header("Spider Settings")]
    private Animator anim;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGround;
    private Rigidbody2D spiderRb;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float damageAmount;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float horizontalMoveSpeed = 5f; 
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField] private float SpiderJumpDelay = 1.5f; 
    private float nextJumpTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spiderRb = GetComponent<Rigidbody2D>();
        nextJumpTime = Time.time + SpiderJumpDelay; 
    }

    private void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (PlayerDetected)
        {
            if (isGround && Time.time >= nextJumpTime)
            {
                FlipTowardsPlayer(); 
                JumpAttack();
                nextJumpTime = Time.time + jumpCooldown;
            }
        }
       
    }

    private void JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        spiderRb.velocity = new Vector2(spiderRb.velocity.x, 0);
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        spiderRb.AddForce(direction * horizontalMoveSpeed, ForceMode2D.Impulse);
        spiderRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        
    }



    private void FlipTowardsPlayer()
    {
        if (transform.position.x < playerTransform.position.x)
        {
         
            if (!IsFacingRight())
            {
                SpiderFlip();
            }
        }
        else
        {
          
            if (IsFacingRight())
            {
                SpiderFlip();
            }
        }
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void SpiderFlip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    public void SpiderDamagePlayer()
    {
        Collider2D[] hittarget = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D target in hittarget)
        {
            target.GetComponent<Health>().TakeDamage(damageAmount);
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }
}