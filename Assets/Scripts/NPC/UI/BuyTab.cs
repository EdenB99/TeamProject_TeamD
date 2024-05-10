using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyTab : MonoBehaviour
{
    InventoryUI inventoryUI;
    ItemData CurrentItemdata;
    Button yesButton;
    Button noButton;
    StoreSlot slot;
    TextMeshProUGUI text;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        yesButton = child.GetComponent<Button>();
        yesButton.onClick.AddListener(OnBuyButtonClicked);

        child = transform.GetChild(2);
        noButton = child.GetComponent<Button>();
        noButton.onClick.AddListener(() => gameObject.SetActive(false));
        
        text = GetComponentInChildren<TextMeshProUGUI>();
        //slot = FindAnyObjectByType<StoreSlot>();
    }

    private void Start()
    {
        inventoryUI = GameManager.Instance.InventoryUI;
    }

    public void SetItemdata (ItemData itemdata, StoreSlot currentslot)
    {
        slot = currentslot;
        this.gameObject.SetActive(true);
        CurrentItemdata = itemdata;
        text.text = $"{CurrentItemdata.itemName}�� �����Ͻðڽ��ϱ�?";
    }

    private void OnBuyButtonClicked()
    {
        inventoryUI.getItem(CurrentItemdata.code);
        slot.ClearSlot();
        this.gameObject.SetActive(false);
    }
}
