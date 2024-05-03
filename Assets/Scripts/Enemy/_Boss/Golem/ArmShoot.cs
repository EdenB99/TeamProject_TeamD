using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmShoot : MonoBehaviour
{
    Boss_Golem boss;
    int attackPower;
    Player player;

    private void Awake()
    {
        boss = FindAnyObjectByType<Boss_Golem>();
        player = GameManager.Instance.Player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Ground"))
        {
            boss.ReturnArmShootToPool(gameObject);
        }

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
