using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �� ������ ������ �����ϴ� ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Weapon Effect Data", order = 1)]
public class WeaponEffectData : ItemData
{
    [Header("���� ����Ʈ ����")]
    public WeaponType weaponType;
    public GameObject effectPrefab;
}
