using System;
using System.Collections.Generic;
using System.Text;

public interface IWeapon : IEquipable
{
    int GetWeaponDamage();
    float GetWeaponSpeed();

    float GetEffectSpeed();
}