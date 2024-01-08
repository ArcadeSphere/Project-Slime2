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

    [Header("Reference Settings")]
    [SerializeField] private DetectionIndicator detectionIndicator;
    [SerializeField] private CharacterFlip characterFlip;

    [Header("Goblin ChaseAttack Settings")]
    [SerializeField] private float chaseSpeed = 5f;
    [SerializeField] private float stopDistance = 1.5f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float attackCooldown = 2f;
    private bool isCooldown = false;

    [Header("Goblin Patrol Settings")]
    public Transform patrolPoint1;
    public Transform patrolPoint2;
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float patrolStopDuration = 2f;
    private bool isTurning = false;


    [Header("Edge Detection Settings")]
    public Transform edgeDetector;
    public float edgeDetectionDistance = 0.2f;

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
            detectionIndicator.DeactivateAlert();
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
        if (characterFlip.isFacingRight && !isTurning)
        {
            transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
            if (transform.position.x > patrolPoint2.position.x)
            {
                isTurning = true;
                StartCoroutine(TurnDelay());
            }
        }
        else if (!characterFlip.isFacingRight && !isTurning)
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
            detectionIndicator.ActivateAlert();
        }
        else
        {
            anim.SetFloat("moveSpeed", 1f);
            detectionIndicator.DeactivateAlert();
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
        characterFlip.FlipTheCharacter();
    }

    private void DetectionDelay()
    {
        detectionDelayTimer -= Time.deltaTime;

        if (detectionDelayTimer <= 0f)
        {
            currentState = GobbyAxeState.Chase;
            detectionIndicator.ActivateAlert();
            detectionDelayTimer = 0f; // Reset the timer
        }
    }
    private void ChasePlayer()
    {
        Vector2 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.Normalize();

        characterFlip.FlipTowardsTarget(playerTransform.position);

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        Debug.Log("Distance to Player: " + distanceToPlayer);
        Debug.Log("Is Player in Detection Zone: " + IsPlayerInChaseDetectionZone());

        // Check if there's an edge or hole in front
        if (IsObjectInFront("Obstacle") || IsObjectInFront("Hole"))
        {
            Debug.Log("Edge or Hole in Front! Stopping Chase.");
            currentState = GobbyAxeState.Patrol;
            anim.SetFloat("moveSpeed", 0f);
            detectionIndicator.DeactivateAlert();
            return;
        }

        // If the player is still in the chase detection zone, continue chasing
        if (IsPlayerInChaseDetectionZone())
        {
            detectionIndicator.ActivateAlert();
            if (distanceToPlayer > stopDistance)
            {
                MoveTowardsPlayer(directionToPlayer);
                anim.SetFloat("moveSpeed", 1f);
            }
            else if (distanceToPlayer <= stopDistance && !IsPlayerInAttackRange(distanceToPlayer))
            {
                // Player is close but not in attack range, continue chasing
                anim.SetFloat("moveSpeed", 1f);
            }
            else
            {
                // Player is in attack range, stop and attack
                StopAndAttack();
                return; // Return to prevent additional movement after stopping to attack
            }
        }
        else
        {
            // If the player is not in the detection zone, go back to Patrol state
            currentState = GobbyAxeState.Patrol;
            anim.SetFloat("moveSpeed", 0f);
            detectionIndicator.DeactivateAlert();
        }
    }


    private bool IsObjectInFront(string tag)
    {
        Vector2 rayDirection = characterFlip.isFacingRight ? Vector2.right : Vector2.left;
        RaycastHit2D[] hits = Physics2D.RaycastAll(edgeDetector.position, rayDirection, edgeDetectionDistance, LayerMask.GetMask(tag));

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
                Debug.Log("Hit distance: " + hit.distance);

                // Check if the detected object has the specified tag
                if (hit.collider.CompareTag(tag))
                {
                    Debug.Log(tag + " in Front! Stopping Chase.");
                    return true;
                }
            }
        }

        return false;
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
    private bool IsPlayerInChaseDetectionZone()
    {
        Vector2 offset = characterFlip.isFacingRight ? chaseDetectorOriginOffset : new Vector2(-chaseDetectorOriginOffset.x, chaseDetectorOriginOffset.y);
        Vector2 detectionZonePosition = (Vector2)chaseDetectionZoneOrigin.position + offset;

        Collider2D collider = Physics2D.OverlapBox(detectionZonePosition, new Vector2(chaseDetectorSize.x, chaseDetectorSize.y), 0f, playerLayer);

        return collider != null && collider.CompareTag("Player"); // Ensure that the detected object is the player
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 offset = characterFlip.isFacingRight ? chaseDetectorOriginOffset : new Vector2(-chaseDetectorOriginOffset.x, chaseDetectorOriginOffset.y);
        Gizmos.DrawWireCube(chaseDetectionZoneOrigin.position + offset, new Vector3(chaseDetectorSize.x * (characterFlip.isFacingRight ? 1 : -1), chaseDetectorSize.y, 1f));

        Gizmos.color = Color.blue;

        // Draw gizmo for the edge detection
        Gizmos.color = Color.green;
        Vector3 edgeDirection = characterFlip.isFacingRight ? Vector2.right : Vector2.left;
        Gizmos.DrawRay(edgeDetector.position, edgeDirection * edgeDetectionDistance);


    }

}