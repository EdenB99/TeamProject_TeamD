using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("이동")]
    private Vector2 moveInput; // 이동 입력을 저장할 변수

    [Header("점프")]
    public float jumpPower = 15.0f;
    public float downJump;
    public float downJumpTime;
    private bool isJump = true;
    private int jumpCount;
    private bool isJumpOff;

    [Header("대시")]
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 5.0f;
    public float dashingTime = 0.2f;
    public float dashCoolTime = 2.0f;
    private float currentdashTime = 0.0f;
    public Action<float, float> OnDashingCoolChanged;

    [Header("플레이어 회전")]
    private bool isPosition = false;        // 플레이어 회전
    private Vector2 mousePos;               // 플레이어 마우스 회전

    TrailRenderer tr;

    // 레이어 태그
    private int playerLayer;
    private int platformLayer;

    private PlayerStats playerStats;

    /// <summary>
    /// 외부에서 playerStats 을 읽기위한 프로퍼티
    /// </summary>
    public PlayerStats PlayerStats { get { return playerStats; } }

    PlayerAction inputActions;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Animator Player_ani;
    private CapsuleCollider2D cc;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player_ani = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider2D>();
        playerStats = GetComponent<PlayerStats>();
        inputActions = new PlayerAction();
        tr = GetComponent<TrailRenderer>();

        // 플레이어 사망시 작동 정지
        PlayerStats.OnDie += inputActions.Player.Disable;
    }

    
    private void Start()
    {
        dialogBox = FindAnyObjectByType<Image>();  // NPC 상호작용
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
                Physics2D.IgnoreLayerCollision(playerLayer, platformLayer, false);

            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            currentdashTime = 0.0f;
            StartCoroutine(Dash());
        }
        //대쉬 쿨타임 관련 조건문
        if (currentdashTime < dashCoolTime)
        {
            OnDashingCoolChanged?.Invoke(dashCoolTime,currentdashTime);
            currentdashTime += Time.deltaTime;
            if (currentdashTime >= dashCoolTime)
            {
                canDash = true;
            }
        }

    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        MovePosition();
        if (isDashing)
        {
            return;
        }
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Dash.performed += OnDash;
        inputActions.Player.Interaction.performed += PressF; // NPC 상호작용 버튼 F 
        inputActions.Player.Esc.performed += ESC;
        inputActions.Player.DownJump.performed += OnDownJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Dash.performed -= OnDash;
        inputActions.Player.Interaction.performed -= PressF; // NPC 상호작용 버튼 F
        inputActions.Player.Esc.performed -= ESC;
        inputActions.Player.DownJump.performed -= OnDownJump;
        inputActions.Player.Disable();
    }


    // 이동 , 애니메이션 관련 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

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
        float moveDistance = moveInput.x * playerStats.Speed * Time.fixedDeltaTime;
        transform.position += new Vector3(moveDistance, 0, 0);
    }


    protected void MousePosition()
    {
        if (playerStats != null && playerStats.IsAlive )
        {
            // 마우스 포지션 변경
           // Camera camera = FindAnyObjectByType<Camera>();
            //if ( camera != null)
            {
                mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                bool shouldFlip = mousePos.x < transform.position.x;
                if (spriteRenderer.flipX != shouldFlip)
                {
                    spriteRenderer.flipX = shouldFlip;
                    isPosition = !isPosition;
                }
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

    
    //TODO:: 플레이어가 계단식 그라운드타일을 올라갈 때 붙어서 떨어지지않고 올라가면 점프횟수가 돌아오지않음, Spike태그의 Spike타일에선 점프불가능
    void Jump()
    {
        // 점프 로직
        rigid.velocity = Vector2.up * jumpPower; // 점프 파워를 적용하여 즉시 점프
        Player_ani.SetBool("Jump", true);
        isJump = true;
        jumpCount -= 1;
    }


    private void OnDash(InputAction.CallbackContext context)
    {
        if (canDash)
        {
            currentdashTime = 0.0f;
            // 애니메이션 넣고 출력
            StartCoroutine(Dash());
        }
    }

    // 대쉬 점프 수정중
    private IEnumerator Dash()
    {
        if (!canDash || !isJump ) yield break; // 대시가 가능한 상태인지 확인

        Debug.Log("대시");
        canDash = false;
        tr.emitting = true;
        float originalGravity = rigid.gravityScale;
        rigid.gravityScale = 0f; // 대시 동안 중력 영향 제거
        rigid.velocity = Vector2.zero; // 현재 속도 초기화

        Vector2 dashDirection;

        /*
        if (moveInput.x == 0) // 플레이어 입력이 없을 경우 마우스 위치를 사용
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dashDirection = (mousePosition - new Vector2(transform.position.x, transform.position.y)).normalized;
        }
        */

        dashDirection = new Vector2(moveInput.x, 0).normalized;
        rigid.AddForce(dashDirection * dashingPower, ForceMode2D.Impulse); // 계산된 방향으로 대시 힘 적용
        Debug.Log($"대시 방향: {dashDirection}");

        yield return new WaitForSeconds(dashingTime); // 대시 지속 시간 동안 기다림

        rigid.gravityScale = originalGravity; // 대시가 끝난 후 중력 설정 복원
        tr.emitting = false;
    }

    // 지형 관련 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    /// <summary>
    /// 맵 체크
    /// </summary>
    private bool isGround;

    public void OnDownJump(InputAction.CallbackContext context)
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            jumpCount = 1;
            Player_ani.SetBool("Jump", false);
        }
    }

    // NPC 관련 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    /// <summary>
    /// NPC 상호작용 전용
    /// </summary>
    Image dialogBox;
    bool canInteract = false;
    NPC_Base interactingNPC;

    /// <summary>
    /// NPC 상호작용 전용
    /// </summary>
    /// <param name="context"></param>
    private void PressF(InputAction.CallbackContext context)
    {
        if (canInteract && interactingNPC != null)
        {
            if (!interactingNPC.IsInteracting)
            {
                interactingNPC.StartDialog();
            }
            else
            {
                interactingNPC.NextDialog();
            }
        }
    }

    private void ESC(InputAction.CallbackContext context)
    {
        if (canInteract && interactingNPC != null)
        {
            if (interactingNPC.IsInteracting)
            {
                interactingNPC.EndDialog();
            }
        }

        if (interactingNPC != null && interactingNPC is NPC_Store)
        {
            NPC_Store store = (NPC_Store)interactingNPC;
            if (store.StoreUI.gameObject.activeSelf)
            {
                store.DisableStore();
            }
        }
    }



    private void OnCollisionStay2D(Collision2D other)
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

    private void OnTriggerEnter2D(Collider2D other)  // NPC 상호작용
    {
        NPC_Store npc = other.GetComponent<NPC_Store>();
        if (npc != null)
        {
            canInteract = true;
            interactingNPC = npc;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<NPC_Store>() != null)
        {
            canInteract = false;
            interactingNPC = null;
        }
    }

    public void Jumper()
    {
        cc.enabled = true;
    }

}