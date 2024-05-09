using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBase_Call_Swab : MonoBehaviour
{
    [SerializeField] List<ItemData_Weapon> weaponsData = new List<ItemData_Weapon>();

    int currentWeaponIndex = 0;

    PlayerAction inputActions;

    GameObject currentWeaponInstance; // 현재 씬에 생성된 무기 인스턴스

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
            Debug.LogWarning("인벤토리에 무기가 없습니다.");
            return;
        }

        // 다음 무기 인덱스로 변경
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponsData.Count;
        SwitchToWeapon(currentWeaponIndex);
    }

    public void ActivateWeaponPrefab(ItemData_Weapon weaponData)
    {
        // 현재 무기 인스턴스를 제거
        DestroyCurrentWeapon();

        // 새로운 무기 프리팹 생성
        GameObject weaponPrefab = Instantiate(weaponData.Weaponinfo.modelPrefab, transform.position, Quaternion.identity);
        currentWeaponInstance = weaponPrefab;
    }

    private void SwitchToWeapon(int index)
    {
        if (weaponsData.Count == 0)
        {
            Debug.LogWarning("인벤토리에 무기가 없습니다.");
            return;
        }

        // 현재 무기 인스턴스를 제거
        DestroyCurrentWeapon();

        // 다음 무기 프리팹 생성
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
        weaponsData.Add(itemData);
    }

    public void deleteWeaponData(ItemData_Weapon itemData)
    {
        weaponsData.Remove(itemData);
    }
}
