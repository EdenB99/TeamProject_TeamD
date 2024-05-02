using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Equip_Switching : TestBase
{
    [SerializeField] List<GameObject> weapons = new List<GameObject>();
    int currentWeaponIndex = 0;


    // 1번 슬롯 장착
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        if (weapons.Count == 0)
        {
            // blade 프리팹을 가져와서 활성화
            GameObject bladePrefab = Resources.Load<GameObject>("Prefabs/Blade"); // 프리팹 경로에 따라 수정
            Debug.Log("프리팹퐐성화");
            GameObject bladeInstance = Instantiate(bladePrefab, transform.position, transform.rotation, transform);
            Debug.Log("프리팹 생성");

            // 현재 무기 리스트에 추가
            weapons.Add(bladeInstance);

            // 현재 무기 인덱스 설정
            currentWeaponIndex = 0;

            // 새로운 무기 활성화
            weapons[currentWeaponIndex].SetActive(true);
        }
    }

    // 2번 슬롯 장착
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        if (weapons.Count == 0)
        {
            // spear 프리팹을 가져와서 활성화
            GameObject spearPrefab = Resources.Load<GameObject>("Prefabs/Spear"); // 프리팹 경로에 따라 수정
            GameObject spearInstance = Instantiate(spearPrefab, transform.position, transform.rotation, transform);

            // 현재 무기 리스트에 추가
            weapons.Add(spearInstance);

            // 현재 무기 인덱스 설정
            currentWeaponIndex = 0;

            // 새로운 무기 활성화
            weapons[currentWeaponIndex].SetActive(true);
        }
    }

    // 무기 스위칭
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        SwitchWeapons();
    }

    void SwitchWeapons()
    {
        // 현재 무기 비활성화
        weapons[currentWeaponIndex].SetActive(false);
        // 다음 무기 인덱스로 이동
        currentWeaponIndex = (currentWeaponIndex + 1) % weapons.Count;
        // 다음 무기 활성화
        weapons[currentWeaponIndex].SetActive(true);
    }
}
