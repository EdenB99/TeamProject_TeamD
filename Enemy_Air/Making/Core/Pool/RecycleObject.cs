using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleObject : MonoBehaviour
{
    /// <summary>
    /// 재활용 오브젝트가 비활성화 될 때 실행되는 델리게이트
    /// </summary>
    public Action onDisable;

    protected virtual void OnEnable()
    {
        StopAllCoroutines();        // 리셋
    }

    protected virtual void OnDisable()
    {
        onDisable?.Invoke();        // 비활성화 알림
    }


}
