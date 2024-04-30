using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEffect : MonoBehaviour
{
    public float damageMultiplier = 1.0f;   // 데미지 배율

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerstats = FindObjectOfType<PlayerStats>();
        float baseDamage = playerstats.AttackPower;

        // 크리티컬 대미지 구현 로직
        /*   float critical = playerstats.criticalChance;
           bool criticalHit = false;

           float randomChance = Random.Range(0, 100);
           if (critical >= randomChance)
           {
               criticalHit = true;
               baseDamage *= 1.1f; // 크리데미지 1.1데미지 증가
           }*/
        //float damage = baseDamage * damageMultiplier; // 데미지 배율 적용


        float damage = baseDamage;
       
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IEnemy>().TakeDamage(damage);
            
        }
    }



}
