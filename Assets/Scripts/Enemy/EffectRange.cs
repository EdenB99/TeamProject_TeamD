using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectRange : MonoBehaviour, IAttack
{
    public uint damage = 0;
    public uint AttackPower => damage;
}
