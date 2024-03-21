using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("플레이어 스탯")]
    public float Damage;        // 공격력
    public float Defense;       // 방어력
    public float MaxHp;         // 최대체력
    public float Hp;            // 체력
    public float DashPower;     // 대쉬파워 (대쉬를 할때 데미지를 줌)  
    private float CurrentHp;  // Hp량


    public float Satiety;       // 포만감


    public int gold;            // 소지골드
    public int criticalChance;  // 크리티컬

    

    private bool isTakingDamage; // 데미지피격 확인용


    // 현재 hp 프로퍼티
    public float MyCurrentHp
    {
        get
        {
            return Hp;
        }

        set
        {
            if(value > MaxHp)
            {
                Hp = MaxHp;
            }
            else if(value < 0)
            {
                Hp = 0;
            }
            else
            {
                Hp = value;
            }
            CurrentHp = Hp / MaxHp;
            // Hp UI가 들어올경우 처리할예정
        }
    }

    private void Awake()
    {
        MaxHp = 100;
        Hp = MaxHp;
    }




    public void Update()
    {
        if(Hp <= 0)
        {
            // Hp UI불러오기
        }
    }



    // 데미지가 플레이어한테 들어온다면 
    public void TakeDamge(int damage)
    {
        if(isTakingDamage)
        {
            Hp -= damage;
            // 코루틴 무적 생각하기

        }
    }

}
