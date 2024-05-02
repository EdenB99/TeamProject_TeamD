    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponEquipManager : MonoBehaviour
{
    // ���� ����Ī ���� ----------------------------------------------------------------------------------
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
        // ��� ���⸦ ��Ȱ��ȭ
        SetWeaponsActive(false);
        // ó�� ���⸦ Ȱ��ȭ
        if (weapons.Count > 0)
            weapons[currentWeaponIndex].SetActive(true);   

        InitializeWeapons();
    }

    void InitializeWeapons()
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
        }
    }

    public void SwitchWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weapons.Count)
        {
            Debug.LogError("�߸��� ���� �ε����Դϴ�.");
            return;
        }

        // ���� ���� ��Ȱ��ȭ
        weapons[currentWeaponIndex].SetActive(false);
        Debug.Log("���繫�� ��Ȱ��ȭ");

        // �� ���� Ȱ��ȭ
        currentWeaponIndex = weaponIndex;
        weapons[currentWeaponIndex].SetActive(true);
        Debug.Log("�� ���� Ȱ��ȭ");
    }

    public void AddWeaponToInventory(GameObject weaponPrefab)
    {
        GameObject weaponInstance = Instantiate(weaponPrefab, transform);
        weapons.Add(weaponInstance);
        weaponInstance.SetActive(false); // ���� �߰��� ����� �⺻������ ��Ȱ��ȭ ���·� �߰�
    }

    /// <summary>
    /// ���⸦ ����Ī�ϴ� �Լ�
    /// </summary>
    /// <param name="context"></param>
    void SwitchWeapon(InputAction.CallbackContext context)
    {
        // ���� ���� ��Ȱ��ȭ
        weapons[currentWeaponIndex].SetActive(false);
        // ���� ���� �ε����� �̵�
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        // ���� ���� Ȱ��ȭ
        weapons[currentWeaponIndex].SetActive(true);
    }

    /// <summary>
    /// ��� ���⸦ Ȱ��ȭ/��Ȱ��ȭ�ϴ� �Լ�
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