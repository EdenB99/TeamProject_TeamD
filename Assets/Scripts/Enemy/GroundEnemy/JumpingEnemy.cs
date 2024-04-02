using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : EnemyBase_
{
    /// <summary>
    /// 공격 거리
    /// </summary>
    public float attackDistance = 2f;

    /// <summary>
    /// 레이의 길이
    /// </summary>
    float rayLength = 1.0f;

    /// <summary>
    /// 점프 쿨타임
    /// </summary>
    float jumpCool = 3f;

    float jumpForce = 5.0f;

    bool isJumping = false;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(JumpCoroutine());
    }

    protected override void FixedUpdate()
    {
        if (playerDetected)
        {
            targetPos = player.transform.position;
            if (IsMove)
            {
                if (targetPos.x < rb.position.x) CheckLR = 1;
                else CheckLR = -1;
            }
            attackAction();
        }
        
    }

    /// <summary>
    /// 플레이어 추적 및 벽 감지 점프 
    /// </summary>
    protected override void attackAction()
    {
        //  플레이어가 감지되었는지 확인
        if (playerDetected)
        {
            float distanceToPlayerSqr = ((Vector2)transform.position - targetPos).sqrMagnitude;

            // 플레이어와의 거리가 4f 이상이면 실행 
            if (distanceToPlayerSqr > 4f)
            {

                float step = mobMoveSpeed * Time.deltaTime;
                Vector2 currentPosition = (Vector2)transform.position;
                Vector2 targetPosition = new Vector2(targetPos.x, transform.position.y);

                //목표위치까지 이동
                Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, step);
                transform.position = newPosition;
            }
           

            Vector2 direction = new Vector2(-CheckLR, 0);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, LayerMask.GetMask("Wall"));

            if (hit.collider != null)
            {
                Jump();
            }
        }
    }

    /// <summary>
    /// 점프 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpCoroutine()
    {
        while (true) // 무한 루프
        {
            if (!isJumping)
            {
                Jump();
            }
            yield return new WaitForSeconds(jumpCool); // 점프 쿨타임
            
        }
    }

    /// <summary>
    /// 점프
    /// </summary>
    void Jump()
    {
        isJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = false;
    }
}
