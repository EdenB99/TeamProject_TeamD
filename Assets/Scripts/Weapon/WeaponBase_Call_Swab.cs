using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBase_Call_Swab : MonoBehaviour
{
    // ���� ����Ī ���� ----------------------------------------------------------------------------------
    [SerializeField] List<ItemData_Weapon> weaponsData = new List<ItemData_Weapon>();

    int currentWeaponIndex = 0;

    PlayerAction inputActions;

    GameObject currentWeaponPrefab;

    protected virtual void Awake()
    {
        inputActions = new PlayerAction();
        Debug.Log($"{currentWeaponPrefab}");
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
        Debug.Log($"{currentWeaponPrefab}");
    }

    /// <summary>
    /// ���� ����Ī �Լ�
    /// </summary>
    /// <param name="context"></param>
    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (weaponsData.Count == 0)
        {
            Debug.LogWarning("�κ��丮�� ���Ⱑ �����ϴ�.");
            return;
        }

        currentWeaponIndex = (currentWeaponIndex + 1) % weaponsData.Count;
        SwitchToWeapon(currentWeaponIndex);
        Debug.Log($"{currentWeaponPrefab}");
    }

    /// <summary>
    /// ���� ������ Ȱ��ȭ
    /// </summary>
    public void ActivateWeaponPrefab()
    {
        if (currentWeaponPrefab != null)
        {
            currentWeaponPrefab.SetActive(false);
        }

        currentWeaponPrefab = weaponsData[currentWeaponIndex].weaponinfo.modelPrefab;
        currentWeaponPrefab.SetActive(true);
        Debug.Log($"{currentWeaponPrefab}");
    }


    private void SwitchToWeapon(int index)
    {
        if (currentWeaponPrefab != null)
        {
            currentWeaponPrefab.SetActive(false);
        }

        currentWeaponPrefab = weaponsData[currentWeaponIndex].weaponinfo.modelPrefab;
        currentWeaponPrefab.SetActive(true);
        Debug.Log("���⸦ �����߽��ϴ�.");
    }
}