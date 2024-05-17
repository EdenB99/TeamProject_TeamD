using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class StabWeapon : WeaponBase
{
    protected override void Awake()
    {
        base.Awake();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    protected override void Attack()
    {
        base.Attack();
        animator.SetTrigger("StabAttack");
    }
}
