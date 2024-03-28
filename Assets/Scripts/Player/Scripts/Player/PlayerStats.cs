using System;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("플레이어 스텟")]

    /// <summary>
    /// 플레이어의 공격력
    /// </summary>
    public float attackPower;
    /// <summary>
    /// 플레이어의 방어력
    /// </summary>
    public float defense;               
    /// <summary>
    /// 플레이어의 최대 체력
    /// </summary>
    public float maxHp = 100.0f;        // 최대체력
    /// <summary>
    /// 플레이어의 현재 체력
    /// </summary>
    public float _hp;                   // 현재체력
    /// <summary>
    /// 플레이어의 크리티컬 확률
    /// </summary>
    public int criticalChance;          // 크리티컬
    /// <summary>
    /// ?
    /// </summary>
    public float damageTaken;           // 몬스터 받는 피해
    /// <summary>
    /// 플레이어의 레벨
    /// </summary>
    public int Level;                   // 레벨
    /// <summary>
    /// 플레이어의 속도
    /// </summary>
    public float attackSpeed;

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

    public Action OnDie;

    Animator ani;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    /// <summary>
    /// 체력 설정 프로퍼티
    /// </summary>
    public float CurrentHp
    {
        get => _hp;

        set
        {
            _hp = Mathf.Clamp(value, 0, maxHp);

            if(!IsAlive)
            {
                Die();
            }
        }
    }

    private void Start()
    {
        _hp = maxHp;    // 게임 시작 시 체력을 최대로 설정
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
        if(Input.GetKeyDown(KeyCode.K))
        {
            CurrentHp -= 10;
        }
    }
}