using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobbyAxe : MonoBehaviour
{
    private Animator anim;
    private Transform playerTransform;

    public float moveSpeedTowardsPlayer = 5f;
    public float stopDistance = 1.5f;
    public float chaseDetectionRangeX = 5f;
    public float chaseDetectionRangeY = 2f;
    public float attackDetectionRange = 1.5f;

    public Transform patrolPoint1;
    public Transform patrolPoint2;
    public Transform chaseDetectionZoneOrigin;
    public Transform attackDetectionZoneOrigin;

    private enum GobbyAxeState
    {
        Patrol,
        Chase,
        Attack,
        Cooldown
    }

    private GobbyAxeState currentState = GobbyAxeState.Patrol;
    private float attackCooldown = 2f;
    private bool isCooldown = false;
    private bool isFacingRight = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
     
    }

    private void Update()
    {
        switch (currentState)
        {
            case GobbyAxeState.Patrol:
                Patrol();
                break;

            case GobbyAxeState.Chase:
                Chase();
                break;

            case GobbyAxeState.Attack:
                Attack();
                break;

            case GobbyAxeState.Cooldown:
                Cooldown();
                break;
        }
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(chaseDetectionZoneOrigin.position, new Vector3(chaseDetectionRangeX * 2, chaseDetectionRangeY * 2, 1f));

     
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackDetectionZoneOrigin.position, attackDetectionRange);
    }

    private void Patrol()
    {
        
        if (isFacingRight)
        {
            transform.Translate(Vector2.right * moveSpeedTowardsPlayer * Time.deltaTime);
            if (transform.position.x > patrolPoint2.position.x)
            {
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeedTowardsPlayer * Time.deltaTime);
            if (transform.position.x < patrolPoint1.position.x)
            {
                Flip();
            }
        }

    
        if (IsPlayerInChaseDetectionZone())
        {
            currentState = GobbyAxeState.Chase;
        }
    }

    // Move towards the player
    private void Chase()
    {
      
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.Normalize();

       
        FlipTowardsPlayer();

   
        transform.Translate(directionToPlayer * moveSpeedTowardsPlayer * Time.deltaTime);

      
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

      
        if (distanceToPlayer > stopDistance)
        {
            // Continue chasing
        }
        else
        {
            // Stop moving when close to the player
            transform.Translate(Vector2.zero);
            currentState = GobbyAxeState.Attack;
            return;
        }

       
        if (IsPlayerInAttackRange(distanceToPlayer))
        {
            
            currentState = GobbyAxeState.Attack;
        }
        else if (!IsPlayerInChaseDetectionZone())
        {
      
            currentState = GobbyAxeState.Patrol;
        }
    }

    private bool IsPlayerInAttackRange(float distanceToPlayer)
    {
        
        return distanceToPlayer < attackDetectionRange;
    }
    private void FlipTowardsPlayer()
    {
        if (playerTransform.position.x < transform.position.x)
        {
            
            if (isFacingRight)
            {
                Flip();
            }
        }
        else
        {
          
            if (!isFacingRight)
            {
                Flip();
            }
        }
    }

    private void Attack()
    {
  
        anim.SetTrigger("AttackPlayer");

 
        currentState = GobbyAxeState.Cooldown;
        isCooldown = true;
        Invoke("EndCooldown", attackCooldown);
    }

    private void Cooldown()
    {
        if (!isCooldown)
        {
            currentState = GobbyAxeState.Patrol;
        }
    }

    private void EndCooldown()
    {
        isCooldown = false;
    }

    private bool IsPlayerInChaseDetectionZone()
    {
   
        float distanceToPlayerX = Mathf.Abs(transform.position.x - playerTransform.position.x);
        float distanceToPlayerY = Mathf.Abs(transform.position.y - playerTransform.position.y);

        return distanceToPlayerX < chaseDetectionRangeX && distanceToPlayerY < chaseDetectionRangeY;
    }


    private void Flip()
    {
       
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
