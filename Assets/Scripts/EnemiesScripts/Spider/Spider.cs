using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class Spider : PlayerDetector
{
    private Animator anim;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGround;
    private Rigidbody2D spiderRb;
    [SerializeField] private Transform playerTransform;
   [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float spiderJumpMoveSpeed = 5f; 
    [SerializeField] private float jumpCooldown = 2f;
    private float nextJumpTime;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spiderRb = GetComponent<Rigidbody2D>();
        nextJumpTime = Time.time;
    }

    private void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (PlayerDetected)
        {
            if (isGround && Time.time >= nextJumpTime)
            {
                JumpAttack();
                nextJumpTime = Time.time + jumpCooldown;
            }
        }
        else
        {
            StopJumpAttack();
        }
    }

    private void JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        spiderRb.velocity = new Vector2(spiderRb.velocity.x, 0);
        spiderRb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        spiderRb.AddForce(direction * spiderJumpMoveSpeed, ForceMode2D.Impulse);
    }

    private void StopJumpAttack()
    {
       
    }
}