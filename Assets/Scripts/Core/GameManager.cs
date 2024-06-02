using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    MainCamera mainCamera;

    public MainCamera MainCamera
    { 
        get
        {
            if (mainCamera == null)
                mainCamera = FindAnyObjectByType<MainCamera>();
            return mainCamera;
        } 
    }



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
        private set { weaponManager = value; }
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

    // 게임 클리어창 관련 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ 

    private int killCount;
    public int KillCount => killCount;

    public int goldCount = 0;

    private int GoldCount => goldCount;
    

    private float playTime = 0.0f;

    public float PlayTime => playTime;

    private void Update()
    {
        if ( !gameClear)
        {
            playTime += Time.deltaTime;
        }
    }

    public void KillCountAdd()
    {
        killCount++;
    }

    public void GoldCountAdd(int gold)
    {
        goldCount += gold;
    }


    /// <summary>
    /// 클리어 여부
    /// </summary>
    public bool gameClear = false;

    public void GameReset()
    {
        playTime = 0;
        goldCount = 0;
        gameClear = false;
        killCount = 0;
    }
        



}
