using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipable
{
    /// <summary>
    /// 아이템을 장착하는 함수
    /// </summary>
    /// <param name="slot">장착할 아이템이 들어있는 슬롯</param>
    void Equip(EquipmentSlot_Base slot);

    /// <summary>
    /// 아이템을 장착 해제하는 함수
    /// </summary>
    /// <param name="slot">장착 해제할 아이템이 들어있는 슬롯</param>
    void UnEquip(EquipmentSlot_Base[] slots);


}
