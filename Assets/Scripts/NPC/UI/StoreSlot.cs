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
    ItemData currentItemData;
    NPC_Store storeNPC;
    Transform buyTab; 
    Transform storeCanvas;
    StoreSlot store;


    private void Awake()
    {
        storeNPC = FindAnyObjectByType<NPC_Store>();
        storeCanvas = storeNPC.transform.GetChild(1);
        Transform child = storeCanvas.GetChild(1);
        buyTab = child.GetChild(2);

        storeSlot = GetComponent<Button>();
        store = GetComponent<StoreSlot>();
        storeSlot.onClick.AddListener(() => ShowBuyTab());
    }

    public void ClearSlot()
    {
        itemIconImage.sprite = null;
        itemNameText.text = null;
        itemDescriptionText.text = null;
        itemPriceText.text = null;
        itemStatText.text = null;
        currentItemData = null;
        storeSlot.onClick.RemoveAllListeners();
    }

    public void SetItemData(ItemData itemdata)
    {
        if (itemdata != null)
        {
            currentItemData = itemdata;
            itemIconImage.sprite = itemdata.itemIcon;
            itemNameText.text = itemdata.itemName;
            itemDescriptionText.text = itemdata.itemDescription;
            itemPriceText.text = itemdata.price.ToString()+"G";
        }
    }

    public void ShowBuyTab()
    {
        BuyTab buyTabComponent = buyTab.GetComponent<BuyTab>();
        buyTabComponent.SetItemdata(currentItemData, store);
    }
}
