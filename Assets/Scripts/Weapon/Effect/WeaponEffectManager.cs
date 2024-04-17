using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffectDataManager : MonoBehaviour
{
    public WeaponEffectData[] weaponEffectDatas = null;

    public WeaponEffectData this[WeaponCode code] => weaponEffectDatas[(int)code];

    public WeaponEffectData this[int index] => weaponEffectDatas[index];

}
