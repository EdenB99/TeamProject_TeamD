using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : RecycleObject
{
    /// <summary>
    /// 이 오브젝트가 가질 아이템 데이터
    /// </summary>
    public ItemData data = null;

    /// <summary>
    /// 스프라이트 ( ItemData 에서 가져온다 )
    /// </summary>
    SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        spriteRenderer = child.GetComponent<SpriteRenderer>();

        
    }

    protected override void OnEnable()
    {
        data = null;
        base.OnEnable();
    }

    public void itemDel()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    /// 흡수되는 코드

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
