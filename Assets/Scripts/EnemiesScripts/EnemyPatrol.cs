using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyPatrol : MonoBehaviour
{
    [SerializeField] private GameObject[] patrolPoints;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool flightless = true;
    [SerializeField] private PlayerDetector playerDetectorScript; 
    private BoxCollider2D enemyCollider;
    private SpriteRenderer enemySprite;
    private int currentPoint = 0;

    private void Start() {
        enemySprite = this.GetComponent<SpriteRenderer>();
        enemyCollider = this.GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if (flightless)
            GroundEnemyPatrol();
        else
            FlyEnemyPatrol();
    }

    void GroundEnemyPatrol()
    {
        if (Vector2.Distance(new Vector2(patrolPoints[currentPoint].transform.position.x, transform.position.y), transform.position) < 0.1f)
        {
            FlipEnemy();
            currentPoint++;
            if (currentPoint >= patrolPoints.Length)
            {
                currentPoint = 0;
            }
        }
        Vector2 targetPosition = new Vector2(patrolPoints[currentPoint].transform.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    void FlyEnemyPatrol()
    {
        if (Vector2.Distance(patrolPoints[currentPoint].transform.position, transform.position) < 1f)
        {
            FlipEnemy();
            currentPoint++;
            if (currentPoint >= patrolPoints.Length)
            {
                currentPoint = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPoint].transform.position, moveSpeed * Time.deltaTime);
    }

    void FlipEnemy() 
    {
        if (playerDetectorScript != null)
        {
            playerDetectorScript.detectorOriginOffset *= -1;
        }
        enemySprite.flipX = !enemySprite.flipX;
        
    }

    bool IsOnGround()
    {
        return Physics2D.BoxCast(enemyCollider.bounds.center, enemyCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
    }

}
