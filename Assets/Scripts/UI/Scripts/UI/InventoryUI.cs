using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// �� UI�� ������ �κ��丮
    /// </summary>
    Inventory inven;
    /// <summary>
    /// �κ��丮�� �ִ� slot UI��
    /// </summary>
    InvenSlotUI[] slotUIs;
    /// <summary>
    /// �ӽ� ����
    /// </summary>
    public TempSlotUI tempSlotUI;
    /// <summary>
    /// �� ����â
    /// </summary>
    public DetaillUI detail;
    /// <summary>
    /// ��ư UI
    /// </summary>
    public UsableUI usableUI;

    public IngameUI ingameUI;


    public Transform InvenSlotsTransform;
    public Transform Weapons;
    WeaponSlotUI[] WeaponsSlots;

    public Transform Accessoires;
    AccessorySlotUI[] AccessoriesSlots;

    InventoryInput InventoryInput;
    CanvasGroup canvasGroup;
    private void Awake()
    {
        slotUIs = InvenSlotsTransform.GetComponentsInChildren<InvenSlotUI>();

        WeaponsSlots = Weapons.GetComponentsInChildren<WeaponSlotUI>();

        AccessoriesSlots = Accessoires.GetComponentsInChildren<AccessorySlotUI>();
        InventoryInput = new InventoryInput();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void OnEnable()
    {
        InventoryInput.Inventory.Enable();
        InventoryInput.Inventory.Open_Close.performed += OnInvenstate;
    }
    private void OnDisable()
    {
 
        InventoryInput.Inventory.Open_Close.performed -= OnInvenstate;
        InventoryInput.Inventory.Disable();
    }
    // UI �� ���� ���� ����
    private void OnInvenstate(InputAction.CallbackContext obj)
    {
        if (canvasGroup.interactable)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    private void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        ingameUI.SetQuickSlotOnOff(false);
        
    }

    private void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        ingameUI.SetQuickSlotOnOff(true);
    }
    /// <summary>
    /// �κ��丮 �ʱ�ȭ�� �Լ�
    /// </summary>
    /// <param name="playerInventory">�� UI�� ǥ���� �κ��丮</param>
    public void InitializeInventory(Inventory playerInventory)
    {
        inven = playerInventory;    // ����

        for (uint i = 0; i < slotUIs.Length; i++)
        {
            slotUIs[i].InitializeSlot(inven[i]);    // ��� �κ��丮 ���� �ʱ�ȭ
            slotUIs[i].onClick += OnSlotClick;
            slotUIs[i].onPointerEnter += OnItemDetailOn;
            slotUIs[i].onPointerExit += OnItemDetailOff;
        }
        usableUI.ClickedDiscard += DiscardItem;
        usableUI.ClickedEquip_Use += Eqiup_UseItem;
        tempSlotUI.InitializeSlot(inven.TempSlot);  // �ӽ� ���� �ʱ�ȭ
        Close();
    }




    /// <summary>
    /// ������ �� ����â�� ���� �Լ�
    /// </summary>
    /// <param name="index">�� ����â���� ǥ�õ� �������� ����ִ� ������ �ε���</param>
    private void OnItemDetailOn(uint index)
    {
        if (!usableUI.isOn)
        {
            detail.Open(slotUIs[index].InvenSlot.ItemData); // ����
        }
    }
    /// <summary>
    /// ������ �� ����â�� �ݴ� �Լ�
    /// </summary>
    private void OnItemDetailOff()
    {
        detail.Close(); // �ݱ�
    }





    /// <summary>
    /// ���Ծȿ��� ���콺 Ŀ���� �������� �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="screen">���콺 Ŀ���� ��ũ�� ��ǥ</param>
    private void OnSlotPointerMove(Vector2 screen)
    {
        detail.MovePosition(screen);    // �����̱�
        usableUI.MovePosition(screen);
    }
    /// <summary>
    /// ������ Ŭ������ �� ����Ǵ� �Լ�
    /// </summary>
    /// <param name="index">Ŭ���� ������ �ε���</param>
    private void OnSlotClick(uint index)
    {
        if (detail.isOn)
        {
            detail.Close();
        }
        if (usableUI.isOn)
        {
            usableUI.Close();
        }
        usableUI.Open(slotUIs[index]);

    }
    private void OnItemMoveEnd(uint index, bool isSlotEnd)
    {
        inven.MoveItem(tempSlotUI.Index, index);    // �ӽ� -> �������� ������ �ű��

        if (tempSlotUI.InvenSlot.IsEmpty)
        {
            tempSlotUI.Close();                     // �ӽ� ������ ��� �ݴ´�.
        }

        detail.isOn = false; // ���� Ǯ��
        if (isSlotEnd)           // ���Կ��� ���� ������ �� ����â �ٽ� ����
        {
            detail.Open(inven[index].ItemData);
        }
    }







    private void Eqiup_UseItem(InvenSlotUI slotUI)
    {
        ItemData itemdata = slotUI.InvenSlot.ItemData;
        if (itemdata != null)
        {
            switch (itemdata.type)
            {
                case (ItemType.Weapon):
                    if (slotUI.InvenSlot.IsEquipped) //������ ������ ��� �������̸�
                    {   
                        IEquipable equipable = itemdata as IEquipable;
                        if (equipable != null)
                        {
                            equipable.UnEquip();               //weaponslot���� ����
                        }
                        slotUI.InvenSlot.EquipItem(false); // �ش� ��� �κ������� ����
                    } else
                    {
                        EquipWeapon(itemdata); //weaponslot���� ��ȣ�ۿ�, ����
                        slotUI.InvenSlot.EquipItem(true); //�ؽ�Ʈ ������ ���� ����Ȱ��ȭ
                    }
                    break;
                case (ItemType.Accessory):
                    if (slotUI.InvenSlot.IsEquipped) //������ ������ ��� �������̸�
                    {
                        IEquipable equipable = itemdata as IEquipable;
                        equipable.UnEquip();               //Accessoryslot���� ����
                        slotUI.InvenSlot.EquipItem(false); // �ش� ��� �κ������� ����
                    }
                    else
                    {
                        EquipAccessory(itemdata); //Accessoryslot���� ��ȣ�ۿ�, ����
                        slotUI.InvenSlot.EquipItem(true); //�ؽ�Ʈ ������ ���� ����Ȱ��ȭ
                    }
                    break;
                case (ItemType.Consumable):
                    slotUI.InvenSlot.UseItem();
                    break;
                default: usableUI.Close(); break;
            }
        }
    }

    

    /// <summary>
    /// ���ⱺ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="itemData">������ ���� ������ ������</param>
    private void EquipWeapon(ItemData itemData)
    {
        IEquipable equipable = itemData as IEquipable;
        //������ ������ ���� ���� �������̽� Ȯ��
        bool isFull = false;
        //���� ������ �������� Ȯ���ϴ� ����
        for (int i = 0; i<WeaponsSlots.Length; i++) //������ ��������
        {
            if (WeaponsSlots[i].SlotItemData == null) //���� ������ ���������
            {
                equipable.Equip(WeaponsSlots[i]);   //������ ���� ���� �Լ��� ����
                isFull = false;                         //������ �������� ����
                break;                                  //�ݺ��� ����
            } else
            {
                isFull = true;                          //������� ������ �������
            }
        }
        /*if (isFull)     //��� ������ ����ִٸ�
        {
            for (int i =0; i<WeaponsSlots.Length; i++)      //������ ��������
            {
                if (!WeaponsSlots[i].Onhand)                //���� ������ �տ� ����ִٸ�
                {
                    IEquipable handEquipable = WeaponsSlots[i].SlotItemData as IEquipable;                           //���� �տ� �鸰 ������ �����Լ��� ����
                    handEquipable.UnEquip();                //�տ� �鸰 ���� ����
                    equipable.Equip(WeaponsSlots[i]);       //���ο� ���⸦ ����
                    break;
                }
            }
        }*/
    }

    private void EquipAccessory(ItemData itemdata)
    {
        IEquipable equipable = itemdata as IEquipable;
        bool isFull = false;
        for (int i = 0; i < AccessoriesSlots.Length; i++)
        {
            if (AccessoriesSlots[i].SlotItemData == null)
            {
                if (equipable != null)                  //���� �������̽��� �ִٸ�
                {
                    equipable.Equip(AccessoriesSlots[i]);   //������ ���� ���� �Լ��� ����
                }
                isFull = false;                         //������ �������� ����
                break;
            }
            else
            {
                isFull = true;
            }
        }
        if (isFull)        //������ ����á�� ��
        {
            IEquipable firstAccessory = AccessoriesSlots[0].SlotItemData as IEquipable;
            firstAccessory.UnEquip(); //ù��° ������ ������ ����
            equipable.Equip(AccessoriesSlots[0]); //��ĭ�� ������ ����
        }
    }


    private void WeaponDataToPlayer(ItemData itemdata)
    {

    }

    private void DiscardItem(InvenSlotUI slotUI)
    {
        if (slotUI.InvenSlot.IsEquipped)
        {
            Eqiup_UseItem(slotUI);
        }
        slotUI.InvenSlot.ClearSlotItem();
    }

}
