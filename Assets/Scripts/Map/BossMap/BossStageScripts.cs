using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossStageScripts : MonoBehaviour
        
{
    //TODO:: public �����, ��ũ��Ʈ �ϼ�,
    public CapsuleCollider2D collider;

   public Boss_Knight boss;

 public    Animator animator;

    public TextMeshPro bossInfo;

    
    private void Awake()
    {
        Transform child = transform.GetChild(0);
        collider = child.GetComponent<CapsuleCollider2D>();
        boss = GameObject.FindAnyObjectByType<Boss_Knight>();
        child = transform.GetChild(1);
        bossInfo = child.GetComponent<TextMeshPro>();
        child = transform.GetChild(2);
        animator = child.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        collider.enabled = true;
        boss.gameObject.SetActive(false);
        bossInfo.alpha = 0.0f;
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�΋H��");
            Player player = GameObject.FindAnyObjectByType<Player>();
            //collider.enabled = false;
            //StartCoroutine(StartBossCoroutine(collider));

        }
    }

    IEnumerable StartBossCoroutine(Collider2D collider)
    {
        boss.gameObject.SetActive(true);
        bossInfo.alpha += Time.deltaTime;
        animator.SetBool("IsClose", true);
        yield return new WaitForSeconds(3);
        bossInfo.alpha -= Time.deltaTime;
        collider.enabled = true;




    }

}

