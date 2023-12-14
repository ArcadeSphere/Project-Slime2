using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;
    private bool isAttacking;
    private bool isFacingRight = true;
   


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Animator animator;
    


    void Update()
    {
        if (isDashing || isAttacking)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("run", Mathf.Abs(horizontal));
        animator.SetBool("grounded", IsGrounded());

    

        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {

 
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);


            }
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {

            StartCoroutine(Dash());
        }

        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    public bool IsDashing()
    {
        return isDashing;
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
        enabled = enable;
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        animator.SetTrigger("dash");
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        animator.SetTrigger("dashends");
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}

