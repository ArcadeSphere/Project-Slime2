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
    [SerializeField] private Transform playerTransform;

    [Header("Spider Jump Settings")]
    [SerializeField] private DetectionIndicator detectionIndicator;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField] private float SpiderJumpDelay = 1.5f;
    private float nextJumpTime;
    [SerializeField] private float damageAmount;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Reference Settings")]
    [SerializeField] private CharacterFlip characterFlip;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spiderRb = GetComponent<Rigidbody2D>();
        characterFlip = GetComponent<CharacterFlip>();
        nextJumpTime = Time.time + SpiderJumpDelay;
    }

    private void Update()
    {
        bool isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (PlayerDetected)
        {
            if (isGround && Time.time >= nextJumpTime)
            {
                characterFlip.FlipTowardsTarget(playerTransform.position);
                detectionIndicator.ActivateAlert();
                StartCoroutine(DelayedSpiderJump());
                nextJumpTime = Time.time + jumpCooldown;
            }
        }
        else
        {
            detectionIndicator.DeactivateAlert();
        }
    }

    // adds a delay for the first jump
    private IEnumerator DelayedSpiderJump()
    {
        yield return new WaitForSeconds(SpiderJumpDelay);
        JumpAttack();
    }

    // jump attack
    private void JumpAttack()
    {
        anim.SetTrigger("JumpAttack");
        AudioManager.instance.PlaySoundEffects(attackSound);
        float distanceFromPlayer = playerTransform.position.x - transform.position.x;
        spiderRb.AddForce(new Vector2(distanceFromPlayer, jumpForce), ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            anim.SetTrigger("dying");
        }
    }
}