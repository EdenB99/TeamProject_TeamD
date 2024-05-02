using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : RecycleObject
{
    TextMeshPro damageText;

    /// <summary>
    /// ��ü ��� �ð�
    /// </summary>
    public float duration = 2.0f;

    /// <summary>
    /// ���� ���� �ð�
    /// </summary>
    float elapsedTime = 0.0f;

    /// <summary>
    /// ���� ��
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

        // ���� �ʱ�ȭ
        alpha = 1.0f;
        elapsedTime = 0.0f;                         // ����ð� �ʱ�ȭ
        transform.localScale = Vector3.one;         // ������ �ʱ�ȭ
        transform.rotation = Quaternion.identity;   // ȸ�� �ʱ�ȭ
        transform.Rotate(0, 0, 30);                 // 30�� ȸ�����Ѽ� ����
        transform.position += new Vector3(0, 0.3f, -0.1f);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float timeRatio = elapsedTime / duration;

        // ���� ȸ�� / ����
        if ( transform.rotation.z > 0 )
        {
            transform.Rotate(0, 0, -1);
        }

        if ( timeRatio < 1 - fontSize )
        {
            transform.localScale = new( 1 - timeRatio, 1 - timeRatio, 1 - timeRatio); // ������ ����
        }
        else
        {
            alpha -= Time.deltaTime;
        }

        damageText.color = new Color(1, 1, 1, alpha);  // ���� ����


        if (alpha < 0.1f)        // ����ð��� �ٵǸ�
        {
            gameObject.SetActive(false);    // ������ ��Ȱ��ȭ
        }
    }

    /// <summary>
    /// ��µ� ���� ����
    /// </summary>
    /// <param name="damage">��µ� ������</param>
    public void SetDamage(int damage)
    {
        damageText.text = damage.ToString();
    }
}
