using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossBase_ : MonoBehaviour, IEnemy
{
    //������Ʈ �ҷ�����
    Rigidbody2D rb;
    public SpriteRenderer sprite;

    /// <summary>
    /// �÷��̾� �ҷ�����
    /// </summary>
    protected Player player;

    /// <summary>
    /// �÷��̾� ��ġ Ÿ����
    /// </summary>
    Vector2 playerPos;

    /// <summary>
    /// �÷��̾� ���� ���� ( ������ �ö��̴��� �ƴ� �ڵ�� �÷��̾ Ž���Ѵ� )
    /// </summary>
    public float sightRange = 1.0f;

    protected enum BossState
    {
        Wait,       // ���
        Chase,      // �÷��̾� ����
        Attack,     // ���� ����
        Dead        // ����
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    BossState state = BossState.Wait;

    /// <summary>
    /// ���� ����
    /// </summary>
    Action stateUpdate;

    /// <summary>
    /// ���� ������Ƽ
    /// </summary>
    protected BossState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                switch (state)  // ���¿� ������ �� �� �ϵ� ó��
                {
                    case BossState.Wait:
                        stateUpdate = Update_Wait; break;

                    case BossState.Chase:
                        stateUpdate = Update_Chase; break;

                    case BossState.Attack:
                        stateUpdate = Update_Attack; break;
                    case BossState.Dead:
                        Die();
                        stateUpdate = Update_Attack; break;
                }
            }
        }
    }



    protected float hp = 100.0f;
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


    public float maxHP = 100.0f;
    public float MaxHP => maxHP;

    /// <summary>
    /// �� ��ü�� ������ ( �ε����� ��츸 )
    /// </summary>
    public uint Attackpower = 1;
    public uint AttackPower => Attackpower;
    public Action onDie { get; set; }

    /// <summary>
    /// ���� ���ð�
    /// </summary>
    protected float waitTime;

    /// <summary>
    /// �׽�Ʈ�� uint
    /// </summary>
    public uint TestPattern = 0;

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
                // ���� ��ȯ ( flip ���� ���� �ʿ��ϸ�, ��������Ʈ�� �⺻�� �����̳� �����̳Ŀ� ���� �ٲ��־����.)
                gameObject.transform.localScale = new Vector3(1.0f * checkLR, 1.0f, 1.0f);
            }

        }
    }

   
    protected virtual void Awake()
    {
        //animator = GetComponent<Animator>(); ���Ŀ� �ִϸ����Ͱ� �ݵ�� ��
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        InitializePatterns();
    }

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;   
    }

    protected virtual void Update()
    {
        // �÷��̾��� ��ġ�� �޴´�.
        playerPos = player.transform.position;

        // ���¿� ���� ����
        stateUpdate();
    }

    void Update_Wait()
    {
        // �÷��̾��� ��ġ�� �޴´�.
        playerPos = player.transform.position;

        // �÷��̾��� ��ġ������ �¿� 
        if (playerPos.x < rb.position.x) CheckLR = 1;
        else CheckLR = -1;
    }

    void Update_Chase()
    {

    }

    void Update_Attack()
    {

    }

    /// <summary>
    /// ������ ���� ��ųʸ�
    /// </summary>
    protected Dictionary<uint, Func<IEnumerator>> patternActions;

    /// <summary>
    /// ��ųʸ� ���� ���� / 1 = ȣ���� ��ȣ / �� IEnumerator = ��ȣ�� ���� ������ �ڷ�ƾ
    /// </summary>
    protected virtual void InitializePatterns()
    {
        patternActions = new Dictionary<uint, Func<IEnumerator>>()
    {
            { 1, BossPattern_1 }

        // �ٸ� ���ϵ鵵 �̿� ���� �ʱ�ȭ
    };
    }

    /// <summary>
    /// ������ �ൿ�� ����
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator AwakeAction()
    {
        yield return null;
    }

    /// <summary>
    /// ������ �ൿ�� ������ �����ϴ� �ڷ�ƾ, ������ ������ ��ġ�� �� �ڷ�ƾ�� �ٽ� ȣ���Ѵ�.
    /// </summary>
    /// <param name="i">Ư�� ������ �����Ű�� ���� ����</param>
    /// <returns></returns>
    protected virtual IEnumerator bossActionSelect(uint pattern = 0)
    {
        pattern = TestPattern;

        // i�� ���� �־��ٸ�, �ش� ������ �����ϸ�, ���� �ʾҴٸ� ������ ������ �����Ѵ�.
        if (pattern == 0) pattern = (uint)Random.Range(1, patternActions.Count + 1); // ���� ������ ����

        waitPattern();

        yield return new WaitForSeconds(waitTime); // ������ �����ϱ� ��, ���� �ð� (�ִϸ��̼� ����, ��Ÿ�� �� )

        
        // ���� ����
        if (patternActions.TryGetValue(pattern, out var action))
        {
            StartCoroutine(action());
        }
    }

    /// <summary>
    /// ���� ���ð����� ������ �̰��� ����
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator waitPattern()
    {
        yield return new WaitForSeconds(1.0f);
    }


    /// <summary>
    /// ���� 1 : ���� ����
    /// 1�� ������ �������̵��Ͽ� ��� , �������� �ڽĿ��� ���� ����
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator BossPattern_1()
    {
        // ����
        yield return new WaitForSeconds(1.0f);
        //����
        yield return new WaitForSeconds(1.0f);
        //����
        StartCoroutine(bossActionSelect());
    }


    /// <summary>
    /// �÷��̾ Ž���ϴ� ���� �����ϴ� �޼��� SightRange �ȿ� ������ �÷��̾ �ִ°�.
    /// </summary>
    /// <returns>���� true = �÷��̾ �������� �ִ�.</returns>
    private bool playerCheck()
    {
        // ���� ����
        Collider[] colliders = Physics.OverlapSphere(transform.position, sightRange, LayerMask.GetMask("Player"));

        // �÷��̾ �ִٸ�
        if (colliders.Length > 0)
        {
            return true;
        }
        return false;
    }

    public void Attack()
    {
        // �÷��̾�� �����ִ°Ͱ� ���õ� �ൿ ����
    }

    /// <summary>
    /// ���ظ� �޾����� ������ �Լ� ����
    /// </summary>
    /// <param name="Damage">�÷��̾�� ���� ����</param>
    public void Damaged(float damage)
    {
        HP -= damage;
    }

    /// <summary>
    /// �׾����� ���� �� �޼���
    /// </summary>
    public void Die()
    {
        Debug.Log("�׾���.");
        StopAllCoroutines();
    }


}
