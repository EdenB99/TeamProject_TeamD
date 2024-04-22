using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    /// <summary>
    /// 버프 함수
    /// </summary>
    float BuffActive();

    /// <summary>
    /// 버프 함수
    /// </summary>
    void BuffEnd();
}
