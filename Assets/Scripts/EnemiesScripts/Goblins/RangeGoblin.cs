using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeGoblin : MonoBehaviour
{
    [Header("Goblin Settings")]
    private Animator anim;
    private bool isShooting = false;
    [SerializeField] private float shootingCooldown = 2f;
    private bool isShootAnimationInProgress = false;
    private float currentCooldown = 0f;

    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    [SerializeField] private Transform firePoint;
    public float arrowSpeed = 5f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private float patrolSpeed = 2f;
    private bool isPatrolling = true;
    [SerializeField] private float stopDuration = 2f;
    private Coroutine stopCoroutine;
    public bool IsRangePatrolling { get { return isPatrolling; } }
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void RangePatrol()
    {
        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
        anim.SetFloat("moveSpeed", Mathf.Abs(patrolSpeed));

        if ((patrolSpeed > 0 && transform.position.x >= rightPoint.position.x) ||
            (patrolSpeed < 0 && transform.position.x <= leftPoint.position.x))
        {
            StopRangePatrolForDuration();
        }
    }
    public void StopPatrolAndRangeAttack()
    {
        isPatrolling = false;
        anim.SetFloat("moveSpeed", 0f);

        if (!isShooting && currentCooldown <= 0)
        {
            ShootPlayer();
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    private void FlipGoblin()
    {
        patrolSpeed *= -1;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    private IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(shootingCooldown);
        isShooting = false;
        currentCooldown = shootingCooldown;
        isShootAnimationInProgress = false;
    }

    private IEnumerator StopForDuration()
    {
        yield return new WaitForSeconds(stopDuration);
        isPatrolling = true;
        isShooting = false;
        stopCoroutine = null;
    }
    private void StopRangePatrolForDuration()
    {
        isPatrolling = false;
        anim.SetFloat("moveSpeed", 0f);

        if (stopCoroutine == null)
        {
            stopCoroutine = StartCoroutine(StopRangeForDuration());
        }
    }

    private IEnumerator StopRangeForDuration()
    {
        yield return new WaitForSeconds(stopDuration / 2f); // Wait for half of the stopDuration
        FlipGoblin(); // Flip the goblin after the delay

        yield return new WaitForSeconds(stopDuration / 2f); // Wait for the remaining half of the stopDuration
        isPatrolling = true;
        stopCoroutine = null;
    }
    public void ShootPlayer()
    {
        if (!isShootAnimationInProgress)
        {
            anim.SetTrigger("Shooting");
            isShooting = true;
            isShootAnimationInProgress = true;
            StartCoroutine(ShootingCooldown());


            isPatrolling = false;


            if (stopCoroutine != null)
            {
                StopCoroutine(stopCoroutine);
            }
        }
    }

    public void DontShootPlayer()
    {
        if (!isShootAnimationInProgress)
        {

            isPatrolling = false;
            if (stopCoroutine == null)
            {
                stopCoroutine = StartCoroutine(StopForDuration());
            }
        }
    }
    void OnDrawGizmosSelected()
    {
    
        Gizmos.DrawLine(leftPoint.position, rightPoint.position);
    }
    public void ShootArrows()
    {
        GameObject projectile = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);

       ArrowProjectiles projectileComponent = projectile.GetComponent<ArrowProjectiles>();
        if (projectileComponent != null)
        {
            Vector2 shootDirection = (transform.localScale.x > 0) ? Vector2.right : Vector2.left;
            projectileComponent.SetSpeed(arrowSpeed);
            projectileComponent.SetProjectileDirection(shootDirection);
        }
    }
}
