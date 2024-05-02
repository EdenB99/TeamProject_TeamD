using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class PatternEnemyBase : MonoBehaviour, IEnemy
{
    //������Ʈ �ҷ�����
    Rigidbody2D rb;
    protected Animator animator;
    public SpriteRenderer sprite;
    Sprite sprite2d;
    protected Texture2D texture;

    protected readonly int Texture2DID = Shader.PropertyToID("_Texture2D");
    protected readonly int FadeID = Shader.PropertyToID("_Fade");
    protected readonly int HitID = Shader.PropertyToID("_Hit");
    float fade = 0.0f;

    /// <summary>
    /// �÷��̾� �ҷ�����
    /// </summary>
    protected Player player;

    /// <summary>
    /// �÷��̾� ��ġ Ÿ����
    /// </summary>
    protected Vector2 playerPos;

    /// <summary>
    /// �÷��̾� ���� ���� ( ������ �ö��̴��� �ƴ� �ڵ�� �÷��̾ Ž���Ѵ� )
    /// </summary>
    public float sightRange = 1.0f;

    protected enum BossState
    {
        Wait,       // ���
        Chase,      // �÷��̾� ����
        Attack,     // ���� ����
        SpAttack,   // Ư�� ����
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
                        State_Wait();
                        stateUpdate = Update_Wait; break;

                    case BossState.Chase:
                        State_Chase();
                        stateUpdate = Update_Chase; break;

                    case BossState.Attack:
                        State_Attack();
                        stateUpdate = Update_Attack; break;

                    case BossState.SpAttack:
                        State_SpAttack();
                        stateUpdate = Update_Attack; break;

                    case BossState.Dead:
                        Die();
                        stateUpdate = Update_Attack; break;
                }
            }
        }
    }

    protected virtual void State_Wait()
    {

    }

    protected virtual void State_Chase()
    {

    }

    protected virtual void State_Attack() // �ܼ� ������ �������
    {

    }

    protected virtual void State_SpAttack()
    {

    }


    protected float hp = 100.0f;
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            hp = Mathf.Max(hp, 0);

            Debug.Log(hp);

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
    /// ���� ����
    /// </summary>
    bool IsLive = true;

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
            if (checkLR != value && State != BossState.Attack && State != BossState.SpAttack ) // ���� ���� �Ǿ���, �������� �ƴ϶��
            {
                checkLR = value;
                gameObject.transform.localScale = new Vector3(1.0f * checkLR, 1.0f, 1.0f); // �� ������� �����ؾ�, �ڽ� �ö��̴��� ����.
            }

        }
    }

   
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        InitializePatterns();

        stateUpdate = Update_Wait;

        // �� ���׸��� ��������
        sprite.material = GameManager.Instance.Mobmaterial; // ���׸����� �����´�.
        sprite2d = sprite.sprite;                           // ��������Ʈ�� ��������Ʈ�� , 
        texture = sprite2d.texture;                         // �ؽ��ķ� ��ȯ
        sprite.material.SetTexture(Texture2DID, texture);   // ��������Ʈ�� ���׸����� ���� ��������Ʈ�� ��ȯ
    }

    protected virtual void Start()
    {
        HP = MaxHP;
        player = GameManager.Instance.Player;   
    }

    protected virtual void Update()
    {
        if (IsLive)
        {
            // �÷��̾��� ��ġ�� �޴´�.
            playerPos = player.transform.position;
            // �÷��̾��� ��ġ������ �¿� 
            if (playerPos.x < rb.position.x) CheckLR = 1;
            else CheckLR = -1;

            stateUpdate();
        }
        else // �׾��ٸ�,
        {
            fade += Time.deltaTime * 0.25f; // ������ ������ �����
            sprite.material.SetFloat(FadeID, 1 - fade);

            if (fade > 1)
            {
                Destroy(this.gameObject); // 4���� ����
            }
        }

    }

    

    protected virtual void Update_Wait()
    {


    }

    protected virtual void Update_Chase()
    {

    }

    protected virtual void Update_Attack()
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
    protected bool playerCheck()
    {
        // ���� ����
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, sightRange, LayerMask.GetMask("Player"));

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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<IAttack>() != null )            // IAttack�� ������ �ְ�, �������°� �ƴҶ���
        {
            IAttack attack = collision.GetComponent<IAttack>();     // ������Ʈ �����ͼ�

            TakeDamage(attack.AttackPower);                         // �ش� ������Ʈ�� AttackPower��ŭ ���ظ� ����.
        }
    }


    /// <summary>
    /// ���ظ� �޾����� ������ �Լ� ����
    /// </summary>
    /// <param name="Damage">�÷��̾�� ���� ����</param>
    public void TakeDamage(float damage)
    {
        if (IsLive)
        {
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

    /// <summary>
    /// �׾����� ���� �� �޼���
    /// </summary>
    public virtual void Die()
    {
        StopAllCoroutines();

        IsLive = false;
        sprite.material.SetFloat(HitID, 0);
    }


}
