using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour
{
    public Image itemIconImage;
    public TextMeshProUGUI itemNameText; 
    public TextMeshProUGUI itemDescriptionText; 
    public TextMeshProUGUI itemPriceText;
    public TextMeshProUGUI itemStatText;

    Button storeSlot;
    public BuyTab buyTab; 
    ItemData currentItemData;


    private void Awake()
    {
        storeSlot = GetComponent<Button>();
        ClearSlot();
    }

    private void Start()
    {
       
    }
    public void ClearSlot()
    {
        itemIconImage.sprite = null;
        itemNameText.text = null;
        itemDescriptionText.text = null;
        itemPriceText.text = null;
        itemStatText.text = null;
        storeSlot.onClick.RemoveAllListeners();
    }
    public void SetItemCode(ItemData itemdata)
    {
        if (itemdata != null)
        {
            currentItemData = itemdata;
            itemIconImage.sprite = itemdata.itemIcon;
            itemNameText.text = itemdata.itemName;
            itemDescriptionText.text = itemdata.itemDescription;
            itemPriceText.text = itemdata.price.ToString()+"G";
            storeSlot.onClick.AddListener(() => ShowButTab());
        }
    }

    public void ShowButTab()
    {
        buyTab.SetItemdata(currentItemData);
    }

}
