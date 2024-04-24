using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Buff", menuName = "Scriptable Object/Item Buff Data", order = 6)]
public class ItemData_Buff_Swift : ItemData, IBuff
{
    [Header("������ ������ ������")]
    public PlayerBuff playerBuff;
    

    /// <summary>
    /// ���� �ߵ�
    /// </summary>
    /// <param name="target"></param>
    /// <returns>���ӽð��� ��ȯ</returns>
    public float BuffActive()
    {
        Player target = GameManager.Instance.Player;
        if ( target != null)
        {
            target.PlayerStats.onAddBuff(playerBuff);


        }
        return playerBuff.buff_duration;
    }
}
