using System;
using Unity.VisualScripting;

using UnityEngine;

// 아이템 한 종류의 정보를 저장하는 스크립터블 오브젝트
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Weapon Data", order = 1)]
public class ItemData_Weapon : ItemData, IWeapon
{
    public uint maxStackCount = 1;

    [Header("무기 기본 정보")]
    public WeaponInfo Weaponinfo;
    [Header("무기 이펙트 정보")]
    public EffectInfo EffectInfo;

    virtual public void Equip(EquipmentSlot_Base slot)
    {
        slot.SlotItemData = this;

        WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.GetWeaponData(this);
        }
    }
    virtual public void UnEquip(EquipmentSlot_Base[] slots)
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
        }
    }


    public int GetWeaponDamage()
    {
        return (int)Weaponinfo.weaponDamage;
    }
    
    public float GetWeaponSpeed()
    {
        return (float)Weaponinfo.attackSpeed;
    }

    public EffectInfo GetEffectInfo()
    {
        return EffectInfo;
    }

    public float GetEffectSpeed()
    {
        return (float)EffectInfo.effectSpeed;
    }

    public float GetAttackSpeed()
    {
        return (float)Weaponinfo.attackSpeed;
    }

    public WeaponInfo GetWeaponInfo()
    {
        return Weaponinfo;
    }
}

