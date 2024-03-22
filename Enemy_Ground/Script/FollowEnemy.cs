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

    readonly int canAttack_Hash = Animator.StringToHash("canAttack");
    readonly int isWalk_Hash = Animator.StringToHash("isWalk");
    readonly int Die_Hash = Animator.StringToHash("Die");
    readonly int canJump_Hash = Animator.StringToHash("canJump");
    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        hp = maxHp;
    }

    protected override void FixedUpdate()
    {
        if (playerCheck) 
        {
            targetPos = player.transform.position;
            if (IsMove)
            {
                if (targetPos.x < rb.position.x) CheckLR = 1;
                else CheckLR = -1;
            }
        }
        checkNow();
    }

    protected override void attackAction()
    {
        animator.SetTrigger(canAttack_Hash);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            attackAction(); // 공격 액션 수행
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerCheck = true;
        }
    }

    /// <summary>
    /// 플레이어 추적 및 벽 감지 점프 
    /// </summary>
    protected override void checkNow()
    {
        //  플레이어가 감지되었는지 확인
        if (playerCheck)
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

    /// <summary>
    /// 점프
    /// </summary>
    void Jump()
    {
        animator.SetTrigger(canJump_Hash);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
