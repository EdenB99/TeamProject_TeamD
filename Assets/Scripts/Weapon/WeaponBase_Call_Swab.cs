    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// ������ �߰�����
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
/// ����Ʈ�� �߰�����
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
    // ���� ����Ī ���� ----------------------------------------------------------------------------------
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
        // ��� ���⸦ ��Ȱ��ȭ ���·� �ʱ�ȭ
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // ù ��° ���⸦ Ȱ��ȭ
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
        Debug.Log("�κ��丮�� ���� �߰�");
        weaponInstance.SetActive(false); // ���� �߰��� ����� �⺻������ ��Ȱ��ȭ ���·� �߰�
    }

    /// <summary>
    /// ���⸦ ����Ī�ϴ� �Լ�
    /// </summary>
    /// <param name="context"></param>
    public void SwitchWeapon(InputAction.CallbackContext context)
    {
        if (weapons.Count == 0)
        {
            Debug.LogWarning("There are no weapons in the inventory.");
            return;
        }
        // ���� ���� ��Ȱ��ȭ
        weapons[currentWeaponIndex].SetActive(false);
        Debug.Log("���� ���� ��Ȱ��ȭ");
        // ���� ���� �ε����� �̵�
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        Debug.Log("���� ���� �ε����� �̵�");
        // ���� ���� Ȱ��ȭ
        weapons[currentWeaponIndex].SetActive(true);
        Debug.Log("���� ���� Ȱ��ȭ");

        currentWeaponPrefab = weapons[currentWeaponIndex];

        ActivateWeaponPrefab();
        Debug.Log("������ Ȱ��ȭ");
        Debug.Log($"{currentWeaponPrefab}");
    }

    public void ActivateWeaponPrefab()
    {
        // ���� ���� ������ ��Ȱ��ȭ
        if (currentWeaponPrefab != null)
        {
            currentWeaponPrefab.SetActive(false);
        }

        // ���ο� ���� ������ Ȱ��ȭ
        currentWeaponPrefab = weapons[currentWeaponIndex];
        currentWeaponPrefab.SetActive(true);
    }
}