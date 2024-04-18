using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �� ������ ������ �����ϴ� ��ũ���ͺ� ������Ʈ
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Weapon Effect Data", order = 2)]
public class WeaponEffectData : ScriptableObject
{
    [Header("���� ����Ʈ ����")]
    public WeaponEffectType weaponType;
    public EffectCode weaponEffectCode;
    public GameObject effectPrefab;
}
