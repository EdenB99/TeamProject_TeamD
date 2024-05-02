using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextPool : ObjectPool<DamageText>
{
    /// <summary>
    /// 데미지 텍스트 전용 리턴 함수
    /// </summary>
    /// <param name="damage">받은 데미지</param>
    /// <param name="position">생성될 위치</param>
    /// <returns></returns>
    public GameObject GetObject(int damage, Vector2 position)
    {
        DamageText damageText = GetObject(position);
        damageText.SetDamage(damage);
        return damageText.gameObject;
    }

}
