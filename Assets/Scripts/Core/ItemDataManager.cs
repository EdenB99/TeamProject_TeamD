using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    // 아이템 배열을 필드 / 소비 / 기타 / 무기 / 악세사리 별로 나눠야 하는지에대해 의논 필요

    public ItemData[] itemDatas;
    //public WeaponData[] WeaponDatas = null;
    public ItemData this[ItemCode code] => itemDatas[(int)code];
    //public WeaponData this[WeaponCode code] => WeaponDatas[(int)code];
    public ItemData this[int index] => itemDatas[index];
    //public WeaponData this[int index] => WeaponDatas[index];

}
