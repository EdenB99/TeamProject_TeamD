using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : EnemyBase_
{

    //TODO:: 오류: 슬라임이 죽어도 움직이는녀석이 있으며, 죽어도 애니메이션은 계속 작동, 죽은녀석주변에 움직이는 적이있으면 그 적에 끼어서 같이 움직임,
    //또한 아군과 닿으면 아군도 공격함, 점프시 슬라임이 잠깐 사라짐
    //포션에 대한것도 같이 적어놓음, 포션이 왼쪽,오른쪽,아래에서 플레이어에게 닿으면 그냥 벽의역할, 밟아야 먹어짐, 포션을 먹어도 빨간 게이지는 차지만, 피는 그대로임

    /// <summary>
    /// 레이의 길이
    /// </summary>
    float rayLength = 3.0f;

    /// <summary>
    /// 점프 쿨타임
    /// </summary>
    float jumpCool = 3f;
    float lastJumpTime;

    public float jumpForce = 3.0f;

    bool isJumping = false;

    IEnumerator jumpCoroutine;

    readonly int isJump_Hash = Animator.StringToHash("isJump");

    Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        lastJumpTime = -jumpCool;
    }

    protected override void FixedUpdate()
    {
        if (playerDetected && IsLive)
        {
            targetPos = player.transform.position;
            if (IsMove)
            {
                if (targetPos.x < rb.position.x) CheckLR = -1;
                else CheckLR = 1;
            }
        }
        attackAction();

        if (isJumping)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                isJumping = false;
                animator.SetBool(isJump_Hash, false);
            }
        }

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
                if (jumpCoroutine == null) // 코루틴이 이미 실행 중이지 않으면
                {
                    jumpCoroutine = JumpCoroutine(); // 코루틴 할당
                    StartCoroutine(jumpCoroutine); // 코루틴 시작
                }
                firstAction();
            }

            return true;
        }
        return false;
    }

    /// <summary>
    /// 플레이어 추적 및 벽 감지 점프 
    /// </summary>
    protected override void attackAction()
    {
        //  플레이어가 감지되었는지 확인
        if (playerCheck())
        {
            float distanceToPlayerSqr = (transform.position - targetPos).sqrMagnitude;

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
        while(playerDetected)
        {
            if (!isJumping)
            {
                Jump();
            }
            yield return new WaitForSeconds(jumpCool); 
        }
    }

    /// <summary>
    /// 점프
    /// </summary>
    void Jump()
    {
        if (!isJumping && (Time.time - lastJumpTime >= jumpCool))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            animator.SetBool(isJump_Hash, true);
            isJumping = true;
            lastJumpTime = Time.time;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
        }
    }
}
