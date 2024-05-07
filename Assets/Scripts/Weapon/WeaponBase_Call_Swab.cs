using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBase_Call_Swab : MonoBehaviour
{
    // 공격 스위칭 관련 ----------------------------------------------------------------------------------
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
    /// 무기 스위칭 함수
    /// </summary>
    /// <param name="context"></param>
    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (weaponsData.Count == 0)
        {
            Debug.LogWarning("인벤토리에 무기가 없습니다.");
            return;
        }

        currentWeaponIndex = (currentWeaponIndex + 1) % weaponsData.Count;
        SwitchToWeapon(currentWeaponIndex);
        Debug.Log($"{currentWeaponPrefab}");
    }

    /// <summary>
    /// 무기 프리팹 활성화
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
        Debug.Log("무기를 스왑했습니다.");
    }
}