using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
public class GobbyAxe : MonoBehaviour
{
    [Header("Goblin Detection Settings")]
    private Animator anim;
    private Transform playerTransform;
    [SerializeField] private Vector2 chaseDetectorSize = Vector2.one;
    public Vector2 chaseDetectorOriginOffset = Vector2.zero;
    public float attackDetectionRange = 1.5f;
    public Transform chaseDetectionZoneOrigin;
    
    
    [Header("Goblin ChaseAttack Settings")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float damageAmount;
    public Transform attackDetectionZoneOrigin;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float delaySoundInSeconds = 2.0f;
    [SerializeField] private float attackCooldown = 2f;
    private bool isCooldown = false;


    [Header("Goblin Patrol Settings")]
    public Transform patrolPoint1;
    public Transform patrolPoint2;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float patrolStopDuration = 2f;
    [SerializeField] private float turnDelay = 1f;
    private bool isFacingRight = true;
    private bool isTurning = false;

    //Gobby different states
    private enum GobbyAxeState
    {
        Patrol,
        Chase,
        Attack,
        Cooldown
    }

    private GobbyAxeState currentState = GobbyAxeState.Patrol;
    
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

    //patrolling state
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

        if (isTurning)
        {
            anim.SetFloat("moveSpeed", 0f);
        }
        else if (IsPlayerInChaseDetectionZone())
        {
            currentState = GobbyAxeState.Chase;
            anim.SetFloat("moveSpeed", 1f);
        }
        else
        {
            anim.SetFloat("moveSpeed", 1f);
        }
    }
    private IEnumerator TurnDelay()
    {
        anim.SetFloat("moveSpeed", 1f);

        yield return new WaitForSeconds(patrolStopDuration);

        isTurning = false;
        Flip();
    }
    
    //chasing state
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
    //checks whether player is in attacking range
    private bool IsPlayerInAttackRange(float distanceToPlayer)
    {
        return distanceToPlayer < attackDetectionRange;
    }

    //attacking state
    private void Attack()
    {
        anim.SetTrigger("AttackPlayer");
        AudioManager.instance.PlaySoundEffects(attackSound);
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
    public void GobbyDamagePlayer()
    {
        Collider2D[] hittarget = Physics2D.OverlapCircleAll(attackDetectionZoneOrigin.position, attackDetectionRange, playerLayer);

        foreach (Collider2D target in hittarget)
        {
            target.GetComponent<Health>().TakeDamage(damageAmount);
        }
    }


    //detection zone to chase player
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(chaseDetectionZoneOrigin.position + (Vector3)chaseDetectorOriginOffset, new Vector3(chaseDetectorSize.x, chaseDetectorSize.y, 1f));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackDetectionZoneOrigin.position, attackDetectionRange);
    }
    private void PlaySoundWithDelay()
    {
        AudioManager.instance.PlaySoundEffects(attackSound);
    }
}