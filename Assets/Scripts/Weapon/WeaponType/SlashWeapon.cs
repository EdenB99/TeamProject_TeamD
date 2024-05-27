using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SlashWeapon : WeaponBase
{
    public GameObject SkillPrefab;
    GameObject skillInstance;

    public float skillCoolTime = 5.0f;

    public float skillCurrentCoolTime = 0.0f;

    public bool canActivate = true;

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

    protected override void OnEnable()
    {
        if(IsPlayerAlive() && canActivate == true)
        {
            base.OnEnable();
            weaponInputActions.Weapon.SKill.performed += OnSKill;
        }
    }



    protected override void OnDisable()
    {
        if(IsPlayerAlive() && canActivate == true)
        {
            weaponInputActions.Weapon.SKill.performed -= OnSKill;
            base.OnDisable();
        }
    }
    private void OnSKill(InputAction.CallbackContext context)
    {
        if (canActivate)
        {
            SKill();
            skillCurrentCoolTime = 0.0f;
            canActivate = false;
        }
    }

    protected override void Update()
    {
        base.Update();
        if (skillCurrentCoolTime < skillCoolTime)
        {
            skillCurrentCoolTime += Time.deltaTime;
        }
        else
        {
            canActivate = true;
        }
    }

    public void SKill()
    {
        if (skillInstance == null) // 스킬 인스턴스가 존재하지 않는 경우에만 생성
        {
            skillInstance = Instantiate(SkillPrefab, player.transform.position, Quaternion.identity);
            skillInstance.transform.SetParent(player.transform);
            skillInstance.transform.localPosition = new Vector2(0, 0.5f);
        }
        else
        {
            Debug.Log("스킬이 이미 활성화되어 있습니다.");
        }

    }
}
