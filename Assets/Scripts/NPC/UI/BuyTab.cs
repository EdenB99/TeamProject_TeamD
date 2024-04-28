using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyTab : MonoBehaviour
{
    Inventory inventory;
    ItemCode currentItemCode;
    Button yesButton;
    Button noButton;
    ItemDataManager itemDataManager;

    private void Awake()
    {
        itemDataManager = GameManager.Instance.ItemData;

        Transform child = transform.GetChild(1);
        yesButton = child.GetComponent<Button>();
        yesButton.onClick.AddListener(OnBuyButtonClicked);

        child = transform.GetChild(2);
        noButton = child.GetComponent<Button>();
        noButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void SetItemCode(ItemCode itemCode)
    {
        currentItemCode = itemCode;

        ItemData itemData = itemDataManager[itemCode];
    }

    private void OnBuyButtonClicked()
    {
        inventory.AddItem(currentItemCode); // 인벤토리에 아이템 추가
        this.gameObject.SetActive(false); // 구매 후 구매창 비활성화
    }
}
