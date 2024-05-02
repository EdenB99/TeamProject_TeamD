using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Accessory", menuName = "Scriptable Object/Item Accessory Data", order = 7)]

public class ItemData_Accessory : ItemData, IAccessory
{
    [HideInInspector]
    public EquipmentSlot_Base AccessorySlot;
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
        AccessorySlot = slot;
        slot.SlotItemData = this;
        BuffActive();
    }

    public void UnEquip()
    {
        AccessorySlot.SlotItemData = null;
        AccessorySlot = null;
        Player target = GameManager.Instance.Player;
        if (target != null)
        {
            target.PlayerStats.buffs.Remove(playerBuff);
        }
    }

 
}
