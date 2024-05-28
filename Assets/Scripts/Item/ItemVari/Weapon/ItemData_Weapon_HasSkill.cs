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
    [Header("���� ��ų ����")]
    public GameObject SkillPrefab;
    GameObject skillInstance;


    public override void Equip(EquipmentSlot_Base slot)
    {
        slot.SlotItemData = this;
        GameManager.Instance.InventoryUI.ingameUI.ChangeQuickSlotItem(3, this.code);
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
        SKill(); return true;
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
        GameManager.Instance.InventoryUI.ingameUI.ClearQuickSlotItem(3);
        WeaponManager weaponManager = FindObjectOfType<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.DeleteWeaponData(this);
            weaponManager.DestroyCurrentWeapon();
        }
    }
    public void SKill()
    {
        Player player = GameManager.Instance.Player;
        if (skillInstance == null) // ��ų �ν��Ͻ��� �������� �ʴ� ��쿡�� ����
        {
            skillInstance = Instantiate(SkillPrefab, player.transform.position, Quaternion.identity);
            skillInstance.transform.SetParent(player.transform);
            skillInstance.transform.localPosition = new Vector2(0, 0.5f);
        }
        else
        {
            Debug.Log("��ų�� �̹� Ȱ��ȭ�Ǿ� �ֽ��ϴ�.");
        }

    }
}
