using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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


    public Transform Weapons;
    InvenSlotUI[] WeaponsSlots;

    public Transform Accessoires;
    InvenSlotUI[] AccessoriesSlots;

    InventoryInput InventoryInput;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        //��� ����
        Transform child = transform.GetChild(0);
        child = child.transform.GetChild(2);
        child = child.transform.GetChild(0);
        child = child.transform.GetChild(0);
        slotUIs = child.GetComponentsInChildren<InvenSlotUI>();
        //child = transform.GetChild(1);
        //detail = GetComponent<DetaillUI>();
        //child = transform.GetChild(2);
        //usableUI = GetComponent<UsableUI>();

        WeaponsSlots = Weapons.GetComponentsInChildren<InvenSlotUI>();

        AccessoriesSlots = Accessoires.GetComponentsInChildren<InvenSlotUI>();
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
            Debug.Log("Close");
            Close();
        }
        else
        {
            Debug.Log("Open");
            Open();
        }
    }

    private void Open()
    {
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
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
            slotUIs[i].onPointerMove += OnSlotPointerMove;
        }

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
    private void Eqiup_UseItem(ItemData itemData)
    {
        if (itemData != null)
        {
            switch (itemData.type)
            {
                case (ItemType.Weapon):


                    break;
                case (ItemType.Accessory): 


                    break;
                case (ItemType.Consumable): 

                    break;
            }
        }
    }
    private void DiscardItem(InvenSlotUI slotUI)
    {
        
    }
}
