using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy_A : EnemyBase_
{
    // 플레이어가 시야각 안에 없을때 사용할 변수들
    private float idle_speed = 2.0f; // 속도
    private float height = 0.8f; // 왕복 높이
    private Vector3 startPos; // 초기 위치


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
        // 발견시 이동
        Vector3 playerPos = player.transform.position;
        Vector3 moveDir = (playerPos - transform.position ).normalized;

        transform.Translate(Time.deltaTime * mobMoveSpeed * moveDir);




    }






}
