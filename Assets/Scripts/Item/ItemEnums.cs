using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 코드(ID)
/// </summary>
public enum ItemCode : ushort
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
    /// 판매 전용 아이템 ( 다이아몬드 )
    /// </summary>
    Diamond,



    /// <summary>
    /// 무기 아이템 
    /// </summary>
    Sword,


    /// <summary>
    /// 장신구 아이템 
    /// </summary>
}

/// <summary>
/// 장비 가능한 아이템의 종류 => 아이템 종류로 변경 :hjun
/// </summary>
public enum ItemType : byte
{
    Weapon,         //무기
    Accessory,      //악세사리
    Consumable      //소모품
}
/// <summary>
/// 아이템 정렬 기준
/// </summary>
public enum ItemSortBy
{
    Code,       // 코드 기준
    Name,       // 이름 기준
    Price       // 가격 기준
}

public enum WeaponType
{
    Slash,
    Stab
}