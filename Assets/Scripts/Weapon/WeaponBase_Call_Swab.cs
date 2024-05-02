    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 무기의 추가정보
/// </summary>
[System.Serializable]
public struct WeaponInfo
{
    public uint attackPower;
    public float attackSpeed;
    public GameObject modelPrefab;
    public WeaponType weaponType;
}

/// <summary>
/// 이펙트의 추가정보
/// </summary>
[System.Serializable]
public struct EffectInfo
{
    public uint effectSize;
    public float effectSpeed;
    public uint weaponDamage;
    public GameObject modelPrefab;
}

public class WeaponBase_Call_Swab : MonoBehaviour
{
    // 공격 스위칭 관련 ----------------------------------------------------------------------------------
    [SerializeField] List<GameObject> weapons = new List<GameObject>();
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
        // 모든 무기를 비활성화 상태로 초기화
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // 첫 번째 무기를 활성화
        if (weapons.Count > 0)
        {
            weapons[0].SetActive(true);
            currentWeaponPrefab = weapons[0];
            Debug.Log($"{currentWeaponPrefab}");
        }
    }

    public void AddWeaponToInventory(GameObject weaponPrefab)
    {
        GameObject weaponInstance = Instantiate(weaponPrefab, transform);
        weapons.Add(weaponInstance);
        Debug.Log("인벤토리에 무기 추가");
        weaponInstance.SetActive(false); // 새로 추가된 무기는 기본적으로 비활성화 상태로 추가
    }

    /// <summary>
    /// 무기를 스위칭하는 함수
    /// </summary>
    /// <param name="context"></param>
    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (weapons.Count == 0)
        {
            Debug.LogWarning("There are no weapons in the inventory.");
            return;
        }
        // 현재 무기 비활성화
        weapons[currentWeaponIndex].SetActive(false);
        Debug.Log("현재 무기 비활성화");
        // 다음 무기 인덱스로 이동
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        Debug.Log("다음 무기 인덱스로 이동");
        // 다음 무기 활성화
        weapons[currentWeaponIndex].SetActive(true);
        Debug.Log("다음 무기 활성화");

        currentWeaponPrefab = weapons[currentWeaponIndex];

        ActivateWeaponPrefab();
        Debug.Log("프리팹 활성화");
        Debug.Log($"{currentWeaponPrefab}");
    }

    public void ActivateWeaponPrefab()
    {
        // 기존 무기 프리팹 비활성화
        if (currentWeaponPrefab != null)
        {
            currentWeaponPrefab.SetActive(false);
        }

        // 새로운 무기 프리팹 활성화
        currentWeaponPrefab = weapons[currentWeaponIndex];
        currentWeaponPrefab.SetActive(true);
    }
}