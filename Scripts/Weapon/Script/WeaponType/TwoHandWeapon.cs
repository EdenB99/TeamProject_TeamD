using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoHandWeapon : WeaponBase
{
    public GameObject weapon;
    private Transform weaponTransform;
    private Animator animator;

    /// <summary>
    /// 무기 공격 이펙트 프리팹
    /// </summary>
    public GameObject weaponEffectPrefab;

    ///// <summary>
    ///// 현재 무기 이펙트 인스턴스
    ///// </summary>
    ////private GameObject currentWeaponEffect;

    /// <summary>
    /// 공격시 무기가 회전하는 각도
    /// </summary>
    public float targetAngle = 220.0f;


    public float moveSpeed = 20.0f;


    public Vector2 targetPosition;

    bool isAttack = false;

    protected override void Awake()
    {
        base.Awake();
        weapon = GameObject.FindGameObjectWithTag("Weapon");
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
    }



    private void Rotate()
    {
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        // 회전할 시간 설정
        float rotationTime = 0.5f;

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation;

        // 플레이어가 왼쪽을 바라보고 있는지 확인
        if (player.transform.localScale.x < 0)
        {
            // 플레이어가 왼쪽을 바라보고 있으면 회전 각도를 반대로 설정
            targetRotation = Quaternion.Euler(0, 0, -targetAngle);
        }
        else
        {
            // 플레이어가 오른쪽을 바라보고 있으면 기존 회전 각도를 사용
            targetRotation = Quaternion.Euler(0, 0, targetAngle);
        }

        // 회전 진행
        float elapsedTime = 0f;
        while (elapsedTime < rotationTime)
        {
            // 시간에 따른 회전 각도 계산
            float t = elapsedTime / rotationTime;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }


    /// <summary>
    /// 무기를 공격할 때 공격속도를 적용
    /// </summary>
    public new void Attack()
    {
        if (player != null && player.transform != null)
        {
            Rotate();
        }

    }
}

