using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFlip : MonoBehaviour
{

    //use this script for flipping enemies or other objects. make sure to put your character as -1 in the tranform for the code to work correctly
    public bool isFacingRight = true;

    public bool IsFacingRight()
    {
        return isFacingRight;
    }

    //Flip the character
    public void FlipTheCharacter()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    //Flip the character according to other objects tranform
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