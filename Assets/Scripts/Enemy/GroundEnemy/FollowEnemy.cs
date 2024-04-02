using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowEnemy : EnemyBase_
{ 
    /// <summary>
    /// 레이의 길이
    /// </summary>
    float rayLength = 3.0f;

    /// <summary>
    /// 점프 높이
    /// </summary>
    float jumpForce = 5.0f;

    readonly int canAttack_Hash = Animator.StringToHash("canAttack");
    readonly int isWalk_Hash = Animator.StringToHash("isWalk");
    readonly int Die_Hash = Animator.StringToHash("Die");
    readonly int canJump_Hash = Animator.StringToHash("canJump");
    Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
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
        }
        attackAction();
    }

    /*
    protected override void attackAction()
    {

        animator.SetTrigger(canAttack_Hash);
    }
    */

    protected override void attackAction()
    {
        //  플레이어가 감지되었는지 확인
        if (playerDetected)
        {
            float distanceToPlayerSqr = ((Vector2)transform.position - targetPos).sqrMagnitude;

            // 플레이어와의 거리가 4f 이상이면 실행 
            if (distanceToPlayerSqr > 4f)
            {
                animator.SetBool(isWalk_Hash, true);

                float step = mobMoveSpeed * Time.deltaTime;
                Vector2 currentPosition = (Vector2)transform.position;
                Vector2 targetPosition = new Vector2(targetPos.x, transform.position.y);

                //목표위치까지 이동
                Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, step);
                transform.position = newPosition;
            }
            else
            {
                animator.SetBool(isWalk_Hash, false);
            }

            Vector2 direction = new Vector2(-CheckLR, 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, LayerMask.GetMask("Wall"));

            if (hit.collider != null)
            {
                Jump();
            }
        }
    }
    new private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attackAction(); // 공격 액션 수행

        }
    }



   

    /// <summary>
    /// 점프
    /// </summary>
    void Jump()
    {
        animator.SetTrigger(canJump_Hash);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
