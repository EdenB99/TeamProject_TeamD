using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyTab : MonoBehaviour
{
    StoreSlot storeSlot;
    Button yesButton;
    Button noButton;
    GameManager manager;
    Player player;

    ItemCode itemToPurchase;
    Inventory inventory;

    private void Awake()
    {
        manager = new GameManager();
        player = GameManager.Instance.Player;
        inventory = player.PlayerStats.Inventory;
        storeSlot = FindAnyObjectByType<StoreSlot>();

        Transform child = transform.GetChild(1);
        yesButton = child.GetComponent<Button>();
        yesButton.onClick.AddListener(CompletePurchase);

        child = transform.GetChild(2);
        noButton = child.GetComponent<Button>();
        noButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    public void SetItemToPurchase(ItemCode item, Inventory inv)
    {
        itemToPurchase = item;
        inventory = inv;
    }

    private void CompletePurchase()
    {
        Debug.Log("구매 완료: " + itemToPurchase.ToString());
        gameObject.SetActive(false);
        if (inventory != null)
        {
            inventory.AddItem(itemToPurchase);
        }
    }
}
