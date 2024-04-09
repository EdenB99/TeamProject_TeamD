using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{
    /// <summary>
    /// 이 UI가 보여줄 인벤토리
    /// </summary>
    Inventory inven;
    /// <summary>
    /// 인벤토리에 있는 slot UI들
    /// </summary>
    InvenSlotUI[] slotUIs;
    /// <summary>
    /// 임시 슬롯
    /// </summary>
    public TempSlotUI tempSlotUI;
    /// <summary>
    /// 상세 정보창
    /// </summary>
    public DetaillUI detail;
    /// <summary>
    /// 버튼 UI
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
        //요소 선언
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
    // UI 온 오프 상태 조절
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
    /// 인벤토리 초기화용 함수
    /// </summary>
    /// <param name="playerInventory">이 UI가 표시할 인벤토리</param>
    public void InitializeInventory(Inventory playerInventory)
    {
        inven = playerInventory;    // 저장

        for (uint i = 0; i < slotUIs.Length; i++)
        {
            slotUIs[i].InitializeSlot(inven[i]);    // 모든 인벤토리 슬롯 초기화
            slotUIs[i].onClick += OnSlotClick;
            slotUIs[i].onPointerEnter += OnItemDetailOn;
            slotUIs[i].onPointerExit += OnItemDetailOff;
            slotUIs[i].onPointerMove += OnSlotPointerMove;
        }

        tempSlotUI.InitializeSlot(inven.TempSlot);  // 임시 슬롯 초기화
        Close();
    }
    /// <summary>
    /// 아이템 상세 정보창을 여는 함수
    /// </summary>
    /// <param name="index">상세 정보창에서 표시될 아이템이 들어있는 슬롯의 인덱스</param>
    private void OnItemDetailOn(uint index)
    {
        if (!usableUI.isOn)
        {
            detail.Open(slotUIs[index].InvenSlot.ItemData); // 열기
        }
    }
    /// <summary>
    /// 아이템 상세 정보창을 닫는 함수
    /// </summary>
    private void OnItemDetailOff()
    {
        detail.Close(); // 닫기
    }
    /// <summary>
    /// 슬롯안에서 마우스 커서가 움직였을 때 실행되는 함수
    /// </summary>
    /// <param name="screen">마우스 커서의 스크린 좌표</param>
    private void OnSlotPointerMove(Vector2 screen)
    {
        detail.MovePosition(screen);    // 움직이기
        usableUI.MovePosition(screen);
    }
    /// <summary>
    /// 슬롯을 클릭했을 때 실행되는 함수
    /// </summary>
    /// <param name="index">클릭한 슬롯의 인덱스</param>
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
        inven.MoveItem(tempSlotUI.Index, index);    // 임시 -> 도착으로 아이템 옮기기

        if (tempSlotUI.InvenSlot.IsEmpty)
        {
            tempSlotUI.Close();                     // 임시 슬롯이 비면 닫는다.
        }

        detail.isOn = false; // 퍼즈 풀고
        if (isSlotEnd)           // 슬롯에서 끝이 났으면 상세 정보창 다시 열기
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
