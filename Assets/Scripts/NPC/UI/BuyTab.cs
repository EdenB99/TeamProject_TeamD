using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyTab : MonoBehaviour
{
    Inventory inventory;
    ItemData CurrentItemdata;
    Button yesButton;
    Button noButton;


  
    private void Awake()
    {
        Transform child = transform.GetChild(1);
        yesButton = child.GetComponent<Button>();
        yesButton.onClick.AddListener(OnBuyButtonClicked);
        child = transform.GetChild(2);
        noButton = child.GetComponent<Button>();
        noButton.onClick.AddListener(() => gameObject.SetActive(false));
    }
    private void Start()
    {
        inventory = GameManager.Instance.Player.PlayerStats.Inventory;
    }

    public void SetItemdata (ItemData itemdata)
    {
        this.gameObject.SetActive(true);
        CurrentItemdata = itemdata;
    }

    private void OnBuyButtonClicked()
    {
        inventory.AddItem(CurrentItemdata.code); // 인벤토리에 아이템 추가
        this.gameObject.SetActive(false); // 구매 후 구매창 비활성화
    }
}
