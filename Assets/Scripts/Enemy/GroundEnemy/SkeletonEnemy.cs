using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class SkeletonEnemy : EnemyBase_
{ 
    


    /// <summary>
    /// 레이의 길이
    /// </summary>
    float rayLength = 3.0f;

    /// <summary>
    /// 점프 높이
    /// </summary>
    public float jumpForce = 5.0f;

    Transform AttackRange;


    readonly int canAttack_Hash = Animator.StringToHash("canAttack");
    readonly int isWalk_Hash = Animator.StringToHash("isWalk");
    readonly int Die_Hash = Animator.StringToHash("Die");
    readonly int canJump_Hash = Animator.StringToHash("canJump");
    Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        AttackRange = transform.GetChild(0);
    }

    protected override void FixedUpdate()
    {
        if (playerDetected && IsLive) 
        {
            targetPos = player.transform.position;
            if (IsMove)
            {
                if (targetPos.x < rb.position.x) CheckLR = -1;

                else 
                {
                    CheckLR = 1;
                    AttackRange.localScale = new Vector3(-Mathf.Abs(AttackRange.localScale.x), AttackRange.localScale.y);

                }
            }
        }
        attackAction();

        if (!IsLive) // 죽을시
        {
            fade += Time.deltaTime * 0.5f;
            sprite.material.SetFloat(FadeID, 1 - fade);

            if (fade > 1)
            {
                Destroy(this.gameObject); // 1초후 삭제
            }
        }
    }

    bool playerCheck()
    {
        // 범위 내에
        Collider2D colliders = Physics2D.OverlapCircle(transform.position, sightRange, LayerMask.GetMask("Player"));

        // 플레이어가 있다면
        if (colliders != null)
        {


            if (!playerDetected)
            {
                playerDetected = true;
                firstAction();
            }

            return true;
        }
        return false;
    }

    protected override void attackAction()
    {
        //  플레이어가 감지되었는지 확인
        if (playerCheck())
        {
            float distanceToPlayerSqr = (transform.position - targetPos).sqrMagnitude;

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

            Vector2 direction = new Vector2(CheckLR, 0);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool(canAttack_Hash, true);
        }
    }

}
