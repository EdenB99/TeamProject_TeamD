using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTest : MonoBehaviour
{
    public float jumpForce = 10f; // ���� �� ������ ���� ����

    private Rigidbody2D rb; // �÷��̾� ĳ������ Rigidbody2D ������Ʈ�� �����ϱ� ���� ����
    private bool isGrounded = false; // �÷��̾ ���� �ִ��� ���θ� ��Ÿ���� ����

    void Start()
    {
        // Rigidbody2D ������Ʈ�� ������
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Space Ű�� ������ ���� �Լ� ȣ��
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        // �Ʒ� �������� ���� ���� ����
        rb.velocity = new Vector2(rb.velocity.x, -jumpForce);
        Debug.Log("Down");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ����� �� isGrounded�� true�� ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // ������ ����� �� isGrounded�� false�� ����
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

