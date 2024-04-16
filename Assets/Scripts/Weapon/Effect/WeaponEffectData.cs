using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 한 종류의 정보를 저장하는 스크립터블 오브젝트
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Weapon Effect Data", order = 1)]
public class WeaponEffectData : ItemData
{
    [Header("무기 이펙트 정보")]
    public WeaponType weaponType;
    public GameObject effectPrefab;
}
