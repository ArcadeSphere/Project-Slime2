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
    [SerializeField] private float detectionDelayDuration = 1.0f;
    private float detectionDelayTimer;
    [SerializeField] private DetectionIndicator detectionIndicator;

    [Header("Goblin ChaseAttack Settings")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float damageAmount;
    public Transform attackDetectionZoneOrigin;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float attackCooldown = 2f;
    private bool isCooldown = false;

    [Header("Goblin Patrol Settings")]
    public Transform patrolPoint1;
    public Transform patrolPoint2;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float patrolStopDuration = 2f;
    private bool isFacingRight = true;
    private bool isTurning = false;

    // Gobby different states
    private enum GobbyAxeState
    {
        Patrol,
        DetectionDelay,
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

        if (detectionIndicator != null)
        {
            detectionIndicator.Deactivate();
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case GobbyAxeState.Patrol:
                Patrol();
                break;

            case GobbyAxeState.DetectionDelay:
                DetectionDelay();
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
            currentState = GobbyAxeState.DetectionDelay;
            anim.SetFloat("moveSpeed", 0f);
            detectionDelayTimer = detectionDelayDuration;
            detectionIndicator.Activate();
        }
        else
        {
            anim.SetFloat("moveSpeed", 1f);
            detectionIndicator.Deactivate();
        }

        if (!isTurning)
        {
            isTurning = false;
        }
    }

    private IEnumerator TurnDelay()
    {
        anim.SetFloat("moveSpeed", 1f);

        yield return new WaitForSeconds(patrolStopDuration);

        isTurning = false;
        Flip();
    }

    private void DetectionDelay()
    {
        detectionDelayTimer -= Time.deltaTime;

        if (detectionDelayTimer <= 0f)
        {
            currentState = GobbyAxeState.Chase;
            detectionIndicator.Activate();
        }
    }

    private void ChasePlayer()
    {
        ActivateDetectionIndicator(true);

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
            ActivateDetectionIndicator(false);
        }
    }

    private void ActivateDetectionIndicator(bool activate)
    {
        if (detectionIndicator != null)
        {
            detectionIndicator.Activate();
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
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(attackDetectionZoneOrigin.position, attackDetectionRange, playerLayer);

        foreach (Collider2D target in hitTargets)
        {
            target.GetComponent<Health>().TakeDamage(damageAmount);
        }
    }

    private bool IsPlayerInChaseDetectionZone()
    {
        Vector2 offset = isFacingRight ? chaseDetectorOriginOffset : new Vector2(-chaseDetectorOriginOffset.x, chaseDetectorOriginOffset.y);
        Vector2 detectionZonePosition = (Vector2)chaseDetectionZoneOrigin.position + offset;

        Collider2D collider = Physics2D.OverlapBox(detectionZonePosition, new Vector2(chaseDetectorSize.x, chaseDetectorSize.y), 0f, playerLayer);

        return collider != null;
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
        Vector3 offset = isFacingRight ? chaseDetectorOriginOffset : new Vector2(-chaseDetectorOriginOffset.x, chaseDetectorOriginOffset.y);
        Gizmos.DrawWireCube(chaseDetectionZoneOrigin.position + offset, new Vector3(chaseDetectorSize.x * (isFacingRight ? 1 : -1), chaseDetectorSize.y, 1f));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackDetectionZoneOrigin.position, attackDetectionRange);
    }

    private void PlaySoundWithDelay()
    {
        AudioManager.instance.PlaySoundEffects(attackSound);
    }
}