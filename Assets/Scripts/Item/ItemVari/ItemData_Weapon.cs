using System;
using Unity.VisualScripting;

using UnityEngine;

// ������ �� ������ ������ �����ϴ� ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Weapon Data", order = 1)]
public class ItemData_Weapon : ItemData, IWeapon
{
    public uint maxStackCount = 1;

    [Header("���� �⺻ ����")]
    public WeaponInfo Weaponinfo;
    [Header("���� ����Ʈ ����")]
    public EffectInfo EffectInfo;

    public void Equip(EquipmentSlot_Base slot)
    {
        slot.SlotItemData = this;

        WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
        Debug.Log($"{slot.SlotItemData}");
        if (weaponManager != null)
        {
            weaponManager.GetWeaponData(this);
            weaponManager.ActivateWeaponPrefab(this);
        }
    }
    public void UnEquip(EquipmentSlot_Base[] slots)
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
