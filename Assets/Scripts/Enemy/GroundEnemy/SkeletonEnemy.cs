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
    float rayLength = 2.0f;

    /// <summary>
    /// 점프 높이
    /// </summary>
    public float jumpForce = 5.0f;

    /// <summary>
    /// 칼질범위
    /// </summary>
    public float bladeRange = 4.0f;

    /// <summary>
    /// 스피드
    /// </summary>
    public float speed = 3.0f;

    /// <summary>
    /// 공격중인지
    /// </summary>
    bool IsAttack = true;

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

    protected override void attackAction()
    {
        Vector2 direction = new Vector2(-CheckLR, 0);
        float distanceToPlayerSqr = (transform.position - targetPos).sqrMagnitude;

        // 플레이어와의 거리가 칼질 범위 밖이다.
        if (distanceToPlayerSqr > bladeRange && IsAttack)
        {
            transform.Translate(Time.fixedDeltaTime * speed * direction); // 이동
            animator.SetBool(isWalk_Hash, true);
        }
        else if (IsAttack) // 칼질 범위 내부다
        {
            StartCoroutine(CoAttack());
        }

        
        // 벽을 만나면 점프하는 함수
        Vector3 up = new Vector2(0,0.2f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position + up, direction, rayLength, LayerMask.GetMask("Wall"));
        if (hit.collider != null)
        {
            Jump();
        }
    }

    protected override void spriteDirection()
    {
        if (playerDetected && IsLive && IsAttack)
        {
            gameObject.transform.localScale = new Vector3(1.0f * -CheckLR, 1.0f, 1.0f);
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

    IEnumerator CoAttack()
    {
        IsAttack = false;
        animator.SetBool(canAttack_Hash, true);
        yield return new WaitForSeconds(1.1f);
        IsAttack = true;
    }

}
