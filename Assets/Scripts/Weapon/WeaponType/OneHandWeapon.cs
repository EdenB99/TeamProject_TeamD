using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneHandWeapon : WeaponBase
{
    public float attackAnimationSpeed = 1.0f; // 공격 애니메이션 속도

    private Animator animator;


    private float lastAttackTime = 0f; // 마지막 공격 시간

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        player = GameManager.Instance.Player;
        UpdateWeaponPosition();
    }

    protected virtual new void UpdateWeaponPosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.right = (mouseWorldPos - transform.position).normalized;
    }

    protected override void Attack()
    {
        // 공격 애니메이션 재생
        if (Time.time - lastAttackTime < attackCooldown)
        {
            // 짧은 간격 내에 연속 공격 발생시 Attack Up 애니메이션 재생
            animator.SetTrigger("AttackUp");
            Debug.Log("아래공격");
        }
        else
        {
            animator.SetTrigger("Attack");
            Debug.Log("공격");
        }
    }
}
