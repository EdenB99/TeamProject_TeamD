using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
    public GameObject storeSlotPrefab; 
    public Transform storeSlotContainer; 
    public ItemCode[] allItemCode;

    ItemDataManager itemDataManager;

    private void Awake()
    {
        itemDataManager = GameManager.Instance.ItemData;
        InitializeStoreSlots();
    }

    private void InitializeStoreSlots()
    {
        for (int i = 0; i < allItemCode.Length; i++)
        {
            GameObject slotObj = Instantiate(storeSlotPrefab, storeSlotContainer);
            StoreSlot storeSlot = slotObj.GetComponent<StoreSlot>();

            if (storeSlot != null)
            {
                storeSlot.SetItemCode(allItemCode[i]);
                Debug.Log(allItemCode[i]);
            }
        }
    }
}
