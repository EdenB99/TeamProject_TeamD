using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffectDataManager : MonoBehaviour
{
    public WeaponData[] weaponDatas = null;

    public WeaponData this[WeaponCode code] => weaponDatas[(int)code];

    public WeaponData this[int index] => weaponDatas[index];

}
