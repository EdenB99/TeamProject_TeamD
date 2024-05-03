using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Golem : PatternEnemyBase
{
    float intervalpattern1 = 2f;
    float intervalpattern2 = 1.0f;
    public float intervalAfterTeleport = 2f; // 레이저 발사 후 순간이동까지의 시간 간격
    public float teleportDistance = 2f; // 플레이어 뒤로 순간이동할 거리

    readonly int isCast_Hash = Animator.StringToHash("isCast");
    readonly int isShoot_Hash = Animator.StringToHash("isShoot");

    float TimeElapsed = 0;
    public float PatternTime = 5;

    bool startFight = false;

    Transform laser;
    Transform armshoot;
    public GameObject armShootPrefab;

    protected override void Awake()
    {
        base.Awake();
        laser = transform.GetChild(0);
        armshoot = transform.GetChild(1);
    }

    protected override void Start()
    {
        base.Start();
        State = BossState.Wait;
        StartCoroutine(AwakeAction());
        InitializeArmShootPool();
    }

    protected override void Update_Wait()
    {
        if (startFight)
        {
            if (Mathf.Abs(transform.position.y - playerPos.y) < 3.0 && Mathf.Abs(transform.position.x - playerPos.x) > 0.2)
            {
                State = BossState.Chase;
            }
            if (TimeElapsed < PatternTime)
            {
                TimeElapsed += Time.deltaTime;
            }
            else
            {
                //State = BossState.SpAttack;
                TimeElapsed = 0;
            }
        }
    }

    protected override void State_Chase()
    {
        TeleportBehindPlayer();
    }

    protected override void Update_Chase()
    {
        if (playerCheck())
        {
            State = BossState.SpAttack;
            TimeElapsed = 0;
        }

        if (TimeElapsed < PatternTime)
        {
            TimeElapsed += Time.deltaTime;
        }
        else
        {
            State = BossState.SpAttack;

            TimeElapsed = 0;
        }
    }

    protected override void State_SpAttack()
    {
        StartCoroutine(bossActionSelect());
    }

    protected override IEnumerator AwakeAction()
    {
        animator.SetTrigger(isCast_Hash);
        yield return new WaitForSeconds(intervalpattern1);
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        laser.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        startFight = true;
        State = BossState.Chase;
    }

    protected override void InitializePatterns()
    {
        patternActions = new Dictionary<uint, Func<IEnumerator>>()
        {
            { 1, BossPattern_1 },
            {2, BossPattern_2 },
            {3, BossPattern_3 }

        };
    }

    // 패턴 --------------------------------------------------------------------------------------------

    /// <summary>
    /// 패턴1 : 레이저를 쏘고 플레이어 뒤로 순간이동 한 뒤 2초후에 레이저 또 발사
    /// </summary>
    /// <returns></returns>
    new IEnumerator BossPattern_1()
    {
        StartCoroutine(FireLaserCoroutine());
        TeleportBehindPlayer();

        yield return new WaitForSeconds(intervalAfterTeleport);

        // 순간이동 후 레이저 다시 발사
        StartCoroutine(FireLaserCoroutine());

        yield return new WaitForSeconds(intervalpattern2);
        State = BossState.Chase;
    }

    /// <summary>
    /// 레이저 발사 
    /// </summary>
    /// <returns></returns>
    IEnumerator FireLaserCoroutine()
    {
        animator.SetTrigger(isCast_Hash);
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(intervalpattern1);
        laser.gameObject.SetActive(false);
    }

    /// <summary>
    /// 플레이어 뒤로 순간이동하는 함수
    /// </summary>
    void TeleportBehindPlayer()
    {
        Vector2 playerDirection = CheckLR == 1 ? Vector2.right : Vector2.left;
        Vector2 teleportPosition = (Vector2)player.transform.position + playerDirection * teleportDistance;

        teleportPosition.y = transform.position.y;

        float distanceSqr = (teleportPosition - (Vector2)player.transform.position).sqrMagnitude;

        transform.position = teleportPosition;
    }

    /// <summary>
    /// 패턴 2 : 보스의 팔을 발사하는 패턴
    /// </summary>
    /// <returns></returns>
    IEnumerator BossPattern_2()
    {
        StartCoroutine(ArmShootingCoroutine());
        yield return new WaitForSeconds(intervalpattern2);
        State = BossState.Chase;
    }

    IEnumerator ArmShootingCoroutine()
    {
        animator.SetTrigger(isShoot_Hash);
        yield return new WaitForSeconds(intervalpattern2);
        FireArmshoot();
    }

    /// <summary>
    /// 발사 함수
    /// </summary>
    void FireArmshoot()
    {
        GameObject projectile = GetArmShootFromPool();
        projectile.transform.position = armshoot.position;
        projectile.transform.rotation = Quaternion.identity;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        Vector2 shootDirection = -transform.right * CheckLR;
        float shootSpeed = 10f;

        rb.velocity = shootDirection * shootSpeed;
    }

    /// <summary>
    /// 패턴 3: 레이저를 쏜 다음 플레이어 근처로 이동해 팔을 발사하는 패턴
    /// </summary>
    /// <returns></returns>
    IEnumerator BossPattern_3()
    {
        animator.SetTrigger(isCast_Hash);
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(intervalpattern1);
        laser.gameObject.SetActive(false);
        TeleportBehindPlayer();

        yield return new WaitForSeconds(intervalAfterTeleport);

        animator.SetTrigger(isShoot_Hash);
        yield return new WaitForSeconds(intervalpattern2);

        FireMultiple();

        yield return new WaitForSeconds(intervalpattern2);
        State = BossState.Chase;
    }

    /// <summary>
    /// 팔을 여러개 날리는 패턴 함수
    /// </summary>
    void FireMultiple()
    {
        float[] angles = { -30f, 0f, 30f };

        foreach (float angle in angles)
        {
            FireArmShootAngle(angle);
        }
    }

    /// <summary>
    /// 기본 발사함수
    /// </summary>
    /// <param name="angle">날리는 각도</param>
    void FireArmShootAngle(float angle)
    {
        GameObject projectile = GetArmShootFromPool();
        projectile.transform.position = armshoot.position;
        projectile.transform.rotation = Quaternion.identity;
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // 기본 발사 방향
        Vector2 shootDirection = -transform.right * CheckLR;
        shootDirection = Quaternion.Euler(0, 0, angle * CheckLR) * shootDirection;
        float shootSpeed = 10f;

        rb.velocity = shootDirection * shootSpeed;
    }

    Queue<GameObject> armShootPool = new Queue<GameObject>();
    public int poolSize = 32;

    /// <summary>
    /// 풀 생성
    /// </summary>
    private void InitializeArmShootPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(armShootPrefab);
            obj.SetActive(false);
            armShootPool.Enqueue(obj);
        }
    }

    /// <summary>
    /// 풀 가져오기
    /// </summary>
    /// <returns></returns>
    private GameObject GetArmShootFromPool()
    {
        if (armShootPool.Count > 0)
        {
            GameObject obj = armShootPool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(armShootPrefab);
            return obj;
        }
    }

    /// <summary>
    /// 풀로 되돌리는 함수
    /// </summary>
    /// <param name="projectile">반환할 오브젝트</param>
    public void ReturnArmShootToPool(GameObject projectile)
    {
        projectile.SetActive(false);
        armShootPool.Enqueue(projectile);
    }
}
