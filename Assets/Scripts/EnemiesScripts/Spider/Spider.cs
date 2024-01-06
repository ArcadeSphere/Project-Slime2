using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class Spider : PlayerDetector
{
    [Header("Spider Settings")]
    private Animator anim;
    private Rigidbody2D spiderRb;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private Transform playerTransform;

    [Header("Spider Jump Settings")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField] private float SpiderJumpDelay = 1.5f;
    private float nextJumpTime;
    [SerializeField] private float damageAmount;
    private bool isGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;


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
                StartCoroutine(DelayedSpiderJump());
                nextJumpTime = Time.time + jumpCooldown;
            }
        }
    }

    //adds a delay for the first jump
    private IEnumerator DelayedSpiderJump()
    {
        yield return new WaitForSeconds(SpiderJumpDelay);

        JumpAttack();
    }

    //jump attack
    private void JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        AudioManager.instance.PlaySoundEffects(attackSound);
        float distanceFromPlayer = playerTransform.position.x - transform.position.x;
        spiderRb.AddForce(new Vector2(distanceFromPlayer, jumpForce), ForceMode2D.Impulse);
    }


    private void FlipTowardsPlayer()
    {
        if (transform.position.x < playerTransform.position.x)
        {
         
            if (IsFacingRight())
            {
                SpiderFlip();
            }
        }
        else
        {
          
            if (!IsFacingRight())
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            anim.SetTrigger("dying");
        }
       
    }
}