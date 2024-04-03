using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy_C : EnemyBase_
{
    // 플레이어가 시야각 안에 없을때 사용할 변수들
    private float idle_speed = 2.0f; // 속도
    private float height = 0.8f; // 왕복 높이
    private Vector3 startPos; // 초기 위치

    public BulletCode bullet;

    protected override void Start()
    {
        base.Start();
        startPos = transform.position; // 시작 위치를 현재 위치로 설정
    }

    protected override void firstAction()
    {
        StartCoroutine(fire());

    }

    protected override void idleAction()
    {
        // 위아래로 흔들린다. ( checkLR 변동 x )
        float newY = Mathf.Sin(Time.time * idle_speed) * height;
        transform.position = startPos + new Vector3(0, newY, 0);
    }

    protected override void attackAction()
    {
        // 플레이어 위치
        startPos = player.transform.position;
        startPos.y += 5;

        // 자기 위치
        Vector3 currentPos = transform.position;

        // 양 옆으로 흔들린다.
        float newX = Mathf.Lerp(currentPos.x, startPos.x + Mathf.Sin(Time.time * idle_speed) * height * 5 , mobMoveSpeed * Time.deltaTime);

        // 플레이어 + 4 좌표에 대기한다.

        currentPos.y = Mathf.Lerp(currentPos.y, startPos.y , mobMoveSpeed * Time.deltaTime);

        //transform.position = currentPos;
        transform.position = new Vector3(newX, currentPos.y, 0);
    }

    IEnumerator fire()
    {
        yield return new WaitForSeconds(2.0f);
        Vector2 moveDir = (targetPos - transform.position).normalized;

        Factory.Instance.MakeBullet(transform.position,moveDir,bullet);

        StartCoroutine(fire());


    }






}
