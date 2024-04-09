using System;
using System.Collections;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    [Header("플레이어 스텟")]
    public float attackPower;                // 공격력
    public float Defense;               // 방어력
    public float attackSpeed;                // 공격속도
    public float MaxHp = 100.0f;        // 최대체력
    public float hp;                   // 현재체력
    public int criticalChance;          // 크리티컬
    public float damageTaken;           // 몬스터 받는 피해
    public int Level;                   // 레벨
    public float itemRange;              // 아이템을 흡수하는 범위

    /// <summary>
    /// 던그리드 음식 넣을시 넣을 변수
    /// </summary>
    //public int Hungrycurr;
    //public int HungryMax;

    public int gold;                    // 소지 금액

    /// <summary>
    /// 살았는지 죽었는지 확인하기 위한 프로퍼티
    /// </summary>
    public bool IsAlive => hp > 0;

    /// <summary>
    /// 무적시간
    /// </summary>
    private float invincibleTime = 2.0f;

    public Action OnDie;
    /// <summary>
    /// HP의 변경을 알리는 델리게이트
    /// </summary>
    public Action<float> onHealthChange { get; set; }

    SpriteRenderer spriteRenderer;
    Animator ani;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    private void Update()
    {
        pickupItem();
    }

    /// <summary>
    /// 체력 설정 프로퍼티
    /// </summary>
    public float CurrentHp
    {
        get => hp;

        set
        {
            if (IsAlive)
            {
                hp = Mathf.Clamp(value, 0, MaxHp);
                if (hp <= 0.0f)
                {
                    Die();
                }
                onHealthChange?.Invoke(hp / MaxHp);
            }
        }
    }

    private void Start()
    {
        hp = MaxHp;    // 게임 시작 시 체력을 최대로 설정
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        Debug.Log(hp);

        if (hp > 0) // 체력이 남아있을 때만 무적 모드를 활성화
        {
            StartCoroutine(InvinvibleMode());
        }
        else if (hp <= 0)
        {
            Die();
        }
    }

    public void TakeHeal(float heal)
    {
        if (CurrentHp > 0)
        {
            CurrentHp += heal;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("트리거발견");

        if ( collision.GetComponent<IAttack>() != null )
        { 
            IAttack attack = collision.GetComponent<IAttack>();

            TakeDamage(attack.AttackPower);
        }

        
    }

    private void pickupItem()
    {
        // 주변에 있는 Item 레이어의 컬라이더를 전부 찾기
        Collider2D[] itemColliders = Physics2D.OverlapCircleAll(transform.position, itemRange, LayerMask.GetMask("Item"));

        
        foreach (Collider2D collider in itemColliders)
        {
            ItemObject item = collider.GetComponent<ItemObject>();

            IConsume consume = item.ItemData as IConsume;
            if (consume == null)
            {
                item.itemDel();                              // 아이템 제거
            }
            else 
            {
                consume.Consume(); // 즉시발동
                item.itemDel();
            }
        }
    }

    /// <summary>
    /// 체력이 변경되었을때 호출되는 함수
    /// </summary>
    void Die()
    {
        // 캐릭터가 사망했을 때의 로직 처리
        Debug.Log("Player Died");
        ani.SetTrigger("Die");
        OnDie?.Invoke();
        //Player_ani.SetTrigger("Die");
    }



    /// <summary>
    /// 무적 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator InvinvibleMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Player_Invincible"); // 레이어를 무적 레이어로 변경

        float timeElapsed = 0.0f;
        while (timeElapsed < invincibleTime) // 2초동안 계속하기
        {
            timeElapsed += Time.deltaTime;
            float alpha = (Mathf.Cos(timeElapsed * 30.0f) + 1.0f) * 0.25f + 0.5f;   // 코사인 결과를 1 ~ 0.5 사이로 변경
            spriteRenderer.color = new Color(1, 1, 1, alpha);               // 알파에 지정(깜박거리게 된다.)
            yield return null;
        }

        // 2초가 지난후
        gameObject.layer = LayerMask.NameToLayer("Player"); // 레이어를 다시 플레이어로 되돌리기
        spriteRenderer.color = Color.white;                 // 알파값도 원상복구
    }

}