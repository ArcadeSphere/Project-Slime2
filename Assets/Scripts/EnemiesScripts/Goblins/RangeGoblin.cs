using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeGoblin : MonoBehaviour
{
    [Header("Goblin Settings")]
    private Animator anim;
    [SerializeField] private Transform firePoint;
    private bool isShooting = false;
    [SerializeField] private float shootingCooldown = 2f;
    private bool isShootAnimationInProgress = false;
    private float currentCooldown = 0f;

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
            FlipGoblin();
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
}
