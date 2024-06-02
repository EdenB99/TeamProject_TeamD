using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUI : MonoBehaviour
{   //착용된 무기가 해제되도록, 빈 무기슬롯이라도 인덱스가 같다면 텍스트 출력


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
    public DetailUI detail;
    /// <summary>
    /// 버튼 UI
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

    // UI 온 오프 상태 조절
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
        }
        usableUI.ClickedDiscard += DiscardItem;
        usableUI.ClickedEquip_Use += Eqiup_UseItem;
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
        if (slotUIs[index].InvenSlot.ItemData != null)
        {
            usableUI.Open(slotUIs[index], IsStore);
        }

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

    private void Eqiup_UseItem(InvenSlotUI slotUI)
    {
        ItemData itemdata = slotUI.InvenSlot.ItemData;
        if (itemdata != null)
        {
            switch (itemdata.type)
            {
                case (ItemType.Weapon):
                    if (slotUI.InvenSlot.IsEquipped) //선택한 슬롯의 장비가 착용중이면
                    {   
                        IEquipable equipable = itemdata as IEquipable;
                        if (equipable != null)
                        {
                            equipable.UnEquip(WeaponsSlots);            
                        }
                        slotUI.InvenSlot.EquipItem(false); // 해당 장비를 인벤에서도 해제
                    } else
                    {
                        EquipWeapon(itemdata); //weaponslot과의 상호작용, 착용
                        slotUI.InvenSlot.EquipItem(true); //텍스트 변경을 위한 슬롯활성화
                    }
                    break;
                case (ItemType.Accessory):
                    if (slotUI.InvenSlot.IsEquipped) //선택한 슬롯의 장비가 착용중이면
                    {
                        IEquipable equipable = itemdata as IEquipable;
                        equipable.UnEquip(AccessoriesSlots);               //Accessoryslot에서 삭제
                        slotUI.InvenSlot.EquipItem(false); // 해당 장비를 인벤에서도 해제
                    }
                    else
                    {
                        EquipAccessory(itemdata); //Accessoryslot에서 상호작용, 착용
                        slotUI.InvenSlot.EquipItem(true); //텍스트 변경을 위한 슬롯활성화
                    }
                    break;
                case (ItemType.Consumable): //소비 아이템의 갯수를 세어서 퀵슬롯 아이템 사용시 줄어들어야함
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
    /// 무기군을 장착하는 함수
    /// </summary>
    /// <param name="itemData">장착할 무기 아이템 데이터</param>
    private void EquipWeapon(ItemData itemData)
    {
        IEquipable equipable = itemData as IEquipable;
        //아이템 데이터 내에 장착 인터페이스 확인
        bool isFull = true;
        //웨폰 슬롯의 가득참을 확인하는 변수
        for (int i = 0; i<WeaponsSlots.Length; i++)     //슬롯의 갯수까지
        {
            if (WeaponsSlots[i].SlotItemData == null)    //현재 슬롯이 비어있으면
            {
                equipable.Equip(WeaponsSlots[i]);       //아이템 내부 장착 함수를 실행
                isFull = false;                         //슬롯이 가득차지 않음
                break;                                  //반복문 종료
            } else
            {
                isFull = true;                          //비어있지 않으면 변경없음
            }
        }
        if (isFull)     //모든 슬롯이 차있다면
        {
            for (int i = 0; i < WeaponsSlots.Length; i++)      //슬롯의 갯수까지
            {
                if (!WeaponsSlots[i].Onhand)                //현재 슬롯이 손에 들려있다면
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
                    equipable.Equip(WeaponsSlots[i]);       //새로운 무기를 장착
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
                if (equipable != null)                  //장착 인터페이스가 있다면
                {
                    equipable.Equip(AccessoriesSlots[i]);   //아이템 내부 장착 함수를 실행
                }
                isFull = false;                         //슬롯이 가득차지 않음
                break;
            }
            else
            {
                isFull = true;
            }
        }
        if (isFull)        //슬롯이 가득찼을 때
        {
            IEquipable firstAccessory = AccessoriesSlots[0].SlotItemData as IEquipable;
            firstAccessory.UnEquip(AccessoriesSlots); //첫번째 슬롯의 아이템 해제
            equipable.Equip(AccessoriesSlots[0]); //빈칸에 아이템 장착
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
    /// 인벤토리 혹은 퀵슬롯에 아이템을 추가하는 함수
    /// </summary>
    /// <param name="itemCode">추가할 아이템</param>
    /// <param name="Count">추가될 갯수</param>
    public void getItem(ItemCode itemCode, int Count = 1) {

        ItemData data = GameManager.Instance.ItemData[itemCode];
        for (int i = 0; i < Count; i++) inven.AddItem(itemCode);
    }

    /// <summary>
    /// 인벤토리 내의 모든 아이템 삭제
    /// </summary>
    public void ClearAllItem()
    {
        for (int i = 0; i < slotUIs.Length; i++)
        {
            DiscardItem(slotUIs[i]);
        }
    }
    
}
