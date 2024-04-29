using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossStageScripts : MonoBehaviour

{
    //TODO:: public 지우기, 스크립트 완성,
    public Animator animator;

    public new CapsuleCollider2D collider;

    public GameObject bossPrefab;
    public TextMeshPro bossInfo;
    public Transform bossTransform;
    public MainCamera mainCamera;
    Player player;
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
        mainCamera = FindAnyObjectByType<MainCamera>();
        player = GameObject.FindAnyObjectByType<Player>();
    }

    //AwakeAction수정하기
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("충돌함");
            collider.enabled = false;
            //플레이어 이동 끄기
            Instantiate(bossPrefab);
            bossPrefab.transform.position = new Vector3(bossTransform.transform.position.x, bossTransform.transform.position.y-1f, 0.0f);
            bossPrefab.SetActive(true);
            StartCoroutine(StartBossCoroutine(collider));

        }
    }

    private IEnumerator StartBossCoroutine(Collider2D collider)
    {
        float time = 0.0f;
        player.enabled = false;

        yield return new WaitForSeconds(1f);
        mainCamera.enabled = false;
        while (time < 2f)
        {
            time += Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, bossTransform.transform.position, Time.deltaTime * 5);
            bossInfo.alpha += Time.deltaTime;
            bossInfo.rectTransform.Translate(Vector2.right * Time.deltaTime * 0.2f);
            yield return null;
        }
        while (time < 3f)
        {
            time += Time.deltaTime;
            bossInfo.alpha += Time.deltaTime;
            bossInfo.rectTransform.Translate(Vector2.right * Time.deltaTime * 0.2f);
            yield return null;
        }
        bossInfo.alpha = 1f;
        while (time < 5f)
        {
            time += Time.deltaTime;
            bossInfo.rectTransform.Translate(Vector2.right * Time.deltaTime * 0.1f);
            bossInfo.alpha -= Time.deltaTime;
            yield return null;

        }
        player.enabled = true;
        mainCamera.enabled = true;
        animator.SetBool("IsClose", true);



    }

}

