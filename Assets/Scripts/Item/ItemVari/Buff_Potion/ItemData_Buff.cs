using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Buff", menuName = "Scriptable Object/Item Buff Data", order = 6)]
public class ItemData_Buff_Swift : ItemData, IBuff
{
    [Header("버프형 아이템 데이터")]
    public PlayerBuff playerBuff;
    

    /// <summary>
    /// 사용시 발동
    /// </summary>
    /// <param name="target"></param>
    /// <returns>지속시간을 반환</returns>
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
