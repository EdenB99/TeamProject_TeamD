using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HeavyWeapon : WeaponBase
{
    protected override void Awake()
    {
        base.Awake();
        animator = transform.GetChild(0).GetComponent<Animator>();
    }

    protected override void Attack()
    {
        base.Attack();
        animator.SetTrigger("SlashAttack");
    }

    

}
