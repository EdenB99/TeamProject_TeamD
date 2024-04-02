using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Base : RecycleObject
{
    public BulletType bulletType;

    /// <summary>
    /// �÷��̾� �ҷ����� 
    /// </summary>
    protected Player player;

    /// <summary>
    /// ��������Ʈ ( ���Ŀ� ��������Ʈ�� ���������ϰ� ���� )
    /// </summary>
    // SpriteRenderer spriteRenderer = null;

    /// <summary>
    /// Ư�� ��ġ Ÿ����
    /// </summary>
    Vector2 targetPos;

    /// <summary>
    /// �̵�����
    /// </summary>
    public Vector2 moveDir;

    /// <summary>
    /// ź�� �̵��ӵ�
    /// </summary>
    public float moveSpeed = 5;

    public uint bulletDamage = 1;

    /// <summary>
    /// ź�� ���� �Ѵ��� ���Ѵ��� ����
    /// </summary>
    public bool isThrougt;

    /// <summary>
    /// ź�� �ǰ� ���� (�и� , ����)
    /// </summary>
    public bool isParring;

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    private void FixedUpdate()
    {
        switch (bulletType)
        {
            case BulletType.Bullet_Straight:
                transform.Translate(Time.deltaTime * moveSpeed * moveDir);
                break;
            case BulletType.Bullet_Chase:
                break;

        }
    }



    /// <summary>
    /// �浹�� �����ϴ� �޼���
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }

    /// <summary>
    /// ������� ���� �� �޼���
    /// </summary>
    public void Die()
    {
        StopAllCoroutines();
    }
}
