using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour
{
    Button storeSlot;
    Inventory inventory;
    Transform buyTab;
    public ItemCode itemToPurchase;
    public Canvas canvas;

    private void Awake()
    {
        // 구매 창 찾기
        Transform storeUI = canvas.transform.GetChild(1);
        buyTab = storeUI.GetChild(2);

        storeSlot = GetComponent<Button>();
        storeSlot.onClick.AddListener(() => OnItemPurchase());
    }

    public void OnItemPurchase()
    {
        buyTab.gameObject.SetActive(true);
        BuyTab buyTabScript = buyTab.GetComponent<BuyTab>();
        if (buyTabScript != null)
        {
            buyTabScript.SetItemToPurchase(itemToPurchase, inventory);
        }
    }

}
