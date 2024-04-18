using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot_Base : MonoBehaviour
{

    ItemData SlotItemData;
    Image itemimage;
    TextMeshProUGUI EquipText;

    private bool isEquip;
    public bool IsEquip
    {
        get => isEquip;
        set
        {
            isEquip = value;
        }
    }

    private void Awake()
    {
        itemimage = transform.GetChild(0).GetComponent<Image>();
        EquipText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        
    }
 
    /// <summary>
    /// 해당 슬롯에 아이템을 장착
    /// </summary>
    /// <param name="itemData">장착할 아이템 데이터</param>
    public void EquipItemdata(ItemData itemData)
    {
        if (itemData != null)
        {
            SlotItemData = itemData;
            SetItemImage(itemData.itemIcon);
        }
    }

    /// <summary>
    /// 슬롯내 아이템을 삭제
    /// </summary>
    public void ClearItemData()
    {
        SlotItemData = null;
        ClearSlot();
    }
    /// <summary>
    /// 슬롯 내 이미지 설정
    /// </summary>
    /// <param name="itemSprite">변경할 이미지</param>
    public void SetItemImage(Sprite itemSprite)
    {
        itemimage.sprite = itemSprite;
    }
    /// <summary>
    /// 텍스트 온오프
    /// </summary>
    /// <param name="OnOff">켜짐여부</param>
    public void SetEquipmentText(bool OnOff)
    {
        if (OnOff)
        {
            EquipText.text = "E";
        }
        else
        {
            EquipText.text = null;
        }
    }
    /// <summary>
    /// 슬롯 외부 이미지 및 텍스트 초기화
    /// </summary>
    public void ClearSlot()
    {
        itemimage.sprite = null;
        SetEquipmentText(false);
    }

    
}

