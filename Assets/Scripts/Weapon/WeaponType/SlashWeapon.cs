using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashWeapon : WeaponBase
{
    public float attackAnimationSpeed = 1.0f; // 공격 애니메이션 속도

    private Animator animator;

    private float lastAttackTime = 0f; // 마지막 공격 시간

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
     
       animator.SetTrigger("SlashAttack");

       if (lastAttackTime < attackCooldown)
       {
           animator.SetTrigger("AttackUp");
            Debug.Log("어퍼 트리거 발동");
           ActivateEffect(transform.position);
       }
    }                  
}
