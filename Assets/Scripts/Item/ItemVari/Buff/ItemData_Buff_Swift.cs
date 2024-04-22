using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Buff", menuName = "Scriptable Object/Item Buff Data", order = 6)]
public class ItemData_Buff_Swift : ItemData, IBuff
{
    [Header("버프형 아이템 데이터")]
    public int speedUp;
    public float duration;

    /// <summary>
    /// 사용시 발동
    /// </summary>
    /// <param name="target"></param>
    /// <returns>지속시간</returns>
    public float BuffActive()
    {
        Player target = GameManager.Instance.Player;
        if ( target != null)
        {
            target.PlayerStats.speed += speedUp;

        }
        return duration;
    }

    /// <summary>
    /// 버프가 끝날때 발동 ( 원상복구 )
    /// </summary>
    /// <param name="target"></param>
    public void BuffEnd()
    {
        Debug.Log("버프취소");
        Player target = GameManager.Instance.Player;
        if (target != null)
        {
            target.PlayerStats.speed -= speedUp;

        }
    }
}
