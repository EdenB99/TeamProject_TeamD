using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Golem : BossBase_
{
    float intervalpattern1 = 1.5f;
    float intervalpattern2 = 1.0f;
    public float intervalAfterTeleport = 2f; // 레이저 발사 후 순간이동까지의 시간 간격
    public float teleportDistance = 2f; // 플레이어 뒤로 순간이동할 거리

    readonly int isCast_Hash = Animator.StringToHash("isCast");
    readonly int isShoot_Hash = Animator.StringToHash("isShoot");

    public GameObject armShootPrefab;
    Animator animator;
    Transform laser;
    Transform armshoot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        hp = maxHp;
        laser = transform.GetChild(0);
        armshoot = transform.GetChild(1);
    }

    protected override IEnumerator AwakeAction()
    {
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(intervalpattern1);
        laser.gameObject.SetActive(false);
    }

    /// <summary>
    /// 패턴1 : 레이저를 쏘고 플레이어 뒤로 순간이동 한 뒤 2초후에 레이저 또 발사
    /// </summary>
    /// <returns></returns>
    IEnumerator BossPattern_1()
    {
        yield return StartCoroutine(FireLaserCoroutine());
        TeleportBehindPlayer();

        yield return new WaitForSeconds(intervalAfterTeleport);

        // 순간이동 후 레이저 다시 발사
        yield return StartCoroutine(FireLaserCoroutine());
        StartCoroutine(bossActionSelect());
    }

    /// <summary>
    /// 레이저 발사 
    /// </summary>
    /// <returns></returns>
    IEnumerator FireLaserCoroutine()
    {
        animator.SetBool(isCast_Hash, true);
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(intervalpattern1);
        laser.gameObject.SetActive(false);
        animator.SetBool(isCast_Hash, false);
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
        yield return StartCoroutine(ArmShootingCoroutine());
        FireArmshoot();

        yield return new WaitForSeconds(intervalpattern2);

        yield return StartCoroutine(ArmShootingCoroutine());
        StartCoroutine(bossActionSelect());
    }

    IEnumerator ArmShootingCoroutine()
    {
        animator.SetBool(isShoot_Hash, true);
        yield return new WaitForSeconds(intervalpattern2);
        armshoot.gameObject.SetActive(true);
    }

    /// <summary>
    /// 발사 함수
    /// </summary>
    void FireArmshoot()
    {
        Rigidbody2D rb = armshoot.GetComponent<Rigidbody2D>();

        Vector2 shootDirection = -transform.right * CheckLR;
        float shootSpeed = 10f; 

        rb.velocity = shootDirection * shootSpeed;
    }

    IEnumerator BossPattern_3()
    {
        animator.SetBool(isCast_Hash, true);
        laser.gameObject.SetActive(true);
        yield return new WaitForSeconds(intervalpattern1);
        laser.gameObject.SetActive(false);
        animator.SetBool(isCast_Hash, false);
        TeleportBehindPlayer();

        yield return new WaitForSeconds(intervalAfterTeleport);


        animator.SetBool(isShoot_Hash, true);
        yield return new WaitForSeconds(intervalpattern2);

        FireMultiple();


        yield return new WaitForSeconds(intervalpattern2);

        animator.SetBool(isShoot_Hash, false);
        yield return new WaitForSeconds(intervalpattern2);

    }

    void FireMultiple()
    {
        float[] angles = { -30f, 0f, 30f };

        foreach (float angle in angles)
        {
            armShootPrefab.SetActive(true);
            FireArmShootAngle(angle);
        }
    }

    void FireArmShootAngle(float angle)
    {
        GameObject projectile = Instantiate(armShootPrefab, armshoot.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // 기본 발사 방향
        Vector2 shootDirection = -transform.right * CheckLR;
        shootDirection = Quaternion.Euler(0, 0, angle * CheckLR) * shootDirection;
        float shootSpeed = 10f;

        rb.velocity = shootDirection * shootSpeed;
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

    protected override IEnumerator bossActionSelect(uint pattern = 0)
    {
        return base.bossActionSelect(pattern);
    }
}
