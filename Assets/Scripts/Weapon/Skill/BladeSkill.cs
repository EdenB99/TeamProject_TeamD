using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeSkill : MonoBehaviour, IAttack
{
    Player player;
    PlayerStats stats;
    public float skillPower = 5f;

    public uint AttackPower => (uint)(skillPower);

    public float skillDuration = 3.0f;

    private void Awake()
    {
        player = GameManager.Instance.Player;
        stats = player.PlayerStats;
    }

    public void Start()
    {
        StartCoroutine(DeActivateSkill());
    }

    public IEnumerator DeActivateSkill()
    {
        yield return new WaitForSeconds(skillDuration);
        Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            stats.AttackPower = AttackPower;
        }
    }
}
