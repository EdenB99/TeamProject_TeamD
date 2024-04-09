using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Field", menuName = "Scriptable Object/Item Field Data", order = 2)]
public class ItemData_Field_Heal : ItemData, IConsume
{
    [Header("즉발 아이템 데이터")]
    public uint healPoint = 10;

    public void Consume()
    {
        Player player = GameManager.Instance.Player;
        if ( player != null )
        {
            player.PlayerStats.TakeHeal(healPoint);
        }

    }
}