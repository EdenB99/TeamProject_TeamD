using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot_Base : MonoBehaviour
{

    private ItemData slotItemData;
    public ItemData SlotItemData
    {
        get => slotItemData;
        set
        {
            slotItemData = value;
            ItemDataChange?.Invoke(slotItemData);
        }
    }
    public Action<ItemData> ItemDataChange;

    Image itemimage;
    TextMeshProUGUI EquipText;

    private void Awake()
    {
        itemimage = transform.GetChild(0).GetComponent<Image>();
        EquipText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }


 
    /// <summary>
    /// �ش� ���Կ� �������� ����, 
    /// </summary>
    /// <param name="itemData">������ ������ ������</param>
    public void EquipItemdata(ItemData itemData = null)
    {
        if (itemData != null)
        {
            SlotItemData = itemData;
            SetItemImage(itemData.itemIcon);
            SetEquipmentText(true);
        } else
        {
            ClearSlot();
        }
    }

    /// <summary>
    /// ���� �� �̹��� ����
    /// </summary>
    /// <param name="itemSprite">������ �̹���</param>
    public void SetItemImage(Sprite itemSprite)
    {
        itemimage.sprite = itemSprite;
    }
    /// <summary>
    /// ���� �ܺ� �̹��� �� �ؽ�Ʈ �ʱ�ȭ
    /// </summary>
    public void ClearSlot()
    {
        SlotItemData = null;
        itemimage.sprite = null;
        SetEquipmentText(false);
    }
    /// <summary>
    /// �ؽ�Ʈ �¿���
    /// </summary>
    /// <param name="OnOff">��������</param>
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
}

