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

    WeaponManager weaponManager;

    public WeaponManager WeaponManager
    {
        get
        {
            if (weaponManager == null)
                FindAnyObjectByType<WeaponManager>();
            return weaponManager;
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
        weaponManager = FindAnyObjectByType<WeaponManager>();
    }
}
