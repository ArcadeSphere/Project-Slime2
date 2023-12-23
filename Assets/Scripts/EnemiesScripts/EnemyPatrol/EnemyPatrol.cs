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
    [HideInInspector] public bool onEdge = false; // used by PlayerDetector script to handle animations
    [HideInInspector] public bool patrol = true;
    private BoxCollider2D enemyCollider;
    private SpriteRenderer enemySprite;
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
    }

    private void GroundEnemyPatrol()
    {
        StartCoroutine(UpdatePatrolPoints(new Vector2(patrolPoints[currentPoint].transform.position.x, transform.position.y), transform.position));
        Vector2 targetPosition = new Vector2(patrolPoints[currentPoint].transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private void FlyEnemyPatrol()
    {
        StartCoroutine(UpdatePatrolPoints(patrolPoints[currentPoint].transform.position, transform.position));
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPoint].transform.position, moveSpeed * Time.deltaTime);
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
