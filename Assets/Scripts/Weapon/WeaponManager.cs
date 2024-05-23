using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    //weaponManager��  list=> null�� 2���� ���õ� lsit
    //currentweaponindex
    //currentWeaponInstance

    //1.getweapondata ����Ʈ���� null���� ã�Ƽ� �ش� �ε����� ���� ������ ���� (itemdata)
    //���� ��� ���� null�� �ƴϸ� ù��° �ε������� ���� ������ �����ϰ� �ű�� ����
    //2. deletewepaondata ����Ʈ���� ���� ������ �����͸� ã�Ƽ� �ش� �������� ���� �ε��� ���� null�� ����
    //���� ���ⵥ���Ͱ� null, ���� ��� ���� null

    //1.2 ���� index���� ���ⵥ���͸� �ҷ���, clone�� ����
    //���� clone�� ������, ���� index������ �������ְ�, ���� clone�� ������ �����ϰ� ���� index�� clone ����
    //index���� ���ⵥ���Ͱ� null, ���� clone�� �ı��ϰ�, ���� clone���� ��������


    [SerializeField] public List<ItemData_Weapon> weaponsData = new List<ItemData_Weapon>();  
    
    private int currentWeaponIndex = 0;

    GameObject currentWeaponInstance; // ���� ���� ������ ���� �ν��Ͻ�


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

    private bool isWeaponEquipped = false;



    public float switchCooldown = 3.0f;

    float currentSwitchCoolTime = 0.0f;

    bool canSwitch = true;

    public void Awake()
    {
        inputActions = new PlayerAction();
        currentWeaponindexChange += Switchweapon;
        DontDestroyOnLoad(this.gameObject);
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
        CheckEquipWeapon();
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

    public void CheckEquipWeapon()
    {
        if(isWeaponEquipped && weaponsData[currentWeaponIndex] != null)
        {
            ActivateWeaponPrefab(weaponsData[currentWeaponIndex]);
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
        else
        {
            Debug.Log("���");
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
            //���߿� ����
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
        // ���⸦ ã�Ƽ� ����
        for (int i = 0; i < weaponsData.Count; i++)
        {
            if (weaponsData[i] == itemData)
            {
                weaponsData[i] = null;
                break;
            } else
            {
                Debug.Log("���� ������ ����");
            }
        }
    }
}