using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossStageScripts : MonoBehaviour

{
    public Animator animator;

    public new CapsuleCollider2D collider;

    [SerializeField] private GameObject bossBoundaryObject;
    [SerializeField] private GameObject portalBoundaryObject;

    [SerializeField] private GameObject bossPrefab;
    private GameObject boss;

    private TextMeshPro bossInfo;
    private Transform bossTransform;
    private MainCamera mainCamera;
    private Player player;
    private bool IsBossOn => boss != null;
    private bool IsStartBattle = false;
    private bool isDoorOpen = false;
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

    private void Start()
    {
        mainCamera = FindAnyObjectByType<MainCamera>();
        player = GameObject.FindAnyObjectByType<Player>();
    }
    private void OnEnable()
    {
        collider.enabled = true;
        bossInfo.alpha = 0.0f;
        //카메라경계변경
        mainCamera.UpdateBoundaryObject(bossBoundaryObject);
    }

    private void Update()
    {
        //TODO:: 보스 죽으면 작동하도록 할것, 물어보고 코드수정하기, 보스전 종료 연출도 간단하게 있었으면 좋겠음.
        if (!IsBossOn && IsStartBattle && !isDoorOpen)
        {
            animator.SetBool("IsClose", false);
            mainCamera.UpdateBoundaryObject(portalBoundaryObject);
            isDoorOpen = true;
        }
    }

    //보스전 시작,연출용 코드 ==========================================

    //보스전 시작용 콜라이더가 플레이어와 충돌시
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("충돌함");
            collider.enabled = false;
            BossSpawn();

        }
    }

    //보스 스폰
    private void BossSpawn()
    {
        boss = Instantiate(bossPrefab);
        //z축을 제외한 보스트랜스폼의x,y위치에 생성
        boss.transform.position = new Vector3(bossTransform.transform.position.x, bossTransform.transform.position.y - 1f, 0.0f);
        boss.SetActive(true);
        StartCoroutine(StartBossCoroutine());
    }


    //보스전 시작연출용 코루틴
    private IEnumerator StartBossCoroutine()
    {
        float time = 0.0f;
        //플레이어 멈추고 6초에 걸쳐 보스를 보여주고 보스이름을 페이드 인, 아웃시키기
        player.enabled = false;
        IsStartBattle = true;
        yield return new WaitForSeconds(1f);
        mainCamera.enabled = false;
        while (time < 2f)
        {
            time += Time.deltaTime;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, bossTransform.transform.position, Time.deltaTime * 5);
            bossInfo.alpha += Time.deltaTime;
            bossInfo.rectTransform.Translate(0.2f * Time.deltaTime * Vector2.right);
            yield return null;
        }
        while (time < 3f)
        {
            time += Time.deltaTime;
            bossInfo.alpha += Time.deltaTime;
            bossInfo.rectTransform.Translate(0.2f * Time.deltaTime * Vector2.right);
            yield return null;
        }
        bossInfo.alpha = 1f;
        while (time < 5f)
        {
            time += Time.deltaTime;
            bossInfo.rectTransform.Translate(0.1f * Time.deltaTime * Vector2.right);
            bossInfo.alpha -= Time.deltaTime * 0.8f;
            yield return null;

        }
        player.enabled = true;
        mainCamera.enabled = true;
        animator.SetBool("IsClose", true);



    }

}

