using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float BaseAttackPower;                // 공격력
    public float BaseDefense;               // 방어력
    public float BaseAttackSpeed;                // 공격속도
    public float BaseMaxHp = 100.0f;        // 최대체력
    public float BaseCriticalChance;          // 크리티컬
    public float BaseItemRange;              // 아이템을 흡수하는 범위
    public float BaseSpeed;

    // 플레이어가 적용받고 있는 스탯
    private float attackPower;                // 공격력
    private float Defense;               // 방어력
    private float attackSpeed;                // 공격속도
    private float maxHp = 100.0f;        // 최대체력
    private float criticalChance;          // 크리티컬
    private float itemRange;              // 아이템을 흡수하는 범위
    private float speed;

    public float hp;                   // 현재체력

    public List<PlayerBuff> buffs;


    [Header("플레이어 부활")]
    public Transform respawnPoint; // 플레이어가 부활할 위치
    public bool isDead = false;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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

        RefreshStats();
    }

    private void Update()
    {
        pickupItem();

        if (Input.GetKeyDown(KeyCode.K))
        {
            Die();
        }

        if (isDead && Input.GetKeyDown(KeyCode.R))
        {
            Respawn();
            Destroy(this.gameObject);
        }
    }

    SpriteRenderer spriteRenderer;
    Animator ani;

    // 스탯 프로퍼티 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    public float Speed { get { return speed; } set { speed = value; } }
    public float MaxHp { get { return maxHp; } set { maxHp = value; } }
    public float AttackPower { get { return attackPower; } set { attackPower = value; } }

    // 피격 , HP 관련 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    public bool invincible;             // 무적상태

    /// <summary>
    /// HP의 변경을 알리는 델리게이트
    /// </summary>
    public Action<float, float> onHealthChange { get; set; } //델리게이트의 인자값을 float 2개로 변경

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
                onHealthChange?.Invoke(hp, MaxHp); //델리게이트로 hp와 Maxhp 전달
            }
        }
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

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.GetComponent<IAttack>() != null && !invincible)            // IAttack을 가지고 있고, 무적상태가 아닐때만
        {
                if(!collision.CompareTag("PlayerAttack"))
            {
                Debug.Log("트리거발견");

                IAttack attack = collision.GetComponent<IAttack>();     // 컴포넌트 가져와서

                TakeDamage(attack.AttackPower);                         // 해당 컴포넌트의 AttackPower만큼 피해를 받음.
            } 

        }
    }

    void Respawn()
    {
        Debug.Log("플레이어 부활");
        SceneManager.LoadScene("Town", LoadSceneMode.Single);
        StartCoroutine(RespawnPlayer());
    }

    private IEnumerator RespawnPlayer()
    {
        // 씬 로드가 완료될 때까지 대기
        yield return new WaitUntil(() => SceneManager.GetActiveScene().name == "Town");

        // 지정된 부활 위치로 플레이어 이동
        transform.position = respawnPoint.position;
    }

    /// <summary>
    /// 체력이 변경되었을때 호출되는 함수
    /// </summary>
    void Die()
    {
        // 캐릭터가 사망했을 때의 로직 처리
        Debug.Log("플레이어가 죽었다.");
        ani.SetTrigger("Die");
        isDead = true;
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

    // 아이템 관련 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    /// <summary>
    /// 아이템이 플레이어에게 이동하는 속도
    /// </summary>
    public float PickSpeed;

    /// <summary>
    /// 아이템을 획득하는 반경
    /// </summary>
    public float pickupRange = 1.0f;

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

    public void TakeHeal(float heal)
    {
        if (CurrentHp > 0)
        {
            CurrentHp += heal;
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
        else {
            InventoryUI invenUI = GameManager.Instance.InventoryUI;
            invenUI.getItem(item.ItemData.code);
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

        attackPower = BaseAttackPower + temp_attackPower;
        Defense = BaseDefense + temp_Defense;
        MaxHp = BaseMaxHp + temp_hp;
        criticalChance = BaseCriticalChance + temp_criticalChance;
        Speed = BaseSpeed + temp_speed;
        itemRange = BaseItemRange;
    }

    void RefreshStats()
    {
        attackPower = BaseAttackPower;
        Defense = BaseDefense;
        MaxHp = BaseMaxHp;
        criticalChance = BaseCriticalChance;
        Speed = BaseSpeed;
        itemRange = BaseItemRange;
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







}