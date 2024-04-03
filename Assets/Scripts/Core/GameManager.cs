using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player
    {
        get
        {
            if(player == null)
                player = FindAnyObjectByType<Player>();
            return player;
        }
    }
    ItemDataManager itemDataManager;
    public ItemDataManager ItemData => itemDataManager;

    BulletDataManager bulletDataManager;
    public BulletDataManager BulletData => bulletDataManager;

    InventoryUI inventoryUI;
    public InventoryUI InventoryUI => inventoryUI;

    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();
        itemDataManager = GetComponent<ItemDataManager>();
        bulletDataManager = GetComponent<BulletDataManager>();
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }
}
