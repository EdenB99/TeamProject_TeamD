using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    // ������ �迭�� �ʵ� / �Һ� / ��Ÿ / ���� / �Ǽ��縮 ���� ������ �ϴ��������� �ǳ� �ʿ�

    public ItemData[] itemDatas;
    //public WeaponData[] WeaponDatas = null;
    public ItemData this[ItemCode code] => itemDatas[(int)code];
    //public WeaponData this[WeaponCode code] => WeaponDatas[(int)code];
    public ItemData this[int index] => itemDatas[index];
    //public WeaponData this[int index] => WeaponDatas[index];

}
