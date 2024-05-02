using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDataManager : MonoBehaviour
{
    public ItemData_Weapon[] itemDatas = null;

    public ItemData_Weapon this[WeaponCode code] => itemDatas[(int)code];
    public ItemData_Weapon this[int index] => itemDatas[index];
}