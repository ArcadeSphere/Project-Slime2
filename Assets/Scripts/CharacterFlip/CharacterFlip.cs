using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFlip : MonoBehaviour
{
    public bool isFacingRight = true;

    public bool IsFacingRight()
    {
        return isFacingRight;
    }

    public void FlipTheCharacter()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void FlipTowardsTarget(Vector3 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            if (isFacingRight)
            {
                FlipTheCharacter();
            }
        }
        else
        {
            if (!isFacingRight)
            {
                FlipTheCharacter();
            }
        }
    }
}