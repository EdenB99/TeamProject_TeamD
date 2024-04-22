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
        animator.SetTrigger("StabAttack");
    }
}
