using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Accessory", menuName = "Scriptable Object/Item Accessory Data", order = 7)]

public class ItemData_Accessory : ItemData, IAccessory
{
    [Header("버프형 아이템 데이터")]
    public PlayerBuff playerBuff;
    

    public float BuffActive()
    {
        Player target = GameManager.Instance.Player;
        if (target != null)
        {
            target.PlayerStats.onAddBuff(playerBuff);


        }
        return playerBuff.buff_duration;
    }

    public void Equip(EquipmentSlot_Base slot)
    {
        slot.SlotItemData = this;
        BuffActive();
    }

    public void UnEquip(EquipmentSlot_Base[] slots)
    {
        for (int i =0; i < slots.Length; i++)
        {
            if (slots[i].SlotItemData == this)
            {
                slots[i].SlotItemData = null;
                break;
            }
        }
        Player target = GameManager.Instance.Player;
        if (target != null)
        {
            target.PlayerStats.buffs.Remove(playerBuff);
            target.PlayerStats.buffChanged();
        }
    }

 
}
