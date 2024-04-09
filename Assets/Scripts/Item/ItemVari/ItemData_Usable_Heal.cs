using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Object/Item Usable Data", order = 4)]
public class ItemData_Usable_Heal : ItemData, IUsable
{
    public uint healPoint;
    

    public bool Use()
    {
        Player player = GameManager.Instance.Player;
            
        player.PlayerStats.TakeHeal(healPoint);

        return true;
    }
}