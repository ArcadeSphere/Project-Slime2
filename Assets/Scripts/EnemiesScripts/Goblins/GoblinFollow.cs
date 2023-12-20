using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinFollow : PlayerDetector
{
    public MeleeGoblin meleeGoblin;
    public float followSpeed = 3f;
    [SerializeField] private Transform playerTransform;
    public Transform checkGround;
    [SerializeField] private LayerMask Groundlayer;
    private void Update()
    {

        if (PlayerDetected && IsGoblinGrounded())
        {
            FollowPlayer();
        }
        else
        {
            StopFollowingPlayer();
        }
    }
    private bool IsGoblinGrounded()
    {
        
        Collider2D groundCollider = Physics2D.OverlapCircle(checkGround.position, 0.1f, Groundlayer);

        return groundCollider != null;
    }

    private void FollowPlayer()
    {
        
        Vector3 directionToPlayer = playerTransform.position - meleeGoblin.transform.position;

     
        if (directionToPlayer.x < 0)
        {
            meleeGoblin.FlipGoblin();
        }


        meleeGoblin.transform.Translate(directionToPlayer.normalized * followSpeed * Time.deltaTime);
        meleeGoblin.animGoblinMelee.SetBool("isMoving", true);

    }

    private void StopFollowingPlayer()
    {
        
        if (!meleeGoblin.IsPatrolling)
        {
            meleeGoblin.MeleePatrol();
            meleeGoblin.animGoblinMelee.SetBool("isMoving", false);
        }
    }
}
