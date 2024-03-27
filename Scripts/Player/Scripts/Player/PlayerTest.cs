using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playertest : PlayerStats
{
    /// <summary>
    /// 맞았을 때 무적시간
    /// </summary>
    public float invincibleTime = 2.0f;
    
    private void Awake()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(10.0f);
            //StartCoroutine(InvinvibleMode());   // 무적 코루틴
        }
    }

    public void TakeDamage(float damage)
    {
        _hp -= damage;  // 체력감소

        if (_hp <= 0f)
        {

        }
    }

    /// <summary>
    /// 무적 코루틴
    /// </summary>
    /// <returns></returns>
    /*IEnumerator InvinvibleMode()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible"); // 레이어를 무적 레이어로 변경

        float timeElapsed = 0.0f;
        while (timeElapsed < invincibleTime) // 2초동안 계속하기
        {
            timeElapsed += Time.deltaTime;

            float alpha = (Mathf.Cos(timeElapsed * 30.0f) + 1.0f) * 0.5f;   // 코사인 결과를 1 ~ 0 사이로 변경
            spriteRenderer.color = new Color(1, 1, 1, alpha);               // 알파에 지정(깜박거리게 된다.)

            yield return null;
        }

        // 2초가 지난후
        gameObject.layer = LayerMask.NameToLayer("Player"); // 레이어를 다시 플레이어로 되돌리기
        spriteRenderer.color = Color.white;                 // 알파값도 원상복구

    }*/
}
