using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// 무기의 추가정보
/// </summary>
[System.Serializable]
public struct WeaponInfo
{
    public uint weaponDamage;
    public float attackSpeed;
    public GameObject modelPrefab;
    public WeaponType weaponType;
}

public class WeaponBase : WeaponManager
{
    new Rigidbody2D rigidbody;
    Animator animator;
    WeaponAction weaponInputActions;

    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 무기의 세로 길이
    /// </summary>
    private float weaponLength;

    protected Player player;

    protected PlayerStats playerStats;

    /// <summary>
    /// 공격 이펙트 오브젝트
    /// </summary>
    public GameObject weaponEffectPrefab;

    /// <summary>
    /// 이펙트 생성좌표
    /// </summary>
    Vector2 effectPosition;

    protected Vector3 direction;

    /// <summary>
    /// 플레이어의 힌지
    /// </summary>
    protected Transform hinge;

    /// <summary>
    /// 무기 공격력
    /// </summary>
   public  int weaponDamage = 10;

    public float critical = 10.0f;

    public float rotationSpeed = 5.0f;

    /// <summary>
    /// 공격할 때 데미지의 총합
    /// </summary>
    public float totalDamage => weaponDamage + playerStats.AttackPower;

    /// <summary>
    /// 무기의 공격 속도
    /// </summary>
    public float weaponSpeed = 10.0f;

    public float attackCooldown = 0.5f; // 공격 간격

    private float lastAttackTime; // 마지막 공격 시간

    /// <summary>
    /// 공격 애니메이션을 제어하기 위한 트리거
    /// </summary>
    private const string attackTrigger = "Attack";

    protected override void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        weaponInputActions = new WeaponAction();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (!IsPlayerAlive())
        {
            DeactivateWeapon();
        }
    }

    protected override void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        player = GameManager.Instance.Player;

        playerStats = player.PlayerStats;

        hinge = player.transform.GetChild(0);

        weaponLength = spriteRenderer.sprite.bounds.size.y * transform.localScale.y;
    }

    protected override void OnEnable()
    {
        if (IsPlayerAlive())
        {
            weaponInputActions.Weapon.Enable();
            weaponInputActions.Weapon.Attack.performed += OnAttack;
        }
    }

    protected override void OnDisable()
    {
        if (IsPlayerAlive())
        {
            weaponInputActions.Weapon.Attack.performed -= OnAttack;
            weaponInputActions.Weapon.Disable();
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (IsPlayerAlive())
        {
            Attack();
        }
    }

    protected void Update()
    {
        if (IsPlayerAlive())
        {
            UpdateWeaponPosition();
        }
    }

    protected virtual void UpdateWeaponPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        transform.position = hinge.position;

        direction = mousePosition - transform.position;
        direction.z = 0;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = rotation;

        // 마우스 위치에 따라 무기의 방향(좌우 반전) 결정
        if (mousePosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        effectPosition = transform.position + (direction.normalized * weaponLength);
    }

    private void OnDestroy()
    {
        weaponInputActions.Weapon.Disable();
    }

    // 추가된 함수: 공격 입력을 받아 애니메이션을 재생
    protected virtual void Attack()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 마우스 위치에 따라 애니메이션의 방향(좌우 반전) 결정
        if (mousePosition.x < transform.position.x)
        {
            // 애니메이션이 왼쪽을 향하도록 좌우 반전
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // 애니메이션이 오른쪽을 향하도록 원래대로 복구
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        
        animator.SetTrigger(attackTrigger);

        ActivateEffect(transform.position);

        //// 공격 속도에 따라 애니메이션 속도 조절
        //float attackAnimationSpeed = playerStats.attackSpeed;
        //animator.SetFloat("weaponSpeed", attackAnimationSpeed);
    }

    /// <summary>
    /// 이펙트 활성화 함수
    /// </summary>
    protected virtual void ActivateEffect(Vector2 effectPosition)
    {
        float angle = MathF.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject weaponEffectInstance = Instantiate(weaponEffectPrefab, this.effectPosition, rotation);
    }

    /// <summary>
    /// 플레이어가 죽었을 경우 판별
    /// </summary>
    /// <returns></returns>
    protected bool IsPlayerAlive()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        return playerStats != null && playerStats.IsAlive;
    }


    /// <summary>
    /// 무기의 비활성화 함수
    /// </summary>
    protected virtual void DeactivateWeapon()
    {
        weaponInputActions.Weapon.Disable();
    }

}   