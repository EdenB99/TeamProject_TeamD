using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Searcher.SearcherWindow.Alignment;


public class Player : MonoBehaviour
{

    [Header("이동")]
    private Vector2 moveInput; // 이동 입력을 저장할 변수
    public float maxSpeed = 5.0f;


    [Header("점프")]
    public float jumpPower = 15.0f;
    public float downJump;
    public float downJumpTime;
    private bool isJump;
    private int jumpCount;  
    private bool isJumpOff;

    [Header("대시")]
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 2.0f;
    private float dashingTime = 0.2f;
    private float dashingCool = 1f;


    private Vector2 newVelocity;
    private Vector2 newForce;

    // 레이어 태그
    private int playerLayer;
    private int platformLayer;

    /// <summary>
    /// 맵 체크
    /// </summary>
    private bool isGround;
    private float checkRadius = 0.2f;

    /// <summary>
    /// 땅 체크용 레이어마스크
    /// </summary>
    private Transform groundCheck;
    private LayerMask groundLayer;


    [Header("플레이어 회전")]
    private bool isPosition = false;        // 플레이어 회전
    private Vector2 mousePos;               // 플레이어 마우스 회전


    private PlayerStats playerStats;


    PlayerAction inputActions;
    public BoxCollider2D box;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigid;
    public Animator Player_ani;
    public CapsuleCollider2D cc;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player_ani = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();
        cc = GetComponent<CapsuleCollider2D>();
        playerStats = GetComponent<PlayerStats>();
        inputActions = new PlayerAction();

    }

    private void Start()
    {
        /* playerLayer = LayerMask.NameToLayer("Player");
         platformLayer = LayerMask.NameToLayer("Platform");*/

        groundLayer = LayerMask.GetMask("Ground"); // 'Ground' 레이어마스크를 가져오기
    }

    private void Update()
    {
        if(isDashing)
        {
            return;
        }

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

        
    }


    private void FixedUpdate()
    {
        // 플레이어 땅 닿는지 확인
        //Debug.DrawRay(rigid.position, Vector3.down * 2, new Color(0, 1, 0));

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }


        MovePosition();
        if(isDashing)
        {
            return;
        }

        // 대쉬
        //rigid.AddForce(newForce * maxSpeed,ForceMode2D.Force);

        //isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Dash.performed += OnDash;
        inputActions.Player.DownJump.performed += OnDownJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Dash.performed -= OnDash;
        inputActions.Player.DownJump.performed -= OnDownJump;
        inputActions.Player.Disable();
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // 입력 값을 읽어 moveInput에 저장
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        // moveInput.x 값에 따라 Run 애니메이션을 활성화하거나 비활성화
        Player_ani.SetBool("Run", moveInput.x != 0);
    }


    private void MovePosition()
    {
        MousePosition();    // 마우스 포지션 변경 
        float moveDistance = moveInput.x * maxSpeed * Time.fixedDeltaTime;
        transform.position += new Vector3(moveDistance, 0, 0);
    }


    protected void MousePosition()
    {
        if (playerStats != null && playerStats.IsAlive)
        {
            // 마우스 포지션 변경
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            bool shouldFlip = mousePos.x < transform.position.x;
            if (spriteRenderer.flipX != shouldFlip)
            {
                spriteRenderer.flipX = shouldFlip;
                isPosition = !isPosition;
            }
        }
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && jumpCount > 0)
        {
            Jump();
        }
    }

    void Jump()
    {
        // 점프 로직
        Debug.Log("윗점프");
        rigid.velocity = Vector2.up * jumpPower; // 점프 파워를 적용하여 즉시 점프
        Player_ani.SetBool("Jump", true);
        jumpCount -= 1;
    }


    public void OnDownJump(InputAction.CallbackContext context)
    {
        if (context.performed || isGround)
        {
            StartCoroutine(DownJump());
            Debug.Log("아랫점프");
        }
    }

    IEnumerator DownJump()
    {
        // 하단 점프를 위해 플레이어의 Collider를 잠시 비활성화
        cc.enabled = false;
        // 플랫폼과의 충돌을 잠시 무시하는 기간 설정
        yield return new WaitForSeconds(0.2f);
        // Collider를 다시 활성화
        cc.enabled = true;
    }

    private bool IsGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        // 대시 로직 구현
        StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rigid.gravityScale = 0f;
        rigid.AddForce(newForce * dashingPower, ForceMode2D.Impulse);
        //tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        //tr.emitting = false;
        //rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCool);
        canDash = true;
        Debug.Log("대시");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            jumpCount = 2;
            Player_ani.SetBool("Jump", false);
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.S))
            {
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