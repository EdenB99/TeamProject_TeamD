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

    [HideInInspector]
    public EquipmentSlot_Base WeaponSlot;

    public void Equip(EquipmentSlot_Base slot)
    {
        if (WeaponSlot != null)
        {
            UnEquip();
        }

        WeaponManager weaponBase = FindObjectOfType<WeaponManager>();
        Debug.Log($"{weaponBase}");
        if (weaponBase != null)
        {
            weaponBase.getWeaponData(this);
        }

        WeaponSlot = slot;
        WeaponSlot.SlotItemData = this;
    }

    public void UnEquip()
    {
        WeaponManager weaponBase = FindObjectOfType<WeaponManager>();
        Debug.Log($"{weaponBase}");
        if (weaponBase != null)
        {

            weaponBase.deleteWeaponData(this);
        }

        WeaponSlot.SlotItemData = null;
        WeaponSlot = null;
    }

    public int GetWeaponDamage()
    {
        return (int)Weaponinfo.weaponDamage;
    }

    public EffectInfo GetEffectInfo()
    {
        return EffectInfo;
    }

    public void UnEquip(EquipmentSlot_Base[] slots)
    {
        throw new System.NotImplementedException();
    }
}
//public int GetattackSpeed()
//{
//    return (int)Weaponinfo.attackSpeed;
//}
