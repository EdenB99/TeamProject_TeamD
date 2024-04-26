using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어에 대한 버프 구조체 
/// </summary>
[System.Serializable]
public struct PlayerBuff
{
    public ItemCode code;
    public float buff_attackPower;
    public float buff_Defense;
    public float buff_hp;
    public float buff_criticalChance;
    public float buff_speed;
    /// <summary>
    /// -1 일경우 영구지속 ( 해제 전까지 )
    /// </summary>
    public float buff_duration;
}


public class PlayerStats : MonoBehaviour
{
    [Header("플레이어의 기본 스텟")]
    public float attackPower;                // 공격력
    public float Defense;               // 방어력
    public float attackSpeed;                // 공격속도
    public float MaxHp = 100.0f;        // 최대체력
    public float hp;                   // 현재체력
    public float criticalChance;          // 크리티컬
    public float damageTaken;           // 몬스터 받는 피해
    public int Level;                   // 레벨
    public float itemRange;              // 아이템을 흡수하는 범위
    public float speed;

    // 플레이어가 적용받고 있는 스탯
    private float _attackPower;                // 공격력
    private float _Defense;               // 방어력
    private float _attackSpeed;                // 공격속도
    private float _MaxHp = 100.0f;        // 최대체력
    private float _hp;                   // 현재체력
    private float _criticalChance;          // 크리티컬
    private float _damageTaken;           // 몬스터 받는 피해
    private float _itemRange;              // 아이템을 흡수하는 범위
    private float _speed;

    public bool invincible;             // 무적상태


    public List<PlayerBuff> buffs;

    /// <summary>
    /// 아이템이 플레이어에게 이동하는 속도
    /// </summary>
    public float PickSpeed;

    /// <summary>
    /// 아이템이 감지할 반경
    /// </summary>
    public float checkRadius;

    /// <summary>
    /// 아이템을 끌어당길 반경
    /// </summary>
    public float pickupRange = 1.0f;

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
    public Action<float, float> onHealthChange { get; set; } //델리게이트의 인자값을 float 2개로 변경

    /// <summary>
    /// 플레이어의 인벤토리
    /// </summary>
    Inventory inven;
    public Inventory Inventory => inven;
    /// <summary>
    /// 플레이어의 인게임 UI
    /// </summary>
    IngameUI ingameUI;
    public IngameUI IngameUI => ingameUI;

    GameObject blade;
    //WeaponBase weapon;
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
                onHealthChange?.Invoke(hp ,MaxHp); //델리게이트로 hp와 Maxhp 전달
            }
        }
    }

    private void Start()
    {
        hp = MaxHp;    // 게임 시작 시 체력을 최대로 설정
        inven = new Inventory();
        if (GameManager.Instance.InventoryUI != null)
        {
            GameManager.Instance.InventoryUI.InitializeInventory(Inventory);    // 인벤토리와 인벤토리 UI연결
        }
        ingameUI = GameManager.Instance.IngameUI; //ingameUI 연결
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
        Debug.Log(hp);

        if (hp > 0) // 체력이 남아있을 때만 무적 모드를 활성화
        {
            StartCoroutine(InvincibleMode());
        }
        else if (hp <= 0)
        {
            Die();
            blade.SetActive(false);
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
        

        if (collision.GetComponent<IAttack>() != null && !invincible)            // IAttack을 가지고 있고, 무적상태가 아닐때만
        {
            Debug.Log("트리거발견");

            IAttack attack = collision.GetComponent<IAttack>();     // 컴포넌트 가져와서

            TakeDamage(attack.AttackPower);                         // 해당 컴포넌트의 AttackPower만큼 피해를 받음.
        }
    }

    private void pickupItem()
    {
        Collider2D[] itemColliders = Physics2D.OverlapCircleAll(transform.position, itemRange, LayerMask.GetMask("Item"));

        foreach (Collider2D collider in itemColliders)
        {
            ItemObject item = collider.GetComponent<ItemObject>();
            if (item != null)
            {
                // 플레이어와 아이템 간의 거리를 계산
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // 아이템이 끌어당겨질 범위 안에 있으면 플레이어 쪽으로 이동
                if (distance > pickupRange && distance <= itemRange)
                {
                    collider.transform.position = Vector3.MoveTowards(collider.transform.position,
                        transform.position, PickSpeed * Time.deltaTime);
                }
                // 아이템이 획득 범위 안에 들어오면 아이템을 처리
                else if (distance <= pickupRange)
                {
                    processItem(item);
                }
            }
        }
    }

    private void processItem(ItemObject item)
    {
        IConsume consume = item.ItemData as IConsume;
        if (consume != null)
        {
            // 즉시 사용 아이템을 처리
            consume.Consume();
            item.itemDel(); // 아이템을 제거
        }
        else if (Inventory.AddItem(item.ItemData.code)) // 일반 아이템을 인벤토리에 추가
        {
            item.itemDel(); 
        }
    }

    /// <summary>
    /// 버프를 추가하는 함수
    /// </summary>
    /// <param name="buff"></param>
    public void onAddBuff(PlayerBuff buff)
    {
        bool temp = true; // 버프의 실행여부

        if ( buff.buff_duration != -1 )             // 지속시간이 있는 ( 악세사리가 아닌 ) 버프라면,
        {
            foreach ( PlayerBuff inbuff in buffs)   // 현재 지속중인 버프를 찾는다.
            {
                if ( inbuff.code == buff.code )     // 같은 버프가 있다면
                {
                    temp = false;                   // 버프의 실행여부를 false로 한다.
                    break;
                }
            }

            if (temp == true)
            {
                StartCoroutine(Corutine_buffEnd(buff));
                buffs.Add(buff); // 버프를 추가한다.
                buffChanged();
            }

        }
        else                                        // 지속시간이 없는 ( 악세사리 ) 버프라면,
        {
            buffs.Add(buff); // 버프를 추가한다.
            buffChanged();
        }

    }

    public void buffChanged()
    {
        float temp_attackPower = 0;
        float temp_Defense = 0;
        float temp_hp = 0;
        float temp_criticalChance = 0;
        float temp_speed = 0;

        foreach (var inbuff in buffs)
        {
            temp_attackPower += inbuff.buff_attackPower;
            temp_Defense += inbuff.buff_Defense;
            temp_hp += inbuff.buff_hp;
            temp_criticalChance += inbuff.buff_criticalChance;
            temp_speed += inbuff.buff_speed;
        }

        _attackPower = attackPower + temp_attackPower;
        _Defense = Defense + temp_Defense;
        _MaxHp = MaxHp + temp_hp;
        _criticalChance = criticalChance + temp_criticalChance;
        _speed = speed + temp_speed;   
    }

    IEnumerator Corutine_buffEnd(PlayerBuff buff)
    {
        if ( buff.buff_duration > 0 )
        {
            yield return new WaitForSeconds(buff.buff_duration);
        }
        buffs.Remove(buff);
        buffChanged();

    }

    /// <summary>
    /// 체력이 변경되었을때 호출되는 함수
    /// </summary>
    void Die()
    {
        // 캐릭터가 사망했을 때의 로직 처리
        Debug.Log("플레이어가 죽었다.");
        ani.SetTrigger("Die");
        OnDie?.Invoke();
        //Player_ani.SetTrigger("Die");
    }

    /// <summary>
    /// 무적 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator InvincibleMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Player_Invincible"); // 레이어를 무적 레이어로 변경
        invincible = true;

        float timeElapsed = 0.0f;
        while (timeElapsed < invincibleTime) // 2초동안 계속하기
        {
            timeElapsed += Time.deltaTime;
            float alpha = (Mathf.Cos(timeElapsed * 30.0f) + 1.0f) * 0.25f + 0.5f;   // 코사인 결과를 1 ~ 0.5 사이로 변경
            spriteRenderer.color = new Color(1, 1, 1, alpha);               // 알파에 지정(깜박거리게 된다.)
            yield return null;
        }

        // 2초가 지난후
        invincible = false;
        gameObject.layer = LayerMask.NameToLayer("Player"); // 레이어를 다시 플레이어로 되돌리기
        spriteRenderer.color = Color.white;                 // 알파값도 원상복구
    }

    void OnDrawGizmosSelected()
    {
        // 에디터에서 감지 범위와 획득 범위를 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, itemRange);
    }

}