using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class EnemyBase_ : MonoBehaviour, IEnemy , IAttack
{
    //������Ʈ �ҷ�����
    protected Rigidbody2D rb;
    Collider2D col;
    protected SpriteRenderer sprite;
    Sprite sprite2d;
    Texture2D texture;

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
    protected float hp = 1.0f;

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
            if (hp <= 0 && IsLive)
            {
                Die();
            }
        }
    }

    protected bool IsLive = true;

    /// <summary>
    /// �¿� Ȯ��
    /// </summary>
    private int checkLR = 1;

    /// <summary>
    /// �¿� ����� ������Ƽ
    /// </summary>
    public int CheckLR
    {
        get { return checkLR; }
        set
        {
            if (checkLR != value && IsLive) // ���� ���� �Ǿ��ٸ�
            {
                checkLR = value;
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
    /// ���� ó��
    /// </summary>
    public Action onDie { get; set; }

    readonly int Texture2DID = Shader.PropertyToID("_Texture2D");
    protected readonly int FadeID = Shader.PropertyToID("_Fade");
    readonly int HitID = Shader.PropertyToID("_Hit");
    protected float fade = 0.0f;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // �� ���׸��� ��������
        sprite.material = GameManager.Instance.Mobmaterial;
        sprite2d = sprite.sprite;
        texture = sprite2d.texture;
        sprite.material.SetTexture(Texture2DID, texture);

        HP = MaxHP;
    }

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected virtual void FixedUpdate()
    {
        spriteDirection();

        if ( playerDetected && IsLive) // �÷��̾� �߽߰� �ൿ
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
        else if ( IsLive) // �÷��̾� �̹߽߰� �ൿ
        {
            
            playerCheck();
            idleAction();
        }

        if ( !IsLive ) // ������
        {
            fade += Time.deltaTime * 0.5f;
            sprite.material.SetFloat(FadeID, 1 - fade);

            if ( fade > 1 )
            {
                Destroy(this.gameObject); // 1���� ����
            }
        }
    }

    private void OnDestroy()
    {
        MapManager map = FindAnyObjectByType<MapManager>();

        if (map != null)
        {
            map.CheckEnemysInScene(map.CurrentMap);
        }
    }

    /// <summary>
    /// ��������Ʈ �������
    /// </summary>
    protected virtual void spriteDirection()
    {
        if ( playerDetected && IsLive)
        {
            if (checkLR == 1) sprite.flipX = false; else { sprite.flipX = true; }
            if (checkLR == -1) sprite.flipX = true; else { sprite.flipX = false; }
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
    /// �ǰ�
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<IAttack>() != null)            // IAttack�� ������ �ְ�
        {
            IAttack attack = collision.GetComponent<IAttack>();     // ������Ʈ �����ͼ�

            TakeDamage(attack.AttackPower);                         // �ش� ������Ʈ�� AttackPower��ŭ ���ظ� ����.

            if ( !playerDetected )                              // �߰����� ���߾ ���ظ� ������ �߰�ó��
            {
                playerDetected = true;
            }
        }
    }


    /// <summary>
    /// �÷��̾ Ž���ϴ� ���� �����ϴ� �޼��� SightRange �ȿ� ������ �÷��̾ �ִ°�.
    /// </summary>
    /// <returns>���� true = �÷��̾ �������� �ִ�.</returns>
    protected bool playerCheck()
    {
        // ���� ����
        Collider2D colliders = Physics2D.OverlapCircle(transform.position, sightRange, LayerMask.GetMask("Player"));

        // �÷��̾ �ִٸ�
        if (colliders != null && IsLive)
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
    public void TakeDamage(float damage)
    {
        if ( IsLive )
        {
            Factory.Instance.MakeDamageText((int)damage, transform.position);
            texture = sprite2d.texture;
            sprite.material.SetTexture(Texture2DID, texture);
            sprite.material.SetFloat(HitID, 1);

            StartCoroutine(onHit());

            HP -= damage;
        }

    }

    IEnumerator onHit()
    {
        yield return new WaitForSeconds(0.1f);
        sprite.material.SetFloat(HitID, 0);
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
        Vector2 dropPosition = transform.position;
        float dropY = 0.4f; 

        foreach (var item in dropItems)
        {
            if ( item.dropRatio > Random.value )
            {
                Factory.Instance.MakeItems(item.code, item.dropCount, new Vector2(dropPosition.x, dropPosition.y + dropY));
                dropY += 0.4f; // ���� ������ ��� ��ġ�� ���� ���� �̵�

            }
        }
    }

    /// <summary>
    /// �׾����� ���� �� �޼���
    /// </summary>
    public void Die()
    {
        Debug.Log("�׾���.");
        IsLive = false;
        sprite.material.SetFloat(HitID, 0);

        this.gameObject.layer = 17;
        StopAllCoroutines();
        ItemDrop();
        rb.freezeRotation = false;
        col.isTrigger = false;



    }
}
