using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttack
{
    /// <summary>
    /// 공격력 프로퍼티
    /// </summary>
    uint AttackPower { get; }
}
