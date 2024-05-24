using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlashWeapon : WeaponBase
{
    public GameObject SkillPrefab;

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
        if(IsPlayerAlive())
        {
            base.OnEnable();
            weaponInputActions.Weapon.SKill.performed += SKill;
        }
    }

    protected override void OnDisable()
    {
        if(IsPlayerAlive())
        {
            weaponInputActions.Weapon.SKill.performed -= SKill;
            base.OnDisable();
        }
    }

    public void SKill(InputAction.CallbackContext context)
    {
        GameObject skillInstance = Instantiate(SkillPrefab, player.transform.position, Quaternion.identity);

        skillInstance.transform.SetParent(player.transform);
        skillInstance.transform.localPosition = new Vector2(0, 0.5f);
        
    }
}
