using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public InventoryUI inventoryUI;
    Inventory inven;
    Player player;
    private void Start()
    {
        inven = new Inventory();
        inven.AddItem(ItemCode.HealingPotion_A);
        inven.AddItem(ItemCode.Apple);
        inven.AddItem(ItemCode.Sword);
        inven.AddItem(ItemCode.Diamond);


        inventoryUI.InitializeInventory(inven);

        player = GameManager.Instance.Player;

        player.PlayerStats.TakeDamage(30);
    }
}
