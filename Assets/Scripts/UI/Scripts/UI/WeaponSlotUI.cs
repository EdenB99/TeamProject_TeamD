using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotUI : EquipmentSlot_Base
{
    private bool onhand = false;
    public bool Onhand
    {
        get => onhand;
        set
        {
            onhand = value;
            SetEquipmentText(onhand);
        }
    }
}