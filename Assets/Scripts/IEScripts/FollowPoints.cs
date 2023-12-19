using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPoints : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private float moveSpeed = 3f;
    private int currentWayPointIndex = 0;

    private void Update() 
    {
        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint() 
    {
        if (Vector2.Distance(waypoints[currentWayPointIndex].transform.position, transform.position) < 0.3f)
        {
            currentWayPointIndex++;
            if (currentWayPointIndex >= waypoints.Length) 
            {
                currentWayPointIndex = 0;
            }
        }    
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWayPointIndex].transform.position, moveSpeed * Time.deltaTime);
    }

}
