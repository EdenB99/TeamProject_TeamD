using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : EnemyBase_
{

    protected override void attackAction()
    {

    }

    protected override void idleAction()
    {

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCheck = true;
        }
    }
}
