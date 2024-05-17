using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashWeapon : WeaponBase
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
