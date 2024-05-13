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


    [SerializeField] List<ItemData_Weapon> weaponsData = new List<ItemData_Weapon>();

    private int currentWeaponIndex = 0;

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

    GameObject currentWeaponInstance; // ���� ���� ������ ���� �ν��Ͻ�

    protected virtual void Awake()
    {
        inputActions = new PlayerAction();
        currentWeaponindexChange += Switchweapon;
    }

    protected virtual void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.SwitchWeapon.performed += keyinput;
    }

    protected virtual void OnDisable()
    {
        inputActions.Player.SwitchWeapon.performed -= keyinput;
        inputActions.Player.Disable();
    }

    protected virtual void Start()
    {

    }

    public void keyinput(InputAction.CallbackContext context)
    {
        Debug.Log("�����Է�");
        CurrentWeaponIndex = (CurrentWeaponIndex + 1) % weaponsData.Count;
    }

    public void Switchweapon(int index)
    {
        if (weaponsData[currentWeaponIndex] != null)
        {
            ActivateWeaponPrefab(weaponsData[currentWeaponIndex]);
        }
        else Debug.Log("���� �ε����� ������ ������ ����");
    }

    public void ActivateWeaponPrefab(ItemData_Weapon weaponData)
    {
        Debug.Log("���� ������Ʈ �ı�");
        DestroyCurrentWeapon();
        if (weaponData != null)
        {
            Debug.Log("������Ʈ ����");
            GameObject weaponPrefab = Instantiate(weaponData.Weaponinfo.modelPrefab, transform.position, Quaternion.identity);
            currentWeaponInstance = weaponPrefab;
        }
        else
        {
            Debug.Log("���");
        }
    }

    private void DestroyCurrentWeapon()
    {
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
            currentWeaponInstance = null;
            Debug.Log("�ı� �Ϸ�");
        }
    }

    public void GetWeaponData(ItemData_Weapon itemData)
    {
        bool isfull = true;
        for (int i = 0; i < weaponsData.Count; i++)
        {
            if (weaponsData[i] == null)
            {
                Debug.Log("������ ����");
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


//