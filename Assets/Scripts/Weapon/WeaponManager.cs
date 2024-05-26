using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WeaponManager : MonoBehaviour
{
    //weaponManager는  list=> null값 2개로 세팅된 lsit
    //currentweaponindex
    //currentWeaponInstance

    //1.getweapondata 리스트에서 null값을 찾아서 해당 인덱스에 무기 데이터 삽입 (itemdata)
    //만약 모든 값이 null이 아니면 첫번째 인덱스에서 무기 데이터 제거하고 거기다 삽입
    //2. deletewepaondata 리스트에서 받은 아이템 데이터를 찾아서 해당 아이템을 가진 인덱스 값을 null로 변경
    //받은 무기데이터가 null, 현재 모든 값이 null

    //1.2 변경 index값의 무기데이터를 불러서, clone값 변경
    //현재 clone이 없으면, 현재 index값으로 생성해주고, 현재 clone이 있으면 제거하고 현재 index값 clone 생성
    //index값의 무기데이터가 null, 현재 clone만 파괴하고, 무기 clone생성 하지않음


    [SerializeField] public List<ItemData_Weapon> weaponsData = new List<ItemData_Weapon>();

    public int currentWeaponIndex = 0;

    public int CurrentWeaponIndex
    {
        get => currentWeaponIndex;
        set
        {
            currentWeaponIndex = value;
            currentWeaponindexChange?.Invoke(currentWeaponIndex);
        }
    }
    public Action<int> currentWeaponindexChange;
    PlayerAction inputActions;

    GameObject currentWeaponInstance; // 현재 씬에 생성된 무기 인스턴스

    public bool OnHand
    {
        get => OnHand;
        set
        {
            isWeaponEquipped = value;
        }
    }

    private bool isWeaponEquipped = false;

    public float switchCooldown = 1.0f;

    float currentSwitchCoolTime = 0.0f;

    bool canSwitch = true;


    public void Awake()
    {
        inputActions = new PlayerAction();
        currentWeaponindexChange += Switchweapon;
        DontDestroyOnLoad(gameObject);
    }

    public void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.SwitchWeapon.performed += KeyInput;
    }

    public void OnDisable()                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
    {
        inputActions.Player.SwitchWeapon.performed -= KeyInput;
        inputActions.Player.Disable();
    }

    public void Start()
    {
        currentSwitchCoolTime = switchCooldown;
        canSwitch = true;
        isWeaponEquipped = false;
    }

    public void Update()
    {
        if (currentSwitchCoolTime < switchCooldown)
        {
            currentSwitchCoolTime += Time.deltaTime;
            canSwitch = false;
        }
        else
        {
            canSwitch = true;
        }
    }

    protected bool IsPlayerAlive()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        return playerStats != null && playerStats.IsAlive;
    }

    public void KeyInput(InputAction.CallbackContext context)
    {
        if (IsPlayerAlive() && canSwitch)
        {
            CurrentWeaponIndex = (CurrentWeaponIndex + 1) % weaponsData.Count;
            currentSwitchCoolTime = 0.0f;
            canSwitch = false;
        }
    }



    public void Switchweapon(int index)
    {
        if (weaponsData[currentWeaponIndex] != null)
        {
            ActivateWeaponPrefab(weaponsData[currentWeaponIndex]);
        }
        else
        {
            DestroyCurrentWeapon();
        }
    }

    public void ActivateWeaponPrefab(ItemData_Weapon weaponData)
    {
        DestroyCurrentWeapon();
        if (weaponData != null)
        {
            GameObject weaponPrefab = Instantiate(weaponData.Weaponinfo.modelPrefab, transform.position, Quaternion.identity);
            currentWeaponInstance = weaponPrefab;
            isWeaponEquipped = true;
        }
    }

    public void DestroyCurrentWeapon()
    {
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
            currentWeaponInstance = null;
            isWeaponEquipped = false;
        }
    }

    public void GetWeaponData(ItemData_Weapon itemData)
    {
        bool isfull = true;
        for (int i = 0; i < weaponsData.Count; i++)
        {
            if (weaponsData[i] == null)
            {
                weaponsData[i] = itemData;
                isfull = false;
                break;
            }
        }
        if (isfull)
        {
            //나중에 수정
            int emptyindex;
            if (currentWeaponIndex == 0)
            {
                emptyindex = 1;
            } else emptyindex = 0;
            weaponsData[emptyindex] = itemData;
        }
    }
    public void DeleteWeaponData(ItemData_Weapon itemData)
    {
        // 무기를 찾아서 제거
        for (int i = 0; i < weaponsData.Count; i++)
        {
            if (weaponsData[i] == itemData)
            {
                weaponsData[i] = null;
                break;
            }
        }
    }    
}