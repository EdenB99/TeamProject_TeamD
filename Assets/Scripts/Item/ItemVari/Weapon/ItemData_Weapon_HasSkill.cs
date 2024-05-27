using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ������ �� ������ ������ �����ϴ� ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Skill Weapon Data", order = 9)]
public class ItemData_Weapon_HasSkill : ItemData_Weapon, IWeapon ,IActivatable
{
    //QuickSlotUI, IngameUI, WeaponManger�� Ȱ���ؼ�
    //�������� ������ �� QuickslotUI�� ������ �������� �����ϰ�
    //Ű�� �Է��ϸ� �ش� �������� ��ų�� ����Ͽ� ��Ÿ���� �����ϰ�
    //weaponmanager�� ���� �ش� ������ ��ų�� ȣ���ϰ� �Ѵ�.
    //ItemActive�� ���� QuickSlot���� ȣ���Ѵ�.
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
