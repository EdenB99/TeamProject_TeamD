using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    int attackPower;
    Player player;

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
            Debug.Log($"{attackPower}��ŭ ���ظ� �Ծ��� {player.PlayerStats.CurrentHp}���ҽ��ϴ�");
        }
    }

    void Attack()
    {
        player.PlayerStats.TakeDamage(attackPower);
    }
}
