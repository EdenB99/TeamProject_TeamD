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

    public void Equip(EquipmentSlot_Base slot)
    {
        slot.SlotItemData = this;

        WeaponManager weaponmanager = FindObjectOfType<WeaponManager>();
        Debug.Log($"{slot.SlotItemData}");
        if (weaponmanager != null)
        {
            weaponmanager.GetWeaponData(this);
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

        WeaponManager weaponmanager = FindObjectOfType<WeaponManager>();
        if (weaponmanager != null)
        {
            weaponmanager.DeleteWeaponData(this);
        }
    }

    public int GetWeaponDamage()
    {
        return (int)Weaponinfo.weaponDamage;
    }
    
    public int GetWeaponSpeed()
    {
        return (int)Weaponinfo.attackSpeed;
    }

    public EffectInfo GetEffectInfo()
    {
        return EffectInfo;
    }

}
//public int GetattackSpeed()
//{
//    return (int)Weaponinfo.attackSpeed;
//}
