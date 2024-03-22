using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // �뽬 �ڷ�ƾ �ִϸ��̼� �ڷ�ƾ ���
    // ??


    // �̵�
    private float xinput;

    /// <summary>
    /// �÷��̾� �̵��ӵ�
    /// </summary>
    private float maxSpeed = 5.0f;

    /// <summary>
    /// ����
    /// </summary>
    public float jumpPower = 15.0f;
    public float downJump;
    public float downJumpTime;


    /// <summary>
    /// �� üũ�� ���̾��ũ
    /// </summary>
    private Transform groundCheck;
    private LayerMask whatIsGround;

    /// <summary>
    /// ���� Ȯ��
    /// </summary>
    private bool isJump;

    /// <summary>
    /// ���� Ƚ��
    /// </summary>
    private int jumpCount;

    /// <summary>
    /// ���� ���� Ȯ�ο�
    /// </summary>
    private bool isJumpOff;

    private Vector2 newVelocity;
    private Vector2 newForce;

    // ���̾� �±�
    private int playerLayer;
    private int platformLayer;

    /// <summary>
    /// �� üũ
    /// </summary>
    private bool isGround;


    /// <summary>
    /// �÷��̾� ȸ��
    /// </summary>
    private bool isPosition = false;

    /// <summary>
    /// ���콺 �÷��̾� ȸ��
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
        // �÷��̾� �� ����� Ȯ��
        Debug.DrawRay(rigid.position, Vector3.down * 2, new Color(0, 1, 0));
        MovePosition();

        // �÷��̾� �ִϸ��̼�
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
        // ���콺 ������ ����
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // ���콺 ��ǥ ĳ����ȸ��
        // ��������Ʈ�� X.Y ����

        if (mousePos.x < transform.position.x)
        {
            if (isPosition)
            {
                // �÷��̾� ��������Ʈ �̹��� ����
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

        //�������°� �ƴϰų� ���� Ƚ���� ������ ����
        if (!isJump || jumpCount == 0)
        {
            return;
        }
        Debug.Log("������");
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
                Debug.Log("�Ʒ�����");
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