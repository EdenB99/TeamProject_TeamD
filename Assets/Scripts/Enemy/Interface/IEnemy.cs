using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적이라면 반드시 가지고 있는 변수, 메서드들
/// </summary>
public interface IEnemy
{
    /// <summary>
    /// HP 프로퍼티
    /// </summary>
    float HP { get; set; }

    /// <summary>
    /// 최대 HP 프로퍼티
    /// </summary>
    float MaxHP { get; }

    /// <summary>
    /// 사망 델리게이트 프로퍼티
    /// </summary>
    Action onDie { get; set; }

    /// <summary>
    /// 적의 공격 메서드
    /// </summary>
    void Attack();

    /// <summary>
    /// 적이 피해를 받는 메서드
    /// </summary>
    /// <param name="damage">들어온 데미지</param>
    void TakeDamage(float damage);

    /// <summary>
    /// 사망 처리용 함수(메서드 method)
    /// </summary>
    void Die();
}
