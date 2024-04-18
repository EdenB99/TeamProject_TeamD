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
        
    }

    protected override void ActivateEffect(Vector2 effectPosition)
    {
        weaponEffectPrefab.transform.position = effectPosition;
        GameObject weaponEffect = Factory.Instance.MakeEffect(effectPosition, WeaponEffectType.StabEffect);
    }
}
