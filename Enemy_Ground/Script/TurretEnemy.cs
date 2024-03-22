using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : EnemyBase_
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        hp = maxHp;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        if (playerCheck) 
        {
            targetPos = player.transform.position;
            if (!IsMove)
            {
                if (targetPos.x < rb.position.x) CheckLR = 1;
                else CheckLR = -1;
            }
            attackAction();
        }
        else 
        {
            idleAction();
        }
    }

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
