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
    public Canvas canvas;

    ItemDataManager itemDataManager;
    Button storeSlot;
    BuyTab buyTab; 
    ItemCode currentItemCode;

    private void Awake()
    {
        itemDataManager = GameManager.Instance.ItemData;

        Transform storeUI = canvas.transform.GetChild(1);
        buyTab = storeUI.gameObject.transform.GetComponentInChildren<BuyTab>();

        storeSlot = GetComponent<Button>();
        storeSlot.onClick.AddListener(() => ShowButTab());

    }

    public void SetItemCode(ItemCode itemCode)
    {
        currentItemCode = itemCode;

        ItemData itemData = itemDataManager[itemCode];

        if (itemData != null)
        {
            itemIconImage.sprite = itemData.itemIcon;
            itemNameText.text = itemData.itemName;
            itemDescriptionText.text = itemData.itemDescription;
            itemPriceText.text = itemData.price.ToString();
        }
    }

    public void ShowButTab()
    {
        buyTab.SetItemCode(currentItemCode);
        buyTab.gameObject.SetActive(true);
    }

}
