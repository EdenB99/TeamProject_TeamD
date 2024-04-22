using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff<T>
{
    /// <summary>
    /// 버프 함수
    /// </summary>
    float BuffActive(T target);

    /// <summary>
    /// 버프 함수
    /// </summary>
    void BuffEnd(T target);
}
