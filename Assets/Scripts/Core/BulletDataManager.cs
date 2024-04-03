using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDataManager : MonoBehaviour
{
    public BulletData[] bulletDatas = null;
   
    public BulletData this[BulletCode code] => bulletDatas[(int)code];
   
    public BulletData this[int index] => bulletDatas[index];
   
}
