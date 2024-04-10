using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /// 이펙트 활성화 여부
    /// </summary>
    private bool isEffectActive = false;

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


    private float lastAttackTime = 0f; // 마지막 공격 시간

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
    }



    protected void Update()
    {
        UpdateWeaponPosition();

        // 공격 입력 처리
        if (Input.GetButtonDown("Fire1") && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }


    /// <summary>
    /// 업데이트된 무기의 위치좌표
    /// </summary>
    protected virtual void UpdateWeaponPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);        // 마우스 위치 = 스크린상 월드 좌표
        mousePosition.z = 0f;
        transform.position = hinge.position;            // 힌지 위치 불러오기

        Vector3 direction = mousePosition - transform.position; // 방향 벡터 계산
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = rotation;

        Collider2D weaponCollider = GetComponent<Collider2D>();
        if (weaponCollider != null)
        {
            Vector3 pivotPosition = hinge.position; // 무기의 콜라이더 중심 위치를 기준으로 설정
            Vector3 rayDirection = Vector3.forward; // 2D 게임에서는 z축으로 깊이를 고려하지 않으므로 앞쪽 방향을 사용
            RaycastHit2D hit = Physics2D.Raycast(pivotPosition, rayDirection);

            if (hit.collider != null && hit.collider != weaponCollider)
            {
                // 레이가 무기의 콜라이더와 충돌하지 않은 위치에 무기 이펙트를 활성화
                Vector2 effectPosition = hit.point;
                ActivateEffect(effectPosition);
            }
        }

    }

    //protected void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy"))
    //    {
    //        /* 추후에 인터페이스로 에너미 찾을 예정
    //        Enemy enemy = collision.GetComponent<Enemy>();
    //        if (enemy != null)
    //        {
    //            float totalDamage = weaponDamage + playerStats.attackPower;
    //            enemy.Damaged(totalDamage);
    //        }
    //        */
    //    }
    //}

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
        animator.SetTrigger("Attack");
        Debug.Log("공격트리거 발동");



        ActivateEffect(transform.position);

        //// 공격 속도에 따라 애니메이션 속도 조절
        //float attackAnimationSpeed = playerStats.attackSpeed;
        //animator.SetFloat("weaponSpeed", attackAnimationSpeed);

    }


    /// <summary>
    /// 이펙트 활성화 함수
    /// </summary>
    protected void ActivateEffect(Vector2 spawnPoint)
    {
        if (isEffectActive)
            return;

        GameObject weaponEffectInstance = Instantiate(weaponEffectPrefab, spawnPoint, Quaternion.identity);
        isEffectActive = true;
        weaponEffectPrefab.transform.position = spawnPoint;

        StartCoroutine(DeactivateEffectAfterAnimation(weaponEffectInstance));

        Debug.Log("무기 이펙트 활성화");
    }

    private IEnumerator DeactivateEffectAfterAnimation(GameObject weaponEffect)
    {
        yield return new WaitForSeconds(weaponEffect.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
        Destroy(weaponEffect);
        isEffectActive = false;
    }

}