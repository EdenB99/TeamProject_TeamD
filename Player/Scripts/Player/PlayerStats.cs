using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("플레이어 스텟")]
    public float Damage;                // 공격력
    public float Defense;               // 방어력
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
    private bool IsAlive => _hp > 0;

    /// <summary>
    /// 체력 설정 프로퍼티
    /// </summary>
    public float CurrentHp
    {
        get => _hp;

        set
        {
            float clampedValue = Mathf.Clamp(value, 0, MaxHp); // 클램핑된 값을 임시 변수에 저장
            if (_hp != clampedValue) // 값이 변경되었는지 확인
            {
                _hp = clampedValue; // 변경된 값으로 Hp 업데이트
                if (IsAlive)
                {
                    // 플레이어가 살아있을 때의 처리
                }
                else
                {
                    // 플레이어가 사망했을 때의 처리
                   
                }
            }
        }
    }

    private void Start()
    {
        _hp = MaxHp;
    }

   
}