using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{   //����� ���Ⱑ �����ǵ���, �� ���⽽���̶� �ε����� ���ٸ� �ؽ�Ʈ ���


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
    public DetailUI detail;
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

    WeaponManager weaponManager;

    private bool isStore;
    public bool IsStore {
        get => isStore;
        set
        {
            isStore = value;
            if (isInvenOn || isStore)
            {
                PlayerMoveToggle?.Invoke(true);
            }
            else PlayerMoveToggle?.Invoke(false);
        }
    }
    private bool isInvenOn;
    public bool IsInvenOn
    {
        get => isInvenOn;
        set
        {
            isInvenOn = value;
            if (isInvenOn || isStore)
            {
                PlayerMoveToggle?.Invoke(true);
            } 
            else PlayerMoveToggle?.Invoke(false);
        }
    }

    public  Action<bool> PlayerMoveToggle;
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
    private void Start()
    {
        for (int i =0; i < ingameUI.IngameSlotUIs.Length; i++)
        {
            ingameUI.IngameSlotUIs[i].ItemUse += UseConsumableItem;
        }
        GameManager.Instance.WeaponManager.currentWeaponindexChange += SetOnhandWepaonIndex;
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
        IsInvenOn = true;
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        ingameUI.SetQuickSlotOnOff(false);
        ingameUI.goldPanel.PanelToggle(true);
        
    }

    private void Close()
    {
        IsInvenOn = false;
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        ingameUI.SetQuickSlotOnOff(true);
        ingameUI.goldPanel.PanelToggle(false);
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
        if (slotUIs[index].InvenSlot.ItemData != null)
        {
            usableUI.Open(slotUIs[index], IsStore);
        }

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
                            equipable.UnEquip(WeaponsSlots);            
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
                        equipable.UnEquip(AccessoriesSlots);               //Accessoryslot���� ����
                        slotUI.InvenSlot.EquipItem(false); // �ش� ��� �κ������� ����
                    }
                    else
                    {
                        EquipAccessory(itemdata); //Accessoryslot���� ��ȣ�ۿ�, ����
                        slotUI.InvenSlot.EquipItem(true); //�ؽ�Ʈ ������ ���� ����Ȱ��ȭ
                    }
                    break;
                case (ItemType.Consumable): //�Һ� �������� ������ ��� ������ ������ ���� �پ������
                    ingameUI.AddQuickSlotItem(itemdata.code, CountConsumableItem(itemdata));
                    break;
                default: usableUI.Close(); break;
            }
        }
    }
    
    
    private int CountConsumableItem(ItemData itemData)
    {
        int Count = 0;
        for (int i =0; i<slotUIs.Length; i++)
        {
            if (slotUIs[i].InvenSlot.ItemData == itemData)
            {
                Count++;
            }
        }
        return Count;
    }
    private void UseConsumableItem(ItemData itemData)
    {
        if (itemData.type == ItemType.Consumable)
        {
            for (int i = 0; i < slotUIs.Length; i++)
            {
                if (slotUIs[i].InvenSlot.ItemData == itemData)
                {
                    slotUIs[i].InvenSlot.ClearSlotItem();
                    break;
                }
            }
        }

    }

    public void SetOnhandWepaonIndex(int index)
    {

        for (int i = 0; i < WeaponsSlots.Length; i++)
        {
            if (i == index)
            {
                WeaponsSlots[i].Onhand = true;
            }
            else WeaponsSlots[i].Onhand = false;
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
        bool isFull = true;
        //���� ������ �������� Ȯ���ϴ� ����
        for (int i = 0; i<WeaponsSlots.Length; i++)     //������ ��������
        {
            if (WeaponsSlots[i].SlotItemData == null)    //���� ������ ���������
            {
                equipable.Equip(WeaponsSlots[i]);       //������ ���� ���� �Լ��� ����
                isFull = false;                         //������ �������� ����
                break;                                  //�ݺ��� ����
            } else
            {
                isFull = true;                          //������� ������ �������
            }
        }
        if (isFull)     //��� ������ ���ִٸ�
        {
            for (int i = 0; i < WeaponsSlots.Length; i++)      //������ ��������
            {
                if (!WeaponsSlots[i].Onhand)                //���� ������ �տ� ����ִٸ�
                {
                    for (int j = 0; j < slotUIs.Length; j++)
                    {
                        if (slotUIs[j].InvenSlot.ItemData == WeaponsSlots[i].SlotItemData && slotUIs[j].InvenSlot.IsEquipped)
                        {
                            slotUIs[j].InvenSlot.EquipItem(false);
                        }
                    }
                    IEquipable handEquipable = WeaponsSlots[i].SlotItemData as IEquipable;
                    handEquipable.UnEquip(WeaponsSlots);
                    equipable.Equip(WeaponsSlots[i]);       //���ο� ���⸦ ����
                    break;
                }
            }
        }
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
            firstAccessory.UnEquip(AccessoriesSlots); //ù��° ������ ������ ����
            equipable.Equip(AccessoriesSlots[0]); //��ĭ�� ������ ����
        }
    }

    private void DiscardItem(InvenSlotUI slotUI)
    {
        if (slotUI.InvenSlot.IsEquipped)
        {
            Eqiup_UseItem(slotUI);
        }
        if (IsStore)
        {
            Player player = GameManager.Instance.Player;
            player.Gold += slotUI.InvenSlot.ItemData.price;
        }
        slotUI.InvenSlot.ClearSlotItem();
    }
    /// <summary>
    /// �κ��丮 Ȥ�� �����Կ� �������� �߰��ϴ� �Լ�
    /// </summary>
    /// <param name="itemCode">�߰��� ������</param>
    /// <param name="Count">�߰��� ����</param>
    public void getItem(ItemCode itemCode, int Count = 1) {

        ItemData data = GameManager.Instance.ItemData[itemCode];
        for (int i = 0; i < Count; i++) inven.AddItem(itemCode);
    }

    /// <summary>
    /// �κ��丮 ���� ��� ������ ����
    /// </summary>
    public void ClearAllItem()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            DiscardItem(slotUIs[i]);
        }
    }
    
}
