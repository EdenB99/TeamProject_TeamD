using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Field", menuName = "Scriptable Object/Item Field Data", order = 2)]
public class ItemData_Field : ItemData
{
    [Header("즉발 아이템 데이터")]
    public uint heal;

    public void Consume(GameObject target)
    {

    }
}