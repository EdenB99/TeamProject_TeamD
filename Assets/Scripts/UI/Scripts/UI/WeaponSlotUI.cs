using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotUI : EquipmentSlot_Base
{
    private bool onhand;
    public bool Onhand
    {
        get => onhand;
        set
        {
            onhand = value;
            OnHandChange(onhand);
            SetEquipmentText(onhand);
        }
    }
    Action<bool> OnHandChange;

    //1. 현재 슬롯이 손에 들려있는지


}