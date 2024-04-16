using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUI : MonoBehaviour
{
    public InventoryUI inventoryUI;
    public IngameUI ingameUI;
    Inventory inven;
    Player player;
    private void Start()
    {
        player = GameManager.Instance.Player;

        player.PlayerStats.TakeDamage(30);


        inven = player.PlayerStats.Inventory;
        ingameUI = player.PlayerStats.IngameUI;

        ingameUI.SetQuickSlotItem(0, ItemCode.HealingPotion_A, 2);
        


        inven.AddItem(ItemCode.HealingPotion_A);
        inven.AddItem(ItemCode.Apple);
        inven.AddItem(ItemCode.Sword);
        inven.AddItem(ItemCode.Diamond);


    }
}
