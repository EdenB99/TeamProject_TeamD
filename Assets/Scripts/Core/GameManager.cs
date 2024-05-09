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

    IngameUI ingameUI;
    public IngameUI IngameUI => ingameUI;

    public Material Mobmaterial;

    WeaponBase_Call_Swab weaponBase_Call_Swab;

    public WeaponBase_Call_Swab WeaponBase_Call_Swab
    {
        get
        {
            if (weaponBase_Call_Swab == null)
                FindAnyObjectByType<WeaponBase_Call_Swab>();
            return weaponBase_Call_Swab;
        }
    }

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
        ingameUI = FindAnyObjectByType<IngameUI>();
        weaponBase_Call_Swab = FindAnyObjectByType<WeaponBase_Call_Swab>();
    }
}
