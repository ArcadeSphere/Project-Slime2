using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingThing : MonoBehaviour
{
    private Rigidbody2D swtrb;
    [SerializeField] private float swtspeed;
    [SerializeField] private float LeftAngle;
    [SerializeField] private float RightAngle;
    bool ClockWiseMovement;
    void Start()
    {
        swtrb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {

        ChainMovement();
    }

    public void ChainDirection() { 
    
    if(transform.rotation.z > RightAngle)
        {
            ClockWiseMovement = false;
        }
        if (transform.rotation.z < LeftAngle)
        {
            ClockWiseMovement = true;
        }
    }

    public void ChainMovement()
    {
        ChainDirection();
        if (ClockWiseMovement)
        {
            swtrb.angularVelocity = swtspeed;
        }
        if (!ClockWiseMovement)
        {
            swtrb.angularVelocity = -1 * swtspeed;
        }
    }
}
