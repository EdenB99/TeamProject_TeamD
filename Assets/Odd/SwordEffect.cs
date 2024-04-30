using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEffect : MonoBehaviour
{
    public float damageMultiplier = 1.0f;   // ������ ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStats playerstats = FindObjectOfType<PlayerStats>();
        float baseDamage = playerstats.AttackPower;

        // ũ��Ƽ�� ����� ���� ����
        /*   float critical = playerstats.criticalChance;
           bool criticalHit = false;

           float randomChance = Random.Range(0, 100);
           if (critical >= randomChance)
           {
               criticalHit = true;
               baseDamage *= 1.1f; // ũ�������� 1.1������ ����
           }*/
        //float damage = baseDamage * damageMultiplier; // ������ ���� ����


        float damage = baseDamage;
       
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<IEnemy>().TakeDamage(damage);
            
        }
    }



}
