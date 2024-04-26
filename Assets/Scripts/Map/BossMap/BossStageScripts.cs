using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossStageScripts : MonoBehaviour

{
    //TODO:: public �����, ��ũ��Ʈ �ϼ�,
    public Animator animator;

    public new CapsuleCollider2D collider;

    public GameObject bossPrefab;
    public TextMeshPro bossInfo;
    public Transform bossTransform;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        collider = child.GetComponent<CapsuleCollider2D>();

        child = transform.GetChild(1);
        bossInfo = child.GetComponent<TextMeshPro>();
        child = transform.GetChild(2);
        animator = child.GetComponent<Animator>();
        child = transform.GetChild(3);
        bossTransform = child.GetComponent<Transform>();
    }
    private void OnEnable()
    {
        collider.enabled = true;
        bossInfo.alpha = 0.0f;


    }

    //AwakeAction�����ϱ�
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = GameObject.FindAnyObjectByType<Player>();
            Debug.Log("�浹��");
            collider.enabled = false;
            //�÷��̾� �̵� ����
            Instantiate(bossPrefab, bossTransform);
            StartCoroutine(StartBossCoroutine(collider));

        }
    }

    private IEnumerator StartBossCoroutine(Collider2D collider)
    {
        float time = 0.0f;
        yield return new WaitForSeconds(1f);

        while (time < 3f)
        {
            time += Time.deltaTime;
            bossInfo.alpha += Time.deltaTime;
            bossInfo.rectTransform.Translate(Vector2.right * Time.deltaTime);
            animator.SetBool("IsClose", true);

        }
        bossInfo.alpha -= Time.deltaTime;

        yield return new WaitForSeconds(1f);

        collider.enabled = true;




    }

}

