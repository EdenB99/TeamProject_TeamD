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
    /// �ش� ���Կ� �������� ����
    /// </summary>
    /// <param name="itemData">������ ������ ������</param>
    public void EquipItemdata(ItemData itemData)
    {
        if (itemData != null)
        {
            SlotItemData = itemData;
            SetItemImage(itemData.itemIcon);
        }
    }

    /// <summary>
    /// ���Գ� �������� ����
    /// </summary>
    public void ClearItemData()
    {
        SlotItemData = null;
        ClearSlot();
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
    /// <summary>
    /// ���� �ܺ� �̹��� �� �ؽ�Ʈ �ʱ�ȭ
    /// </summary>
    public void ClearSlot()
    {
        itemimage.sprite = null;
        SetEquipmentText(false);
    }

    
}

