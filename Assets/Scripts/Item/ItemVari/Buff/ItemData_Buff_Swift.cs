using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Buff", menuName = "Scriptable Object/Item Buff Data", order = 6)]
public class ItemData_Buff_Swift : ItemData, IBuff<Player>
{
    [Header("������ ������ ������")]
    public int speedUp;
    public float duration;

    /// <summary>
    /// ���� �ߵ�
    /// </summary>
    /// <param name="target"></param>
    /// <returns>���ӽð�</returns>
    public float BuffActive(Player target)
    {
        if ( target != null)
        {
            target.PlayerStats.speed += speedUp;

        }
        return duration;
    }

    /// <summary>
    /// ������ ������ �ߵ� ( ���󺹱� )
    /// </summary>
    /// <param name="target"></param>
    public void BuffEnd(Player target)
    {
        if (target != null)
        {
            target.PlayerStats.speed -= speedUp;

        }
    }
}
