using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBase_ : MonoBehaviour, IEnemy
{
    //������Ʈ �ҷ�����
    protected Rigidbody2D rb;
    public SpriteRenderer sprite;

    /// <summary>
    /// �÷��̾� �ҷ�����
    /// </summary>
    protected Player player;

    /// <summary>
    /// �÷��̾� ��ġ Ÿ����
    /// </summary>
    protected Vector3 targetPos;

    /// <summary>
    /// �÷��̾� �߰� ����
    /// </summary>
    [SerializeField]
    protected bool playerDetected;

    /// <summary>
    /// �� ��ü�� ������ ( �ε����� ��츸 )
    /// </summary>
    public int mobDamage = 0;

    /// <summary>
    /// �� ��ü�� �̵��ӵ�
    /// </summary>
    public float mobMoveSpeed = 0;

    /// <summary>
    /// ���� �̵��ϴ��� (������) �� ���� ���� false�� ������ȯ + �̵��� ���� �ʴ´�.
    /// </summary>
    public bool IsMove = true;

    /// <summary>
    /// ���� �þ� ����
    /// </summary>
    public float sightRange = 1.0f;

    /// <summary>
    /// HP������
    /// </summary>
    protected float hp = 100.0f;

    /// <summary>
    /// HP
    /// </summary>
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            hp = Mathf.Max(hp, 0);
            // Hp�� 0 ���ϸ� ���
            if (hp <= 0)
            {
                Die();
            }
        }
    }

    /// <summary>
    /// �¿� Ȯ��
    /// </summary>
    public int checkLR = 1;

    /// <summary>
    /// �¿� ����� ������Ƽ
    /// </summary>
    public int CheckLR
    {
        get { return checkLR; }
        set
        {
            if (checkLR != value) // ���� ���� �Ǿ��ٸ�
            {
                checkLR = value;
                // ��������Ʈ ���� ��ȯ 
                if (checkLR == 1) sprite.flipX = false; else { sprite.flipX = true; }
            }

        }
    }

    public float maxHP = 100.0f;
    public float MaxHP => maxHP;

    /// <summary>
    /// �� ��ü�� ������ ( �ε����� ��츸 )
    /// </summary>
    public uint Attackpower = 1;
    public uint AttackPower => Attackpower;

    /// <summary>
    /// ���� ��������Ʈ
    /// </summary>
    public Action onDie { get; set; }
    

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected virtual void FixedUpdate()
    {
        if ( playerDetected ) // �÷��̾� �߽߰� �ൿ
        {
            // �÷��̾��� ��ġ�� �޴´�.
            targetPos = player.transform.position;
            // �����̴� ������ ��쿡��
            if (IsMove)
            {
                // �÷��̾��� ��ġ�� ���� CheckLR �� �����Ѵ�.
                if (targetPos.x < rb.position.x) CheckLR = 1;
                else CheckLR = -1;
            }
            attackAction();
  

        }
        else // �÷��̾� �̹߽߰� �ൿ
        {
            
            playerCheck();
            idleAction();
        }
    }

    /// <summary>
    /// Update���� ����� �ڵ� ( �÷��̾� �߰� )
    /// </summary>
    protected virtual void attackAction()
    {

    }

    /// <summary>
    /// Update���� ����� �ڵ� ( �÷��̾� �̹߰� )
    /// </summary>
    protected virtual void idleAction()
    {

    }

    /// <summary>
    /// �浹�� �����ϴ� �޼���
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }


    /// <summary>
    /// �÷��̾ Ž���ϴ� ���� �����ϴ� �޼��� SightRange �ȿ� ������ �÷��̾ �ִ°�.
    /// </summary>
    /// <returns>���� true = �÷��̾ �������� �ִ�.</returns>
    private bool playerCheck()
    {
        // ���� ����
        Collider2D colliders = Physics2D.OverlapCircle(transform.position, sightRange, LayerMask.GetMask("Player"));

        // �÷��̾ �ִٸ�
        if (colliders != null)
        {
            

            if (!playerDetected)
            {
                playerDetected = true;
                firstAction();
            }

            return true;
        }
        return false;
    }

    /// <summary>
    /// �÷��̾ ù ���������� �� �ൿ ( �Ϲ������� 1ȸ ���� )
    /// </summary>
    protected virtual void firstAction()
    {

    }

    /// <summary>
    /// ���ظ� �޾����� ������ �Լ� ����
    /// </summary>
    /// <param name="Damage">�÷��̾�� ���� ����</param>
    /// <exception cref="NotImplementedException"></exception>
    private void Damaged(int Damage)
    {
        HP -= Damage;
    }

    /// <summary>
    /// ���ظ� �ִ� �޼���
    /// </summary>
    public void Attack()
    {
        // �÷��̾�� �����ִ°Ͱ� ���õ� �ൿ ����
    }

    /// <summary>
    /// ���ظ� �޴� �޼���
    /// </summary>
    /// <param name="damage"></param>
    public void Damaged(float damage)
    {
        HP -= damage;
    }

    [System.Serializable]
    public struct ItemDropInfo
    {
        public ItemCode code;       // ������ ����
        [Range(0, 1)]
        public float dropRatio;     // ��� Ȯ��(1.0f = 100%)
        public uint dropCount;      // �ִ� ��� ����
    }

    public ItemDropInfo[] dropItems;

    /// <summary>
    /// ������ ��� �޼��� / �Ϲ������� Die���� ����
    /// </summary>
    public void ItemDrop()
    {
        foreach(var item in dropItems)
        {
            if ( item.dropRatio > Random.value )
            {
                GameObject obj = Factory.Instance.MakeItem(item.code);
                obj.transform.position = transform.position;

            }
        }
    }

    /// <summary>
    /// �׾����� ���� �� �޼���
    /// </summary>
    public void Die()
    {
        Debug.Log("�׾���.");
        StopAllCoroutines();
        ItemDrop();
    }
}
