using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Bullet = 0,   
}

public class Factory : Singleton<Factory>
{
    /// <summary>
    /// 아이템을 드랍할때 아이템이 무작위로 이동되는 거리
    /// </summary>
    public float xNoise = 0.5f;

    BulletPool bulletPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletPool = GetComponentInChildren<BulletPool>();
        if (bulletPool != null) bulletPool.Initialize();
    }
 
    /// <summary>
    /// 풀에 있는 게임 오브젝트 하나 가져오기
    /// </summary>
    /// <param name="type">가져올 오브젝트의 종류</param>
    /// <param name="position">오브젝트가 배치될 위치</param>
    /// <param name="angle">오브젝트의 초기 각도</param>
    /// <returns>활성화된 오브젝트</returns>
    public GameObject GetObject(PoolObjectType type, Vector3? position = null, Vector3? euler = null)
    {
        GameObject result = null;
        //switch (type)
        //{
        //    //case PoolObjectType.Slime:
        //        //result = slimePool.GetObject(position, euler).gameObject;
        //    //    break;
        //}

        return result;
    }

    /// <summary>
    /// 불렛을 하나 생성하는 함수
    /// </summary>
    /// <param name="code">생성할 아이템의 코드</param>
    /// <returns>아이템의 게임 오브젝트</returns>
    //public GameObject MakeItem(ItemCode code)
    //{
    //    ItemData data = GameManager.Instance.ItemData[code];    // 아이템 데이터 받아오고
    //    ItemObject obj = itemPool.GetObject();
    //    obj.ItemData = data;                    // 풀에서 하나 꺼내고 데이터 설정

    //    return obj.gameObject;
    //}


    public GameObject MakeBullet(Bullet_Base bullet)
    {

        return bullet.gameObject;
    }




}