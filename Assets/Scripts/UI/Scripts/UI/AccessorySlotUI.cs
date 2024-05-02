using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessorySlotUI : EquipmentSlot_Base
{
    public override void SetSlotItemChange(ItemData slotItemData)
    {
        base.SetSlotItemChange(slotItemData);
        if (slotItemData != null)
        {
            SetEquipmentText(true);
        } else
        {
            SetEquipmentText(false);
        }
    }
}
