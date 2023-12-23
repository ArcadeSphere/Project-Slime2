using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : PlayerDetector
{
    [Header("Butterfly Parameters")]
    [SerializeField] private EnemyPatrol enemyPatrol;


    private void Update() {
        if (PlayerDetected)
        {
            if (enemyPatrol != null)
            {
                enemyPatrol.patrol = false;
            }
            transform.Translate(DirectionToTarget.normalized * (enemyPatrol.moveSpeed * 1.3f) * Time.deltaTime);
        }
        else
        {
            enemyPatrol.patrol = true;
        }
    }


}
