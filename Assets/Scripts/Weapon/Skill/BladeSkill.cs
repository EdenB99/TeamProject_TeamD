using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeSkill : MonoBehaviour, IAttack
{
    Player player;
    PlayerStats stats;
    public float skillPower = 5f;

    public uint AttackPower => (uint)(skillPower);

    private void Awake()
    {
        player = GameManager.Instance.Player;
        stats = player.PlayerStats;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            stats.AttackPower = AttackPower;
        }
    }
}
