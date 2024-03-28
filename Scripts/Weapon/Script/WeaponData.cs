using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 한 종류의 정보를 저장하는 스크립터블 오브젝트
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Data", order = 0)]
public class WeaponData : ScriptableObject
{
    [Header("아이템 기본 정보")]
    public WeaponCode code;
    public string 아이템이름 = "아이템";
    public string 아이템설명 = "설명";
    public uint 무기공격력 = 10;
    public float 공격속도 = 1.0f;
    public Sprite itemIcon;
    public uint 가격 = 0;
    public uint 최대소지개수 = 1;
    public GameObject modelPrefab;
}
