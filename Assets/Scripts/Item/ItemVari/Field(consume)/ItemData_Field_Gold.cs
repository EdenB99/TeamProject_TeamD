using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data - Field", menuName = "Scriptable Object/Gold Field Data", order = 8)]
public class ItemData_Field_Gold : ItemData, IConsume
{
    [Header("얻게 될 금액")]
    public uint Itemgold = 100;

    public void Consume()
    {
        GameManager.Instance.GoldCountAdd((int)Itemgold);

        Player player = GameManager.Instance.Player;
        if (player != null)
        {
            player.Gold += Itemgold;
        }

    }
}