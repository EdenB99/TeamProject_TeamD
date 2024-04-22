using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponBase : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    Animator animator;
    WeaponAction inputActions;

    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 무기의 세로 길이
    /// </summary>
    private float weaponLength;

    WeaponData weaponData;

    protected Player player;

    protected PlayerStats playerStats;

    /// <summary>
    /// 공격 이펙트 오브젝트
    /// </summary>
    public GameObject weaponEffectPrefab;

    Transform hinge;

    Transform effectPosition;

    /// <summary>
    /// 무기 공격력
    /// </summary>
   public  int weaponDamage = 10;

    public float critical = 10.0f;


    public float rotationSpeed = 5.0f;

    /// <summary>
    /// 공격할 때 데미지의 총합
    /// </summary>
    public float totalDamage => weaponDamage + playerStats.attackPower;

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

    

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        inputActions = new WeaponAction();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        player = GameManager.Instance.Player;

        playerStats = player.PlayerStats;

        hinge = player.transform.GetChild(0);

        weaponData = new WeaponData();

        SetAnimationState();

        weaponLength = spriteRenderer.sprite.bounds.size.y * transform.localScale.y;
    }



    protected void Update()
    {
        UpdateWeaponPosition();

        // 공격 입력 처리
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    protected virtual void UpdateWeaponPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        transform.position = hinge.position;

        Vector3 direction = mousePosition - transform.position;
        direction.z = 0;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = rotation;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);

        if (hit.collider != null)
        {
            // 충돌 지점이 무기의 세로 길이보다 멀리 있으면, 이펙트는 무기의 최대 세로 길이에서 생성
            Vector2 effectPosition = transform.position + (direction.normalized * weaponLength);
            Debug.Log($"{effectPosition}");
        }
        else
        {
            // 충돌이 없거나 충돌 지점이 무기 세로 길이보다 가까울 경우, 무기 끝에서 이펙트를 생성
            Vector2 effectPosition = transform.position + (direction.normalized * weaponLength);
            Debug.Log($"{effectPosition}");
        }
    }

    void SetAnimationState()
    {
        switch (weaponData.weaponType)
        {
            case WeaponType.Slash:
                animator.SetTrigger("SlashAttack");
                Debug.Log("슬래시 어택 트리거");
                break;

            case WeaponType.Stab:
                animator.SetTrigger("StabAttack");
                break;
            default:
                break;
        }
        Debug.Log($"{weaponData.weaponType}");
    }

    // 추가된 함수: 공격 입력을 받아 애니메이션을 재생
    protected virtual void Attack()
    {
        SetAnimationState();
        animator.SetTrigger(attackTrigger);
        Debug.Log("공격트리거 발동");
        


        ActivateEffect(transform.position);
        Debug.Log($"{transform.position}");

        //// 공격 속도에 따라 애니메이션 속도 조절
        //float attackAnimationSpeed = playerStats.attackSpeed;
        //animator.SetFloat("weaponSpeed", attackAnimationSpeed);

    }


    /// <summary>
    /// 이펙트 활성화 함수
    /// </summary>
    protected virtual void ActivateEffect(Vector2 effectPosition)
    {
        GameObject weaponEffectInstance = Instantiate(weaponEffectPrefab, effectPosition, Quaternion.identity);
    }
}