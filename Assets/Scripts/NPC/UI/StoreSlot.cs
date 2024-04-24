using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour
{
    Button storeSlot;
    Inventory inventory;
    Transform buyTab;
    GameManager manager;
    Player player;

    public ItemCode itemToPurchase;
    public Canvas canvas;

    private void Awake()
    {
        manager = new GameManager();
        player = GameManager.Instance.Player;
        inventory = player.PlayerStats.Inventory;

        // 구매 창 찾기
        Transform storeUI = canvas.transform.GetChild(1);
        buyTab = storeUI.GetChild(2);

        storeSlot = GetComponent<Button>();
        storeSlot.onClick.AddListener(() => OnItemPurchase());
    }

    public void OnItemPurchase()
    {
        buyTab.gameObject.SetActive(true);
        BuyTab buy = buyTab.GetComponent<BuyTab>();
        if (buy != null)
        {
            buy.SetItemToPurchase(itemToPurchase, inventory);
        }
    }

}
