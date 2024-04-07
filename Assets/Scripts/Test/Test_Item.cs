using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Item : TestBase
{
    GameObject obj;
    public GameObject mob;

    FlyEnemy_C fly;

    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
        fly = mob.GetComponent<FlyEnemy_C>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        obj = Factory.Instance.MakeItem(ItemCode.HealingPotion_A);
        obj.transform.position = transform.position;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.PlayerStats.CurrentHp -= 10;
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        fly.Die();
        
    }

}
