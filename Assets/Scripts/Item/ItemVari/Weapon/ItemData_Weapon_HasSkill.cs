using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ������ �� ������ ������ �����ϴ� ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Weapon Data", order = 2)]
public class ItemData_Weapon_HasSkill : ItemData_Weapon, IWeapon ,IActivatable
{
    //QuickSlotUI, IngameUI, WeaponManger�� Ȱ���ؼ�
    //�������� ������ �� QuickslotUI�� ������ �������� �����ϰ�
    //Ű�� �Է��ϸ� �ش� �������� ��ų�� ����Ͽ� ��Ÿ���� �����ϰ�
    //weaponmanager�� ���� �ش� ������ ��ų�� ȣ���ϰ� �Ѵ�.
    //ItemActive�� ���� QuickSlot���� ȣ���Ѵ�.

    public override void Equip(EquipmentSlot_Base slot)
    {
        
    }

    public bool ItemActive(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }

    public override void UnEquip(EquipmentSlot_Base[] slots)
    {
        base.UnEquip(slots);
    }
}
