using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase_ : MonoBehaviour
{
    //컴포넌트 불러오기
    public Rigidbody2D rb;
    public SpriteRenderer sprite;

    /// <summary>
    /// 플레이어 불러오기
    /// </summary>
    protected Player player;

    /// <summary>
    /// 플레이어 위치 타게팅
    /// </summary>
    public Vector2 targetPos;

    /// <summary>
    /// 적 개체의 최대 체력
    /// </summary>
    public int maxHp = 10;

    public float jumpForce = 7f;

    /// <summary>
    /// 적 개체의 현재 체력
    /// </summary>
    public int hp = 10;

    /// <summary>
    /// 적 개체의 데미지 ( 부딪히는 경우만 )
    /// </summary>
    public int mobDamage = 0;

    /// <summary>
    /// 적 개체의 이동속도
    /// </summary>
    public float mobMoveSpeed = 0;

    /// <summary>
    /// 적이 이동하는지 (고정형) 에 대한 여부 false면 방향전환 + 이동을 하지 않는다.
    /// </summary>
    public bool IsMove = true;
    
    /// <summary>
    /// 플레이어를 발견했는지 ( true면 발견 )
    /// </summary>
    protected bool playerCheck = false;


    /// <summary>
    /// Hp 프로퍼티
    /// </summary>
    public int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            hp = Mathf.Max(hp, 0);

            // Hp가 0 이하면 사망
            if (Hp <= 0)
            {
                Die();
            }

        }
    }

    /// <summary>
    /// 좌우 확인
    /// </summary>
    public int checkLR = 1;

    /// <summary>
    /// 좌우 변경용 프로퍼티
    /// </summary>
    public int CheckLR
    {
        get { return checkLR; }
        set
        {
            if (checkLR != value) // 값이 변경 되었다면
            {
                checkLR = value;
                // 방향 전환 ( flip 으로 수정 필요하며, 스프라이트의 기본이 좌측이냐 우측이냐에 따라 바꿔주어야함.)
                gameObject.transform.localScale = new Vector3(1.0f * checkLR, 1.0f, 1.0f);
            }

        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        hp = maxHp;
    }

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected virtual void FixedUpdate() // 이동관련
    {
        if (playerCheck) // 플레이어 발견시 행동
        {
            // 플레이어의 위치를 받는다.
            targetPos = player.transform.position;
            // 움직이는 몬스터일 경우에만
            if (IsMove)
            {
                // 플레이어의 위치에 따라 CheckLR 을 변경한다.
                if (targetPos.x < rb.position.x) CheckLR = 1;
                else CheckLR = -1;
            }
            attackAction();
        }
        else // 플레이어 미발견시 행동
        {
            idleAction();
        }
    }

    /// <summary>
    /// /// 업데이트에서 실행될 코드 ( 플레이어 발견 )
    /// </summary>
    protected virtual void attackAction()
    {

    }

    /// <summary>
    /// 업데이트에서 실행될 코드 ( 플레이어 미발견 )
    /// </summary>
    protected virtual void idleAction()
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if ( !playerCheck ) // 미 발견 상태에서 
        {
            if (collision.gameObject.CompareTag("Player")) // 플레이어가 Trigger 범위 안에 들어왔다면 ( 인식했다면 )
            {
                checkNow();
            }
        }
    }

    /// <summary>
    /// 플레이어 발견 즉시 실행할 함수
    /// </summary>
    protected virtual void checkNow()
    {
        playerCheck = true;
    }

    /// <summary>
    /// 피해를 받았을때 실행할 함수 생성
    /// </summary>
    /// <param name="Damage">플레이어에게 받은 피해</param>
    /// <exception cref="NotImplementedException"></exception>
    private void Damaged(int Damage)
    {
        Hp -= Damage;
    }

    /// <summary>
    /// 죽었을때 실행 될 메서드
    /// </summary>
    public void Die()
    {
        StopAllCoroutines();
    }
}
