using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    public ItemData[] itemDatas = null;
    public WeaponData[] WeaponDatas = null;
    public ItemData this[ItemCode code] => itemDatas[(int)code];
    public WeaponData this[WeaponCode code] => WeaponDatas[(int)code];
    public ItemData this[int index] => itemDatas[index];
    //public WeaponData this[int index] => WeaponDatas[index];
}
