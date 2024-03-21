using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // 대쉬 코루틴 애니메이션 코루틴 사용
    // ??


    // 이동
    private float xinput;

    /// <summary>
    /// 플레이어 이동속도
    /// </summary>
    private float maxSpeed = 5.0f;

    /// <summary>
    /// 점프
    /// </summary>
    public float jumpPower = 15.0f;
    public float downJump;
    public float downJumpTime;


    /// <summary>
    /// 땅 체크용 레이어마스크
    /// </summary>
    private Transform groundCheck;
    private LayerMask whatIsGround;

    /// <summary>
    /// 점프 확인
    /// </summary>
    private bool isJump;

    /// <summary>
    /// 점프 횟수
    /// </summary>
    private int jumpCount;

    /// <summary>
    /// 점프 해제 확인용
    /// </summary>
    private bool isJumpOff;

    private Vector2 newVelocity;
    private Vector2 newForce;

    // 레이어 태그
    private int playerLayer;
    private int platformLayer;

    /// <summary>
    /// 맵 체크
    /// </summary>
    private bool isGround;


    /// <summary>
    /// 플레이어 회전
    /// </summary>
    private bool isPosition = false;

    /// <summary>
    /// 마우스 플레이어 회전
    /// </summary>
    private Vector2 mousePos;


    BoxCollider2D box;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator Player_ani;
    CapsuleCollider2D cc;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player_ani = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        cc = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        /* playerLayer = LayerMask.NameToLayer("Player");
         platformLayer = LayerMask.NameToLayer("Platform");*/
    }

    private void Update()
    {

        if (!isJumpOff)
        {
            if (!isGround)
            {
                //Player_anim.SetBool("Jump", true);
                Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, true);
            }
            else
            {
                //Player_anim.SetBool("Jump", false);
                if (!isGround)
                    Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !Input.GetKey(KeyCode.S))
        {
            isJump = true;
        }
    }

    private void FixedUpdate()
    {
        // 플레이어 땅 닿는지 확인
        Debug.DrawRay(rigid.position, Vector3.down * 2, new Color(0, 1, 0));
        MovePosition();

        // 플레이어 애니메이션
        if (xinput != 0)
        {
            Player_ani.SetBool("Run", true);
        }
        else
        {
            Player_ani.SetBool("Run", false);
        }

    }


    private void MovePosition()
    {
        Jump();
        MousePosition();
        xinput = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * xinput, ForceMode2D.Impulse);

        if (rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if (rigid.velocity.x < maxSpeed * (-1))
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }
    }


    private void MousePosition()
    {
        // 마우스 포지션 변경
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 마우스 좌표 캐릭터회전
        // 스프라이트로 X.Y 변경

        if (mousePos.x < transform.position.x)
        {
            if (isPosition)
            {
                // 플레이어 스프라이트 이미지 반전
                //transform.rotation = Quaternion.Euler(0, 180, 0);
                spriteRenderer.flipX = true;

                isPosition = false;
            }
        }

        else if (mousePos.x > transform.position.x)
        {
            if (!isPosition)
            {
                //transform.rotation = Quaternion.identity;
                spriteRenderer.flipX = false;
                isPosition = true;
            }
        }
    }

    void Jump()
    {

        //점프상태가 아니거나 점프 횟수가 없으면 리턴
        if (!isJump || jumpCount == 0)
        {
            return;
        }
        Debug.Log("윗점프");
        rigid.velocity = Vector3.zero;
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        Player_ani.SetBool("Jump", true);
        jumpCount -= 1;
        isJump = false;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {

            jumpCount = 2;
            Player_ani.SetBool("Jump", false);

        }
        jumpCount = 2;
        Player_ani.SetBool("Jump", false);
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S))
            {
                Debug.Log("아래점프");
                cc.enabled = false;
                Invoke("Jumper", 0.4f);
            }
        }
    }

    public void Jumper()
    {
        cc.enabled = true;
    }
}