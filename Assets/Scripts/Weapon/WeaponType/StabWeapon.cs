using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabWeapon : WeaponBase
{
    public float attackAnimationSpeed = 1.0f; // 공격 애니메이션 속도

    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Attack()
    {
        base.Attack();
        animator.SetFloat("AttackSpeed", attackAnimationSpeed);
        animator.SetTrigger("Attack");
    }
    protected override void ActivateEffect(Vector2 effectPosition)
    {
        weaponEffectPrefab.transform.position = effectPosition;
        GameObject weaponEffectInstance = Instantiate(weaponEffectPrefab, effectPosition, Quaternion.identity);
    }
}
