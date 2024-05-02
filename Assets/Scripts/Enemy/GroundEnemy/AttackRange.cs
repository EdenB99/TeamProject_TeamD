using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    EnemyBase_ parent;
    int attackPower;
    Player player;

    private void Awake()
    {
        parent = FindAnyObjectByType<EnemyBase_>();
        player = GameManager.Instance.Player;
        attackPower = parent.mobDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack();
            Debug.Log($"{attackPower}만큼 피해를 입었고 {player.PlayerStats.CurrentHp}남았습니다");
        }
    }

    void Attack()
    {
        player.PlayerStats.TakeDamage(attackPower);
    }
}
