using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// 아이템 한 종류의 정보를 저장하는 스크립터블 오브젝트
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Skill Weapon Data", order = 9)]
public class ItemData_Weapon_HasSkill : ItemData_Weapon, IWeapon ,IActivatable
{
    //QuickSlotUI, IngameUI, WeaponManger를 활용해서
    //아이템을 착용할 시 QuickslotUI에 아이템 아이콘을 설정하고
    //키를 입력하면 해당 퀵슬롯의 스킬을 사용하여 쿨타임을 적용하고
    //weaponmanager를 통해 해당 무기의 스킬을 호출하게 한다.
    //ItemActive를 통해 QuickSlot에서 호출한다.
    public  GameObject skill;

    public override void Equip(EquipmentSlot_Base slot)
    {
        slot.SlotItemData = this;

        WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
        if (weaponManager != null)
        {
            if (weaponManager.weaponsData[weaponManager.CurrentWeaponIndex] == null)
            {
                weaponManager.ActivateWeaponPrefab(this);
            }
            weaponManager.GetWeaponData(this);
        }
    }

    public bool ItemActive(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }

    public override void UnEquip(EquipmentSlot_Base[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].SlotItemData == this)
            {
                slots[i].SlotItemData = null;
                break;
            }
        }

        WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.DeleteWeaponData(this);
            weaponManager.DestroyCurrentWeapon();
        }
    }
}
