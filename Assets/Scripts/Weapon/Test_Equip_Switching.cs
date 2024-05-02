using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Equip_Switching : TestBase
{
    [SerializeField] List<GameObject> weapons = new List<GameObject>();
    int currentWeaponIndex = 0;


    // 1�� ���� ����
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        if (weapons.Count == 0)
        {
            // blade �������� �����ͼ� Ȱ��ȭ
            GameObject bladePrefab = Resources.Load<GameObject>("Prefabs/Blade"); // ������ ��ο� ���� ����
            Debug.Log("�����սt��ȭ");
            GameObject bladeInstance = Instantiate(bladePrefab, transform.position, transform.rotation, transform);
            Debug.Log("������ ����");

            // ���� ���� ����Ʈ�� �߰�
            weapons.Add(bladeInstance);

            // ���� ���� �ε��� ����
            currentWeaponIndex = 0;

            // ���ο� ���� Ȱ��ȭ
            weapons[currentWeaponIndex].SetActive(true);
        }
    }

    // 2�� ���� ����
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        if (weapons.Count == 0)
        {
            // spear �������� �����ͼ� Ȱ��ȭ
            GameObject spearPrefab = Resources.Load<GameObject>("Prefabs/Spear"); // ������ ��ο� ���� ����
            GameObject spearInstance = Instantiate(spearPrefab, transform.position, transform.rotation, transform);

            // ���� ���� ����Ʈ�� �߰�
            weapons.Add(spearInstance);

            // ���� ���� �ε��� ����
            currentWeaponIndex = 0;

            // ���ο� ���� Ȱ��ȭ
            weapons[currentWeaponIndex].SetActive(true);
        }
    }

    // ���� ����Ī
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        SwitchWeapons();
    }

    void SwitchWeapons()
    {
        // ���� ���� ��Ȱ��ȭ
        weapons[currentWeaponIndex].SetActive(false);
        // ���� ���� �ε����� �̵�
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        // ���� ���� Ȱ��ȭ
        weapons[currentWeaponIndex].SetActive(true);
    }
}
