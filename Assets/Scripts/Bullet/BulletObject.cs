using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObject : RecycleObject , IAttack
{
    public BulletType bulletType;

    /// <summary>
    /// 스프라이트 ( 추후에 스프라이트를 설정가능하게 변경 )
    /// </summary>
    SpriteRenderer spriteRenderer = null;
    BulletData data = null;

    /// <summary>
    /// 피격범위
    /// </summary>
    CircleCollider2D circleCollider;

    /// <summary>
    /// 플레이어 불러오기 
    /// </summary>
    protected Player player;

    /// <summary>
    /// 특정 위치 타게팅
    /// </summary>
    Vector2 targetPos;

    /// <summary>
    /// 이동방향
    /// </summary>
    public Vector2 moveDir;

    /// <summary>
    /// 탄의 이동속도
    /// </summary>
    public float moveSpeed = 5;

    /// <summary>
    /// 탄의 데미지
    /// </summary>
    public uint bulletDamage = 1;

    /// <summary>
    /// 라이프타임
    /// </summary>
    public float LifeTime = 1;

    public float floatDir;

    /// <summary>
    /// 탄이 벽을 넘는지 못넘는지 여부
    /// </summary>
    public bool isThrought;

    /// <summary>
    /// 탄의 피격 여부 (패링 , 제거)
    /// </summary>
    public bool isParring;

    /// <summary>
    /// True라면, 플레이어의 불릿이 된다.
    /// </summary>
    public bool isPlayer;

    public uint AttackPower => bulletDamage;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    protected override void OnEnable()
    {
        data = null;
        base.OnEnable();



    }

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void FixedUpdate()
    {
        switch (bulletType)
        {
            case BulletType.Bullet_Straight:
                transform.Translate(Time.deltaTime * moveSpeed * moveDir);
                break;
            case BulletType.Bullet_Chase:
                moveDir = (player.transform.position - transform.position).normalized;
                transform.Translate(Time.deltaTime * moveSpeed * moveDir);
                break;
            case BulletType.Bullet_Straight_Dir:
                float radians = floatDir * Mathf.Deg2Rad;
                Vector3 direction = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
                moveDir = new Vector2(direction.x, direction.z);
                transform.Translate(Time.deltaTime * moveSpeed * moveDir);
                break;

        }
    }

    /// <summary>
    /// 충돌을 검사하는 메서드
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // 지형이라면 
        {
            if (!isThrought)
            {
                Die();
            }
        }
    }



    /// <summary>
    /// 일정 시간후 사라지도록 하는 메서드
    /// </summary>
    /// <returns></returns>
    IEnumerator BulletDelTime()
    {
        yield return new WaitForSeconds(LifeTime);
        Die();
    }

    /// <summary>
    /// 사라질때 실행 될 메서드
    /// </summary>
    public void Die()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    public BulletData BulletData
    {
        get => data;
        set
        {

            if (data != value)
            {
                data = value;
                //아이콘 값을 바꾼다
                bulletType = data.bulletType;      // 이동방식
                moveSpeed = data.moveSpeed;      // 이동 스피드
                bulletDamage = data.bulletDamage;  // 데미지
                isParring = data.isParring;
                isThrought = data.isThrougt;
                LifeTime = data.lifeTime;
                floatDir = data.floatDir;
                circleCollider.radius = data.bulletSize; // 피격 범위
                spriteRenderer.sprite = data.bulletIcon; // 스프라이트
                isPlayer = data.isPlayer;


                if (isPlayer) this.gameObject.layer = 11;
                else this.gameObject.layer = 12;

                StartCoroutine(BulletDelTime());
            }
        }
    }
}
