using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : RecycleObject
{
    TextMeshPro damageText;

    /// <summary>
    /// 전체 재생 시간
    /// </summary>
    public float duration = 2.0f;

    /// <summary>
    /// 현재 진행 시간
    /// </summary>
    float elapsedTime = 0.0f;

    /// <summary>
    /// 알파 값
    /// </summary>
    float alpha = 1.0f;

    float fontSize = 0.6f; 

    private void Awake()
    {
        damageText = GetComponent<TextMeshPro>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // 각종 초기화
        alpha = 1.0f;
        elapsedTime = 0.0f;                         // 진행시간 초기화
        transform.localScale = Vector3.one;         // 스케일 초기화
        transform.rotation = Quaternion.identity;   // 회전 초기화
        transform.Rotate(0, 0, 30);                 // 30도 회전시켜서 시작
        transform.position += new Vector3(0, 0.3f, -0.1f);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float timeRatio = elapsedTime / duration;

        // 문자 회전 / 정지
        if ( transform.rotation.z > 0 )
        {
            transform.Rotate(0, 0, -1);
        }

        if ( timeRatio < 1 - fontSize )
        {
            transform.localScale = new( 1 - timeRatio, 1 - timeRatio, 1 - timeRatio); // 스케일 설정
        }
        else
        {
            alpha -= Time.deltaTime;
        }

        damageText.color = new Color(1, 1, 1, alpha);  // 색상 설정


        if (alpha < 0.1f)        // 진행시간이 다되면
        {
            gameObject.SetActive(false);    // 스스로 비활성화
        }
    }

    /// <summary>
    /// 출력될 숫자 설정
    /// </summary>
    /// <param name="damage">출력될 데미지</param>
    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
}
