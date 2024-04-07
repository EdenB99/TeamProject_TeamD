using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : RecycleObject
{
    public BulletType bulletType;

    /// <summary>
    /// ��������Ʈ ( ���Ŀ� ��������Ʈ�� ���������ϰ� ���� )
    /// </summary>
    SpriteRenderer spriteRenderer = null;
    BulletData data = null;

    /// <summary>
    /// �ǰݹ���
    /// </summary>
    CircleCollider2D circleCollider;

    /// <summary>
    /// �÷��̾� �ҷ����� 
    /// </summary>
    protected Player player;

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

    /// <summary>
    /// ź�� ������
    /// </summary>
    public uint bulletDamage = 1;

    /// <summary>
    /// ������Ÿ��
    /// </summary>
    public float LifeTime = 1;

    /// <summary>
    /// ź�� ���� �Ѵ��� ���Ѵ��� ����
    /// </summary>
    public bool isThrought;

    /// <summary>
    /// ź�� �ǰ� ���� (�и� , ����)
    /// </summary>
    public bool isParring;

    /// <summary>
    /// True���, �÷��̾��� �Ҹ��� �ȴ�.
    /// </summary>
    public bool isPlayer;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    protected override void OnEnable()
    {
        data = null;
        base.OnEnable();
        
    }

    private void Start()
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
                moveDir = (player.transform.position - transform.position).normalized;
                transform.Translate(Time.deltaTime * moveSpeed * moveDir);
                break;

        }
    }



    /// <summary>
    /// �浹�� �����ϴ� �޼���
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.CompareTag("Platform") ) // �����̶�� 
        {
            if ( isThrought )
            {
                Die();
            }
        }
    }

    /// <summary>
    /// ���� �ð��� ��������� �ϴ� �޼���
    /// </summary>
    /// <returns></returns>
    IEnumerator BulletDelTime()
    {
        yield return new WaitForSeconds(LifeTime);
        Die();
    }

    /// <summary>
    /// ������� ���� �� �޼���
    /// </summary>
    public void Die()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    public BulletData BulletData
    {
        get => data;
        set
        {
            Debug.Log("Create");

            if (data != value)
            {
                data = value;
                //������ ���� �ٲ۴�
                bulletType = data.bulletType;      // �̵����
                moveSpeed = data.moveSpeed;      // �̵� ���ǵ�
                bulletDamage = data.bulletDamage;  // ������
                isParring = data.isParring;
                isThrought = data.isThrougt;
                LifeTime = data.lifeTime;
                circleCollider.radius = data.bulletSize; // �ǰ� ����
                spriteRenderer.sprite = data.bulletIcon; // ��������Ʈ

                StartCoroutine(BulletDelTime());
            }
        }
    }
}
