using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StabWeapon : WeaponBase
{
    public float attackAnimationSpeed = 1.0f; // 공격 애니메이션 속도

    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();

        //float angle = MathF.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.Euler(0f, 0f, angle);


        //animator.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        animator.SetTrigger("StabAttack");
    }
}
