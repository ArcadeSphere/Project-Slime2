using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : PlayerDetector
{
    private Animator anim;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private bool isGround;
    private Rigidbody2D spiderRb;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spiderRb = GetComponent<Rigidbody2D>();

    }
    private void Update()
    {
        if (PlayerDetected)
        {

        }
        else
        {

        }
    }
    private void JumpAttack()
    {

    }
    private void StopJumpAttack()
    {

    }
}
