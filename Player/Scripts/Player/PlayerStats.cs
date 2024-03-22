using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("�÷��̾� ����")]
    public float Damage;                // ���ݷ�
    public float Defense;               // ����
    public float MaxHp = 100.0f;        // �ִ�ü��
    public float _hp;                   // ����ü��
    public int criticalChance;          // ũ��Ƽ��
    public float damageTaken;           // ���� �޴� ����
    public int Level;                   // ����


    /// <summary>
    /// ���׸��� ���� ������ ���� ����
    /// </summary>
    public int Hungrycurr;
    public int HungryMax;

    public int gold;                    // ���� �ݾ�


    /// <summary>
    /// ��Ҵ��� �׾����� Ȯ���ϱ� ���� ������Ƽ
    /// </summary>
    private bool IsAlive => _hp > 0;

    /// <summary>
    /// ü�� ���� ������Ƽ
    /// </summary>
    public float CurrentHp
    {
        get => _hp;

        set
        {
            float clampedValue = Mathf.Clamp(value, 0, MaxHp); // Ŭ���ε� ���� �ӽ� ������ ����
            if (_hp != clampedValue) // ���� ����Ǿ����� Ȯ��
            {
                _hp = clampedValue; // ����� ������ Hp ������Ʈ
                if (IsAlive)
                {
                    // �÷��̾ ������� ���� ó��
                }
                else
                {
                    // �÷��̾ ������� ���� ó��
                   
                }
            }
        }
    }

    private void Start()
    {
        _hp = MaxHp;
    }

   
}