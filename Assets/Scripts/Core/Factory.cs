using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Factory : Singleton<Factory>
{
    BulletPool bulletPool;
    ItemPool itemPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletPool = GetComponentInChildren<BulletPool>();
        if(bulletPool != null ) bulletPool.Initialize();

        itemPool = GetComponentInChildren<ItemPool>();
        if(itemPool != null ) itemPool.Initialize();
    }

    /// <summary>
    /// Bullet을 만드는 메서드
    /// </summary>
    /// <param name="position">위치 지정</param>
    /// <param name="Dir">방향지정</param>
    /// <param name="type">여기에 넣은 불릿 스크립트대로 이동한다.</param>
    /// <returns></returns>
    public GameObject MakeBullet(Vector2 position, Vector2 Dir, BulletCode code )
    {
        BulletObject obj = bulletPool.GetObject(position);
        BulletData data = GameManager.Instance.BulletData[code];
        obj.BulletData = data;
        obj.moveDir = Dir;          // 이동 방향 설정
        
        return obj.gameObject;
    }

    /// <summary>
    /// 아이템 단일 생성 메서드
    /// </summary>
    /// <param name="code">아이템 코드</param>
    /// <returns>아이템 오브젝트</returns>
    public GameObject MakeItem(ItemCode code)
    {
        ItemData data = GameManager.Instance.ItemData[code];    // 아이템 데이터 받기
        ItemObject obj = itemPool.GetObject();
        obj.ItemData = data;                    // 데이터 설정

        return obj.gameObject;
    }

    /// <summary>
    /// 아이템을 여러개 생성하는 함수
    /// </summary>
    /// <param name="code">아이템 코드</param>
    /// <param name="count">생성 개수</param>
    /// <returns>아이템 오브젝트들</returns>
    public GameObject[] MakeItems(ItemCode code, uint count)
    {
        GameObject[] items = new GameObject[count];
        for(int i = 0;i<count;i++)
        {
            items[i] = MakeItem(code);
        }
        return items;
    }


}