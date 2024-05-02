using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextPool : ObjectPool<DamageText>
{
    /// <summary>
    /// ������ �ؽ�Ʈ ���� ���� �Լ�
    /// </summary>
    /// <param name="damage">���� ������</param>
    /// <param name="position">������ ��ġ</param>
    /// <returns></returns>
    public GameObject GetObject(int damage, Vector2 position)
    {
        DamageText damageText = GetObject(position);
        damageText.SetDamage(damage);
        return damageText.gameObject;
    }

}
