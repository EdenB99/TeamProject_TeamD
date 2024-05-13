using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] List<ItemData_Weapon> weaponsData = new List<ItemData_Weapon>();

    int currentWeaponIndex = 0;

    PlayerAction inputActions;

    GameObject currentWeaponInstance; // ���� ���� ������ ���� �ν��Ͻ�

    protected virtual void Awake()
    {
        inputActions = new PlayerAction();
    }

    protected virtual void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.SwitchWeapon.performed += SwitchWeapon;
    }

    protected virtual void OnDisable()
    {
        inputActions.Player.SwitchWeapon.performed -= SwitchWeapon;
        inputActions.Player.Disable();
    }

    protected virtual void Start()
    {
        InitializeWeapons();
    }

    public void InitializeWeapons()
    {
        if (weaponsData.Count > 0)
        {
            currentWeaponIndex = 0;
            SwitchToWeapon(currentWeaponIndex);
        }
    }

    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (weaponsData.Count == 0)
        {
            Debug.LogWarning("�κ��丮�� ���Ⱑ �����ϴ�.");
            return;
        }

        // ���� ���� �ε����� ����
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponsData.Count;
        SwitchToWeapon(currentWeaponIndex);
    }

    public void ActivateWeaponPrefab(ItemData_Weapon weaponData)
    {
        // ���� ���� �ν��Ͻ��� ����
        DestroyCurrentWeapon();

        // ���ο� ���� ������ ����
        if (weaponData != null)
        {
            GameObject weaponPrefab = Instantiate(weaponData.Weaponinfo.modelPrefab, transform.position, Quaternion.identity);
            currentWeaponInstance = weaponPrefab;
        }
        else
        {
            Debug.Log("���");
        }
    }

    private void SwitchToWeapon(int index)
    {
        if (weaponsData.Count == 0)
        {
            Debug.LogWarning("�κ��丮�� ���Ⱑ �����ϴ�.");
            return;
        }

        // ���� ���� �ν��Ͻ��� ����
        DestroyCurrentWeapon();

        // ���� ���� ������ ����
        ItemData_Weapon nextWeaponData = weaponsData[currentWeaponIndex];
        ActivateWeaponPrefab(nextWeaponData);
    }

    private void DestroyCurrentWeapon()
    {
        if (currentWeaponInstance != null)
        {
            Destroy(currentWeaponInstance);
            currentWeaponInstance = null;
        }
    }
    public void getWeaponData(ItemData_Weapon itemData)
    {
        // �� ������ ������ �ش� ���Կ� ���⸦ �߰��ϰ� �Լ� ����
        for (int i = 0; i < weaponsData.Count; i++)
        {
            if (weaponsData[i] == null)
            {
                weaponsData[i] = itemData;
                return;
            }
        }

        // �� ������ ������ ����Ʈ�� ���� ���� �߰�
        weaponsData.Add(itemData);
    }

    public void deleteWeaponData(ItemData_Weapon itemData)
    {
        // ���⸦ ã�Ƽ� ����
        for (int i = 0; i < weaponsData.Count; i++)
        {
            if (weaponsData[i] == itemData)
            {
                weaponsData.RemoveAt(i);
                return; // ���⸦ ã���� �Լ� ����
            }
        }
    }
}
