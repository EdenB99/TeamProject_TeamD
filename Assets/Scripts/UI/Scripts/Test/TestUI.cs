using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public InventoryUI inventoryUI;
    Inventory inven;
    private void Start()
    {
        inven = new Inventory();
        inven.AddItem(ItemCode.Apple);
        inven.AddItem(ItemCode.Apple);
        inven.AddItem(ItemCode.Apple);
       
        inven.AddItem(ItemCode.Apple);

        inventoryUI.InitializeInventory(inven);
    }
}
