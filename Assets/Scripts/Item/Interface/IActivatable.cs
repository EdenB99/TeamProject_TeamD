using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivatable
{
    /// <summary>
    /// 사용 함수
    /// </summary>
    bool ItemActive(Vector2 pos);

}
