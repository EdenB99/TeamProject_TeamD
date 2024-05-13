using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    /// <summary>
    /// 직선으로 이동하는 불릿 타입
    /// </summary>
    Bullet_Straight,

    /// <summary>
    /// 플레이어를 쫓아 이동하는 불릿타입
    /// </summary>
    Bullet_Chase,

    /// <summary>
    /// 0~360을 받아 이동하는 불릿타입
    /// </summary>
    Bullet_Straight_Dir,
}
