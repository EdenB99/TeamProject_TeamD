using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff<T>
{
    /// <summary>
    /// ���� �Լ�
    /// </summary>
    float BuffActive(T target);

    /// <summary>
    /// ���� �Լ�
    /// </summary>
    void BuffEnd(T target);
}
