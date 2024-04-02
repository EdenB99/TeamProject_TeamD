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
    public float _hp;                   // 현재체력
    public int criticalChance;          // 크리티컬
    public float damageTaken;           // 몬스터 받는 피해
    public int Level;                   // 레벨

    /// <summary>
    /// 던그리드 음식 넣을시 넣을 변수
    /// </summary>
    public int Hungrycurr;
    public int HungryMax;

    public int gold;                    // 소지 금액

    /// <summary>
    /// 살았는지 죽었는지 확인하기 위한 프로퍼티
    /// </summary>
    public bool IsAlive => _hp > 0;

    /// <summary>
    /// 무적시간
    /// </summary>
    private float invincibleTime = 2.0f;

    /// <summary>
    /// 무적상태 확인
    /// </summary>
    private bool isInvincible = false;

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

    /// <summary>
    /// 체력 설정 프로퍼티
    /// </summary>
    public float CurrentHp
    {
        get => _hp;

        set
        {
            if (IsAlive)
            {
                _hp = value;
                if (_hp <= 0.0f)
                {
                    Die();
                }
                _hp = Mathf.Clamp(value, 0, MaxHp);
                onHealthChange?.Invoke(_hp / MaxHp);
            }
        }
    }

    private void Start()
    {
        _hp = MaxHp;    // 게임 시작 시 체력을 최대로 설정
    }



    public void TakeDamage(float damage)
    {
        if (!isInvincible) // 플레이어가 무적 상태가 아닐 때만 피해를 입힘
        {
            _hp -= damage;
            Debug.Log(_hp);

            if (_hp > 0) // 체력이 남아있을 때만 무적 모드를 활성화
            {
                StartCoroutine(InvinvibleMode());
            }
            else if (_hp <= 0)
            {
                Die();
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
    /// 플레이어 죽음 테스트
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CurrentHp -= 10;
        }
    }


    /// <summary>
    /// 무적 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator InvinvibleMode()
    {
        isInvincible = true; // 무적 상태 시작
        gameObject.layer = LayerMask.NameToLayer("Invincible"); // 레이어를 무적 레이어로 변경

        float timeElapsed = 0.0f;
        while (timeElapsed < invincibleTime) // 2초동안 계속하기
        {
            timeElapsed += Time.deltaTime;

            float alpha = (Mathf.Cos(timeElapsed * 30.0f) + 1.0f) * 0.5f;   // 코사인 결과를 1 ~ 0 사이로 변경
            spriteRenderer.color = new Color(1, 1, 1, alpha);               // 알파에 지정(깜박거리게 된다.)

            yield return null;
        }

        // 2초가 지난후
        gameObject.layer = LayerMask.NameToLayer("Player"); // 레이어를 다시 플레이어로 되돌리기
        spriteRenderer.color = Color.white;                 // 알파값도 원상복구
        isInvincible = false; // 무적 상태 종료
    }

}