using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 한 종류의 정보를 저장하는 스크립터블 오브젝트
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Weapon Data", order = 1)]
public class WeaponData : ItemData
{
    [Header("아이템 기본 정보")]
    // public ItemCode code;
    public uint maxStackCount = 1;

    [Header("무기 기본 정보")]
    public uint attackPower = 10;
    public float attackSpeed = 1.0f;
    public GameObject modelPrefab;
}
