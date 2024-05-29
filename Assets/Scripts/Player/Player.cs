using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Presets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
    private bool isJumping;

    [Header("대시")]
    private bool canDash = true;
    private bool isDashing;
    public float dashingPower = 5.0f;
    public float dashingTime = 0.2f;
    public float dashCoolTime = 2.0f;
    private float dashInvinciblecool = 0.5f;
    private Vector2 lastDashDirection;
    private float currentdashTime = 0.0f;
    public Action<float, float> OnDashingCoolChanged;

    [Header("플레이어 회전")]
    private bool isPosition = false;        // 플레이어 회전
    private Vector2 mousePos;               // 플레이어 마우스 회전

    TrailRenderer tr;


    // 플레이어 각도
    private float dis = 1.0f;
    private float angle;
    //private float maxAngle = 45.0f;
    private bool isSlope = false;
    private Vector2 prep;

    //골드 관련 변수
    [SerializeField, Range(0.0f, 9999.0f)]
    private uint gold;
    public uint Gold
    {
        get => gold;
        set
        {
            gold = value;
            OnGoldChange?.Invoke(gold);
        }
    }
    public Action<uint> OnGoldChange;

    

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
        Remover.RemoveObjMark(this.gameObject);
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Player_ani = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider2D>();
        playerStats = GetComponent<PlayerStats>();
        inputActions = new PlayerAction();
        tr = GetComponent<TrailRenderer>();
        interactingPortal = FindAnyObjectByType<NextStagePortal>();

        // 플레이어 사망시 작동 정지
        //PlayerStats.OnDie += inputActions.Player.Disable;
    }


    private void Start()
    {
        dialogBox = FindAnyObjectByType<Image>();  // NPC 상호작용
        GameManager.Instance.InventoryUI.PlayerMoveToggle += PlayerMoveToggle;
    }

    private void Update()
    {
        isGrounded = CheckGround();
        if (isGrounded && !isJumping)
        {
            //1
            jumpCount = 1;
        }

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, dis, groundLayerCheck);
        CheckSlope(hit);

        if (!playerStats.IsAlive) // 죽었을때 입력 받기
        {
            revivePlayer();
        }

        if (CheckGround() && isJumping)
        {
            isJumping = false;
            jumpCount = 2;  // 더블 점프를 위해 점프 카운터를 리셋
        }



        if (!isJumpOff)
        {
            if (!isGrounded)
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

        //땅에 닿으면 대쉬가 초기화되서 쿨타임이 UI와 맞지않음
        if (Input.GetMouseButtonDown(1) && canDash)
        {
            currentdashTime = 0.0f;
            StartCoroutine(Dash());
        }
        //대쉬 쿨타임 관련 조건문
        if (currentdashTime < dashCoolTime)
        {
            OnDashingCoolChanged?.Invoke(dashCoolTime, currentdashTime);
            currentdashTime += Time.deltaTime;
            if (currentdashTime >= dashCoolTime)
            {
                canDash = true;
            }
        }

    }

    private void FixedUpdate()
    {
        MovePosition();

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, dis, groundLayerCheck);
        if (isDashing)
        {
            return;
        }
        if (isDashing)
        {
            return;
        }
    }

    private void OnEnable()
    {
        inputEnable();
    }

    private void OnDisable()
    {
        inputDisable();
    }

    void inputEnable()
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

    void inputDisable()
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
    void PlayerMoveToggle(bool toggle)
    {
        if (toggle)
        {
            moveInput = new Vector2(0,0);
            UpdateAnimation();
            inputActions.Player.Move.canceled -= OnMove;
            inputActions.Player.Move.performed -= OnMove;
            inputActions.Player.Jump.performed -= OnJump;
            inputActions.Player.Dash.performed -= OnDash;
            inputActions.Player.DownJump.performed -= OnDownJump;
            canDash = false;
        } else
        {
            inputActions.Player.Move.performed += OnMove;
            inputActions.Player.Move.canceled += OnMove;
            inputActions.Player.Jump.performed += OnJump;
            inputActions.Player.Dash.performed += OnDash;
            inputActions.Player.DownJump.performed += OnDownJump;
            canDash = true;
        }
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
        if (playerStats != null)
        //&& !playerStats.isDead
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
    {//!Input.GetKey(KeyCode.S)
        if (context.performed && jumpCount > 0)
        {
            Jump();
        }
    }



    //TODO:: 플레이어가 계단식 그라운드타일을 올라갈 때 붙어서 떨어지지않고 올라가면 점프횟수가 돌아오지않음,
    void Jump()
    {
        //if (isJumping) return;
        rigid.velocity = Vector2.up * jumpPower; // 점프 파워를 적용하여 즉시 점프
        Player_ani.SetBool("Jump", true);
        isJumping = true; // 점프 상태 설정
        jumpCount--;
    }



    public void OnDownJump(InputAction.CallbackContext context)
    {
        if (Input.GetKey(KeyCode.S)) // 점프 중이 아니고, 땅에 있을 때만 아랫점프 허용
        //if (context.performed && CheckGround() && !isJumping)
        {

        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (Input.GetKey(KeyCode.S))
            {
                // 플랫폼 콜라이더를 일시적으로 무시
                Collider2D platformCollider = collision.collider;
                Collider2D playerCollider = GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(playerCollider, platformCollider, true);
                // 콜라이더를 다시 활성화하는 코루틴 호출
                StartCoroutine(DownCollision(platformCollider, playerCollider));
            }
        }
    }

    IEnumerator DownCollision(Collider2D platformCollider, Collider2D playerCollider)
    {
        yield return new WaitForSeconds(0.25f);
        if (platformCollider != null)
        {
            Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        }
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

    private IEnumerator Dash()
    {
        if (!canDash || !isJump) yield break; // 대시가 가능한 상태인지 확인
        StartCoroutine(DashinvincibleMode());

        canDash = false;
        tr.emitting = true;
        float originalGravity = rigid.gravityScale;
        rigid.gravityScale = 0f; // 대시 동안 중력 영향 제거
        rigid.velocity = Vector2.zero; // 현재 속도 초기화

        Vector2 dashDirection;

        if (moveInput.x == 0) // 플레이어 입력이 없을 경우 마우스 위치를 사용
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dashDirection = (mousePosition - new Vector2(transform.position.x, transform.position.y)).normalized;
        }
        else // 플레이어 입력이 있을 경우 입력 방향을 사용
        {
            dashDirection = new Vector2(moveInput.x, 0).normalized;
            lastDashDirection = dashDirection; // 마지막 대시 방향 업데이트
        }

        dashDirection.y = 0; // Y축 이동 방지
        //rigid.AddForce(dashDirection * dashingPower, ForceMode2D.Impulse); // 계산된 방향으로 대시 힘 적용
        rigid.velocity = dashDirection * dashingPower;

        yield return new WaitForSeconds(dashingTime); // 대시 지속 시간 동안 기다림
        rigid.gravityScale = originalGravity; // 대시가 끝난 후 중력 설정 복원
        //yield return new WaitForSeconds(dashingCool); // 대시 쿨다운 시간
        tr.emitting = false;
        //canDash = true; candash를 Update에서 관리
    }


    /// <summary>
    /// 대시 무적
    /// </summary>
    /// <returns></returns>
    IEnumerator DashinvincibleMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Player_Invincible"); // 레이어를 무적 레이어로 변경

        float timeElapsed = 0.0f;
        while (timeElapsed < dashInvinciblecool) {
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        gameObject.layer = LayerMask.NameToLayer("Player"); // 레이어를 다시 플레이어로 되돌리기
    }


    // 지형 관련 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    /// <summary>
    /// 맵 체크
    /// </summary>
    private bool isGrounded;
    private float checkRadius = 0.2f;
    // Ground Check
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayerCheck;


    private void IsGrounded()
    {
        Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayerCheck);
        if (isGrounded == true && rigid.velocity.y <= 0)
        {
            isJump = false;
            isJumping = false;
            jumpCount = 0;

        }
    }

    /// <summary>
    /// 그라운드 체크에서 오류가뜨면 플레이어 자식에 그라운드 체크 Transform넣어줘야됨
    /// </summary>
    /// <returns></returns>
    private bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayerCheck);
    }

    private void CheckSlope(RaycastHit2D hit)
    {
        if (hit)
        {
            prep = Vector2.Perpendicular(hit.normal).normalized;
            angle = Vector2.Angle(hit.normal, Vector2.up);

            // 각도가 0이 아니면 경사로 간주
            isSlope = angle != 0;

            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.green);
            Debug.DrawLine(hit.point, hit.point + prep, Color.red);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, checkRadius);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Spike") || collision.gameObject.CompareTag("Platform"))
        {
            isJumping = false;
            jumpCount = 2;
            //canDash = true;
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
    public NextStagePortal interactingPortal;
    public TownPortal townptal;
    public QuickPortal interactingQuickPortal;
    /// <summary>
    /// 상호작용 전용
    /// </summary>
    /// <param name="context"></param>
    private void PressF(InputAction.CallbackContext context)
    {
        if (canInteract)
        {
            if (interactingPortal != null && interactingPortal.isInsideTrigger)
            {
                interactingPortal.LoadNextStage();
                return; 
            }

            if(townptal != null && townptal.isInsideTrigger)
            {
                townptal.LodaingNext();
                return;
            }


            if(interactingQuickPortal != null && interactingQuickPortal.isInsideTrigger)
            {
                interactingQuickPortal.OnQuickTrevel();
                return;
            }

            if (interactingNPC != null && interactingNPC.isInsideTrigger)
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
    private void OnTriggerEnter2D(Collider2D other)  // NPC 상호작용
    {
        NPC_Base npc = other.GetComponent<NPC_Base>();
        if (npc != null)
        {
            canInteract = true;
            interactingNPC = npc;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<NPC_Base>() != null)
        {
            if (!interactingNPC.IsInteracting)
            {
                canInteract = true;
            }
            else
            {
                canInteract = false;
                interactingNPC = null;
            }
        }
    }

    // 눌렀을 때 델리게이트 되게끔 수정

    // 죽음 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    void revivePlayer()
    {
        if (!PlayerStats.IsAlive)
        {
            if (Input.GetKey(KeyCode.R)) // 죽었을때 R키를 입력받았다면
            {
                // 플레이어 초기화 ( Hp , 다시 인풋매니저 입력 받도록 )
                Revive();

                // 씬 이동
                SceneManager.LoadScene("Town");
                transform.position = new Vector3(0, 0, 0);

            }
        }
    }

    /// <summary>
    /// 생명력이 0이 되었을때 호출되는 함수
    /// </summary>
    public void Die()
    {
        // 캐릭터가 사망했을 때의 로직 처리
        Player_ani.SetTrigger("Die");
        playerStats.OnDie?.Invoke();
        moveInput = Vector2.zero;
        playerStats.IsAlive = false;
        
        OnDisable();
    }

    /// <summary>
    /// 플레이어 초기화 함수
    /// </summary>
    public void Revive()
    {
        playerStats.IsAlive = true;
        playerStats.CurrentHp = playerStats.MaxHp;
        Player_ani.SetTrigger("Revive");
        OnEnable();
        GameManager.Instance.InventoryUI.ClearAllItem();
    }

}