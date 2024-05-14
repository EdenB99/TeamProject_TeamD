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

        inventoryUI = GameManager.Instance.InventoryUI;
        inventoryUI.getItem(ItemCode.Helmet);
        inventoryUI.getItem(ItemCode.HealingPotion_A);
        inventoryUI.getItem(ItemCode.HealingPotion_A);
        inventoryUI.getItem(ItemCode.SwiftPotion);
        inventoryUI.getItem(ItemCode.HealingPotion_A);
        inventoryUI.getItem(ItemCode.Sword);
        inventoryUI.getItem(ItemCode.Spear);
        inventoryUI.getItem(ItemCode.KingSword);
    }
}
