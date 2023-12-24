using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private GameObject[] patrolPoints;
    public float moveSpeed = 3f;
    [SerializeField] private float turnBackDelay; 
    [SerializeField] private bool flightless = true;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private PlayerDetector playerDetectorScript; // set if needed for specific animations
    [SerializeField] private bool isChasingEnemy;
    [HideInInspector] public bool onEdge = false; // used by PlayerDetector script to handle animations
    [HideInInspector] public bool patrol = true;
    private BoxCollider2D enemyCollider;
    private SpriteRenderer enemySprite;
    private Rigidbody2D enemyRb;
    private int currentPoint = 0;

    private void OnValidate() {
        if (flightless) 
        {
            groundLayer = 0;
        }
    }

    private void Start() {
        enemySprite = this.GetComponent<SpriteRenderer>();
        enemyCollider = this.GetComponent<BoxCollider2D>();
        enemyRb = this.GetComponent<Rigidbody2D>();
    }

    private void Update() {
        StartCoroutine(PatrolEdgeDelay());
        if (patrol)
        {
            if (!onEdge)
            {
                if (flightless)
                    GroundEnemyPatrol();
                else
                    FlyEnemyPatrol();
            }
        }
        if (isChasingEnemy)
            HandleFlipOnChase();
    }

    private void GroundEnemyPatrol()
    {
        StartCoroutine(UpdatePatrolPoints(new Vector2(patrolPoints[currentPoint].transform.position.x, transform.position.y), transform.position));
        Vector3 targetPosition = new Vector3(patrolPoints[currentPoint].transform.position.x, transform.position.y, transform.position.z);
        // transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        enemyRb.velocity = (targetPosition - transform.position).normalized * moveSpeed;
    }

    private void FlyEnemyPatrol()
    {
        StartCoroutine(UpdatePatrolPoints(patrolPoints[currentPoint].transform.position, transform.position));
        // transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPoint].transform.position, moveSpeed * Time.deltaTime);
        Vector2 direction =  patrolPoints[currentPoint].transform.position - transform.position;
        enemyRb.velocity = direction.normalized * moveSpeed;
    }

    private IEnumerator UpdatePatrolPoints(Vector2 pointA, Vector2 pointB)
    {
        if (Vector2.Distance(pointA, pointB) < 0.1f)
        {
            onEdge = true;
            currentPoint++;
            if (currentPoint >= patrolPoints.Length)
            {
                currentPoint = 0;
            }
            // flip sprite if its the first and last point
            if (currentPoint <= 1 || currentPoint >= patrolPoints.Length)
            {
                yield return new WaitForSeconds(turnBackDelay);
                FlipEnemy();
            }
        }
    }

     void HandleFlipOnChase()
    {
        if (IsFacingRight())
        {
            enemySprite.flipX = true;
        }
        else
        {
            enemySprite.flipX = false;
        }
    }

    bool IsFacingRight()
    {
        if (enemyRb.velocity.x < 0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void FlipEnemy() 
    {
        if (playerDetectorScript != null)
        {
            playerDetectorScript.detectorOriginOffset *= -1;
        }
        enemySprite.flipX = !enemySprite.flipX;
    }

    private IEnumerator PatrolEdgeDelay()
    {
        if (onEdge)
        {
            yield return new WaitForSeconds(turnBackDelay);
            onEdge = false;
        }
    }

    // private bool IsOnGround()
    // {
    //     return Physics2D.BoxCast(enemyCollider.bounds.center, enemyCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
    // }

}
