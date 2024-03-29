using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 코드(ID)
/// </summary>
public enum ItemCode : byte
{
    /// <summary>
    /// 소비 아이템 ( 즉발 )
    /// </summary>
    Apple = 0,





    /// <summary>
    /// 소비 아이템 ( 인벤토리 )
    /// </summary>
    HealingPotion_A,


    /// <summary>
    /// 판매 전용 아이템 
    /// </summary>
    Diamond,



    /// <summary>
    /// 무기 아이템 
    /// </summary>
    Sword,


    /// <summary>
    /// 악세서리 아이템 
    /// </summary>
}

/// <summary>
/// 장비 가능한 아이템의 종류
/// </summary>
public enum EquipType : byte
{
    Weapon,
    Accessory
}