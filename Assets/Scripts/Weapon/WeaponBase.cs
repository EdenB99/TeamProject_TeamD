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

    WeaponData weaponData;

    protected Player player;

    protected PlayerStats playerStats;

    /// <summary>
    /// 공격 이펙트 오브젝트
    /// </summary>
    public GameObject weaponEffectPrefab;


    /// <summary>
    /// 무기 이펙트의 생성위치(자식오브젝트로 직접적인 좌표 설정)
    /// </summary>
    private Transform effectPosition;

    Transform hinge;

    /// <summary>
    /// 무기 공격력
    /// </summary>
    public int weaponDamage = 10;

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
    /// 무기의 초기 위치
    /// </summary>
    protected Vector2 originalPosition;

    /// <summary>
    /// 마우스의 현재 위치
    /// </summary>
    protected Vector2 mousePos;

    /// <summary>
    /// 공격 애니메이션을 제어하기 위한 트리거
    /// </summary>
    private const string attackTrigger = "Attack";


    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        inputActions = new WeaponAction();
    }

    protected virtual void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        originalPosition = transform.localPosition;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        player = GameManager.Instance.Player;

        playerStats = player.PlayerStats;

        hinge = player.transform.GetChild(0);

        weaponData = new WeaponData();

        SetAnimationState();

        weaponEffectPrefab = Instantiate(weaponEffectPrefab, transform.position, Quaternion.identity);

        effectPosition = transform.GetChild(1);

        // 팩토리에서 가져오게끔 수정
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

    /// <summary>
    /// 무기 오브젝트가 마우스의 좌표를 따라가게 하는 함수
    /// </summary>
    protected virtual void UpdateWeaponPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        transform.position = hinge.position;

        Vector3 direction = mousePosition - transform.position;
        direction.z = 0;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = rotation;
    }

    /// <summary>
    /// 무기타입에 따른 애니메이션 트리거를 활성화하는 함수
    /// </summary>
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

            if (lastAttackTime < attackCooldown)
            {
                animator.SetTrigger("AttackUp");
                ActivateEffect(transform.position);
            }
    }

    /// <summary>
    /// 이펙트 활성화 함수
    /// </summary>
    protected void ActivateEffect(Vector2 effectPosition)
    {
        weaponEffectPrefab.transform.position = effectPosition;
        GameObject weaponEffect = Instantiate(weaponEffectPrefab, effectPosition, Quaternion.identity);
    }
}