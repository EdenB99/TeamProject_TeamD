using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabbingWeapon : WeaponBase
{
    public float attackAnimationSpeed = 1.0f; // 공격 애니메이션 속도

    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        // weaponEffect 변수 초기화
        weaponEffectPrefab = GameObject.Find("WeaponEffect"); // 혹은 다른 방법으로 weaponEffect를 할당
        if (weaponEffectPrefab == null)
        {
            Debug.LogError("WeaponEffect is not assigned!");
        }
    }

    protected override void Start()
    {
        base.Start();
        player = GameManager.Instance.Player;
        UpdateWeaponPosition();
    }

    protected override void UpdateWeaponPosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.right = (mouseWorldPos - transform.position).normalized;
    }

    protected override void Attack()
    {
        animator.SetFloat("AttackSpeed", attackAnimationSpeed);
        animator.SetTrigger("Attack");
    }
}
