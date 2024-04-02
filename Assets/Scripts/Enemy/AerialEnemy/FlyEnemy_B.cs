using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy_B : EnemyBase_
{
    // 플레이어가 시야각 안에 없을때 사용할 변수들
    private float idle_speed = 2.0f; // 속도
    private float height = 0.8f; // 왕복 높이
    private Vector3 startPos; // 초기 위치

    Vector3 playerPos;

    /// <summary>
    /// 이동각 변수
    /// </summary>
    Vector3 moveDir;

    protected override void Start()
    {
        base.Start();
        startPos = transform.position; // 시작 위치를 현재 위치로 설정
    }

    protected override void idleAction()
    {
        // 위아래로 흔들린다. ( checkLR 변동 x )
        float newY = Mathf.Sin(Time.time * idle_speed) * height;
        transform.position = startPos + new Vector3(0, newY, 0);
    }

    protected override void attackAction()
    {
        if ( IsMove )
        {
            transform.Translate(Time.deltaTime * mobMoveSpeed * moveDir);
        }
    }

    protected override void firstAction()
    {
        StartCoroutine(targetMove());
    }


    IEnumerator targetMove()
    {
        // 대상 위치를 지정한다. 멈춘다.
        IsMove = false;
        yield return new WaitForSeconds(1.5f);                  // 1.5초 동안 대기한뒤
        playerPos = player.transform.position;
        moveDir = (playerPos - transform.position).normalized;
        IsMove = true;                                          // 플레이어가 있던 위치로 1.5초간 이동한다.
        yield return new WaitForSeconds(1.5f);                  
        StartCoroutine(targetMove());                           // 무한 반복
    }






}
