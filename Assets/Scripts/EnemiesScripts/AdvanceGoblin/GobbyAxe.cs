using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobbyAxe : MonoBehaviour
{
    private Animator anim;
    private Transform playerTransform;

    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private float turnDelay = 1f;
    [SerializeField] private Vector2 chaseDetectorSize = Vector2.one;
    public Vector2 chaseDetectorOriginOffset = Vector2.zero;
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
    private bool isTurning = false;

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
                ChasePlayer();
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
        Gizmos.DrawWireCube(chaseDetectionZoneOrigin.position + (Vector3)chaseDetectorOriginOffset, new Vector3(chaseDetectorSize.x, chaseDetectorSize.y, 1f));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackDetectionZoneOrigin.position, attackDetectionRange);
    }

    private void Patrol()
    {
        if (isFacingRight && !isTurning)
        {
            transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
            if (transform.position.x > patrolPoint2.position.x)
            {
                isTurning = true;
                StartCoroutine(TurnDelay());
           
            }
        }
        else if (!isFacingRight && !isTurning)
        {
            transform.Translate(Vector2.left * patrolSpeed * Time.deltaTime);
            if (transform.position.x < patrolPoint1.position.x)
            {
                isTurning = true;
                StartCoroutine(TurnDelay());
             
            }
        }

        if (IsPlayerInChaseDetectionZone())
        {
            currentState = GobbyAxeState.Chase;
            anim.SetFloat("moveSpeed", 1f);  // Set moveSpeed to chaseSpeed
        }
        else
        {
            anim.SetFloat("moveSpeed", 1f);  
        }
    }

    private IEnumerator TurnDelay()
    {
  
        yield return new WaitForSeconds(turnDelay);
        anim.SetFloat("moveSpeed", 0f);
        isTurning = false;
        Flip();
    }

    private void ChasePlayer()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.Normalize();

        FlipTowardsPlayer();

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer > stopDistance)
        {
            MoveTowardsPlayer(directionToPlayer);
            anim.SetFloat("moveSpeed", 1f);
        }
        else
        {
            StopAndAttack();
        }

        if (IsPlayerInAttackRange(distanceToPlayer))
        {
            StopAndAttack();
        }
        else if (!IsPlayerInChaseDetectionZone())
        {
            currentState = GobbyAxeState.Patrol;
            anim.SetFloat("moveSpeed", 0f);  
        }
    }
    private void MoveTowardsPlayer(Vector2 direction)
    {
        transform.Translate(direction * chaseSpeed * Time.deltaTime);
    }

    private void StopAndAttack()
    {
        anim.SetFloat("moveSpeed", 0f);
        transform.Translate(Vector2.zero);
        currentState = GobbyAxeState.Attack;
    }
    private bool IsPlayerInAttackRange(float distanceToPlayer)
    {
        return distanceToPlayer < attackDetectionRange;
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

        return distanceToPlayerX < chaseDetectorSize.x * 0.5f && distanceToPlayerY < chaseDetectorSize.y * 0.5f;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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
}