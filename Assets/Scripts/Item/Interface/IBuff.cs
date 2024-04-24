using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuff
{
    /// <summary>
    /// 버프 함수 , 버프 지속시간을 리턴한다. ( -1 이면 영구 )
    /// </summary>
    float BuffActive();

}
