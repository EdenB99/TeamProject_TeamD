using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    /// <summary>
    /// 이 오브젝트가 가질 아이템 데이터
    /// </summary>
    ItemData data = null;

    /// <summary>
    /// 스프라이트 ( ItemData 에서 가져온다 )
    /// </summary>
    SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// 이미지 설정
    /// </summary>
    public ItemData ItemData
    {
        get => data;
        set
        {
            if (data == null)  
            {
                data = value;
                //아이콘 값을 바꾼다
                spriteRenderer.sprite = data.itemIcon;              
            }
        }
    }
}
