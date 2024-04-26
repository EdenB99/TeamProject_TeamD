using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���̶�� �ݵ�� ������ �ִ� ����, �޼����
/// </summary>
public interface IEnemy
{
    /// <summary>
    /// HP ������Ƽ
    /// </summary>
    float HP { get; set; }

    /// <summary>
    /// �ִ� HP ������Ƽ
    /// </summary>
    float MaxHP { get; }

    /// <summary>
    /// ��� ��������Ʈ ������Ƽ
    /// </summary>
    Action onDie { get; set; }

    /// <summary>
    /// ���� ���� �޼���
    /// </summary>
    void Attack();

    /// <summary>
    /// ���� ���ظ� �޴� �޼���
    /// </summary>
    /// <param name="damage">���� ������</param>
    void TakeDamage(float damage);

    /// <summary>
    /// ��� ó���� �Լ�(�޼��� method)
    /// </summary>
    void Die();
}
