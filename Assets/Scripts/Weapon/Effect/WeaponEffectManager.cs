using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffectDataManager : MonoBehaviour
{
    public WeaponEffectData[] weaponEffectDatas = null;

    public WeaponEffectData this[WeaponEffectType code] => weaponEffectDatas[(int)code];

    public WeaponEffectData this[int index] => weaponEffectDatas[index];

    private WeaponEffectData GetWeaponEffectData(WeaponEffectType type)
    {
        foreach (var data in weaponEffectDatas)
        {
            if (data.weaponType == type)
            {
                return data;
            }
        }
        return null;
    }
}
