using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreSlot : MonoBehaviour
{
    Button storeSlot;
    Inventory inventory;
    Transform buyTab;
    Canvas canvas;

    private void Awake()
    {
        // ���� â ã��
        canvas = FindAnyObjectByType<Canvas>();
        Transform storeUI = canvas.transform.GetChild(1);
        buyTab = storeUI.GetChild(2);

        storeSlot = GetComponent<Button>();

        storeSlot.onClick.AddListener(() => OnItemPurchase());
    }

    public void OnItemPurchase()
    {
        buyTab.gameObject.SetActive(true);
    }
}
