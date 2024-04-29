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
        //ī�޶��躯��
        mainCamera.UpdateBoundaryObject(bossBoundaryObject);
    }

    private void Update()
    {
        //TODO:: ���� ������ �۵��ϵ��� �Ұ�, ����� �ڵ�����ϱ�, ������ ���� ���⵵ �����ϰ� �־����� ������.
        if (!IsBossOn && IsStartBattle && !isDoorOpen)
        {
            animator.SetBool("IsClose", false);
            mainCamera.UpdateBoundaryObject(portalBoundaryObject);
            isDoorOpen = true;
        }
    }

    //������ ����,����� �ڵ� ==========================================

    //������ ���ۿ� �ݶ��̴��� �÷��̾�� �浹��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�浹��");
            collider.enabled = false;
            BossSpawn();

        }
    }

    //���� ����
    private void BossSpawn()
    {
        boss = Instantiate(bossPrefab);
        //z���� ������ ����Ʈ��������x,y��ġ�� ����
        boss.transform.position = new Vector3(bossTransform.transform.position.x, bossTransform.transform.position.y - 1f, 0.0f);
        boss.SetActive(true);
        StartCoroutine(StartBossCoroutine());
    }


    //������ ���ۿ���� �ڷ�ƾ
    private IEnumerator StartBossCoroutine()
    {
        float time = 0.0f;
        //�÷��̾� ���߰� 6�ʿ� ���� ������ �����ְ� �����̸��� ���̵� ��, �ƿ���Ű��
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

