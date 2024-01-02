using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [Header("Player Stats")]
    [SerializeField] private float playerSpeed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;
    private bool isJumping;
    private bool wasGrounded;
    private bool isAttacking;
    private bool isFacingRight = true;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip dashSound;

    [Header("Player Dependencies")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask jumpableLayer;
    [SerializeField] private Animator animator;
    public InputManager inputManager;

    [Header("Platform Disabler")]
    [SerializeField] private BoxCollider2D playerBoxCollider;
    [SerializeField] private float platformDisableDuration = 0.2f;
    private GameObject currentOneWayPlatform;
    private ParticleManager particle;


    private void Start() {
        particle = ParticleManager.Instance;    
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        // horizontal movement using InputManager
        horizontal = inputManager.GetHorizontalInput();

        // animations
        animator.SetFloat("run", Mathf.Abs(horizontal));
        animator.SetBool("grounded", IsGrounded());

        // jump using InputManager
        if (inputManager.GetJumpInput() && IsGrounded())
        {
            if (particle)
                particle.Play(particle.dustParticle);
            AudioManager.instance.PlaySoundEffects(jumpSound);
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if (inputManager.GetJumpInputUp() && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // dash using InputManager
        if (inputManager.GetDashInputDown() && canDash)
        {
            if (particle)
                particle.Play(particle.dashParticle);
            StartCoroutine(Dash());
        }

        // one-way platform using InputManager
        if (inputManager.GetPlatformDisableInputDown())
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisablePlatformCollider());
            }
        }

        if (Landed())
        {
            if (particle)
                particle.Play(particle.dustParticle);
        }

        Flip();
        wasGrounded = IsGrounded();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        // horizontal movement using InputManager
        rb.velocity = new Vector2(horizontal * playerSpeed, rb.velocity.y);
}
public bool IsJumping()
    {
        return !IsGrounded() && rb.velocity.y > 0f;
    }

    public bool IsDashing()
    {
        return isDashing;
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, jumpableLayer);
    }

    private bool Landed()
    {
        return !wasGrounded && IsGrounded();
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }


    }
   
    public void EnablePlayerMovement(bool enable)
    {
        if (!enable)
        {
            horizontal = 0f;
            rb.velocity = Vector2.zero;
        }
        isAttacking = !enable; 
        enabled = enable;
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        AudioManager.instance.PlaySoundEffects(dashSound);
        animator.SetTrigger("dash");
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        animator.SetTrigger("dashends");
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }
    private IEnumerator DisablePlatformCollider() 
    {
        BoxCollider2D currentPlatformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerBoxCollider, currentPlatformCollider);
        yield return new WaitForSeconds(platformDisableDuration);
        Physics2D.IgnoreCollision(playerBoxCollider, currentPlatformCollider, false);
    }

}

