    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponEquipManager : MonoBehaviour
{
    // 공격 스위칭 관련 ----------------------------------------------------------------------------------
    [SerializeField] List<GameObject> weapons = new List<GameObject>();
    int currentWeaponIndex = 0;

    private int weaponIndex = 0;

    PlayerAction inputActions;

    private void Awake()
    {
        inputActions = GetComponent<PlayerAction>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.SwitchWeapon.performed += SwitchWeapon;

    }

    private void OnDisable()
    {
        inputActions.Player.SwitchWeapon.performed -= SwitchWeapon;
        inputActions.Player.Disable();
     }

    void Start()
    {
        // 모든 무기를 비활성화
        SetWeaponsActive(false);
        // 처음 무기를 활성화
        if (weapons.Count > 0)
            weapons[currentWeaponIndex].SetActive(true);   

        InitializeWeapons();
    }

    void InitializeWeapons()
    {
        // 모든 무기를 비활성화 상태로 초기화
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // 첫 번째 무기를 활성화
        if (weapons.Count > 0)
        {
            weapons[0].SetActive(true);
        }
    }

    public void SwitchWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weapons.Count)
        {
            Debug.LogError("잘못된 무기 인덱스입니다.");
            return;
        }

        // 현재 무기 비활성화
        weapons[currentWeaponIndex].SetActive(false);
        Debug.Log("현재무기 비활성화");

        // 새 무기 활성화
        currentWeaponIndex = weaponIndex;
        weapons[currentWeaponIndex].SetActive(true);
        Debug.Log("새 무기 활성화");
    }

    public void AddWeaponToInventory(GameObject weaponPrefab)
    {
        GameObject weaponInstance = Instantiate(weaponPrefab, transform);
        weapons.Add(weaponInstance);
        weaponInstance.SetActive(false); // 새로 추가된 무기는 기본적으로 비활성화 상태로 추가
    }

    /// <summary>
    /// 무기를 스위칭하는 함수
    /// </summary>
    /// <param name="context"></param>
    void SwitchWeapon(InputAction.CallbackContext context)
    {
        // 현재 무기 비활성화
        weapons[currentWeaponIndex].SetActive(false);
        // 다음 무기 인덱스로 이동
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        // 다음 무기 활성화
        weapons[currentWeaponIndex].SetActive(true);
    }

    /// <summary>
    /// 모든 무기를 활성화/비활성화하는 함수
    /// </summary>
    /// <param name="isActive"></param>
    void SetWeaponsActive(bool isActive)
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(isActive);
        }
    }

}