using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossBase_ : MonoBehaviour
{
    //컴포넌트 불러오기
    Rigidbody2D rb;
    public SpriteRenderer sprite;

    /// <summary>
    /// 플레이어 불러오기
    /// </summary>
    protected Player player;

    /// <summary>
    /// 플레이어 위치 타게팅
    /// </summary>
    Vector2 targetPos;

    /// <summary>
    /// 보스 개체의 최대 체력
    /// </summary>
    public int maxHp = 50;

    /// <summary>
    /// 보스 개체의 현재 체력
    /// </summary>
    private int hp = 50;

    /// <summary>
    /// 패턴 대기시간
    /// </summary>
    protected float waitTime;

    /// <summary>
    /// 적 개체의 데미지 ( 부딪히는 경우만 )
    /// </summary>
    public uint mobDamage = 0;

    /// <summary>
    /// 테스트용 uint
    /// </summary>
    public uint TestPattern = 0;


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



    protected virtual void Awake()
    {
        //animator = GetComponent<Animator>(); 추후엔 애니메이터가 반드시 들어감
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        hp = maxHp;
        InitializePatterns();
    }

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;

        // 보스는 공격을 코루틴으로 사용한다.
        StartCoroutine(AwakeAction());

        
    }

    protected virtual void FixedUpdate() // 이동관련
    {
        // 플레이어의 위치를 받는다.
        targetPos = player.transform.position;

        // 플레이어의 위치에따라 좌우 
        if (targetPos.x < rb.position.x) CheckLR = 1;
        else CheckLR = -1;
    }

    /// <summary>
    /// 플레이어가, 보스와 조우했을때 처음으로 할 행동 ( 개전패턴 or 설정 )
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator AwakeAction()
    {
        // 

        yield return new WaitForSeconds(1.0f);
    }

    /// <summary>
    /// 패턴을 담을 딕셔너리
    /// </summary>
    protected Dictionary<uint, Func<IEnumerator>> patternActions;

    /// <summary>
    /// 딕셔너리 내부 구현 / 1 = 호출할 번호 / 뒤 IEnumerator = 번호에 따른 실행할 코루틴
    /// </summary>
    protected virtual void InitializePatterns()
    {
        patternActions = new Dictionary<uint, Func<IEnumerator>>()
    {
            { 1, BossPattern_1 }

        // 다른 패턴들도 이와 같이 초기화
    };
    }

    /// <summary>
    /// 보스가 행동할 패턴을 선택하는 코루틴, 보스는 패턴을 마치면 이 코루틴을 다시 호출한다.
    /// </summary>
    /// <param name="i">특정 패턴을 실행시키기 위한 변수</param>
    /// <returns></returns>
    protected virtual IEnumerator bossActionSelect(uint pattern = 0)
    {
        pattern = TestPattern;

        // i에 값을 넣었다면, 해당 패턴을 실행하며, 넣지 않았다면 무작위 패턴을 실행한다.
        if (pattern == 0) pattern = (uint)Random.Range(1, patternActions.Count + 1); // 패턴 무작위 선택

        waitPattern();

        yield return new WaitForSeconds(waitTime); // 패턴을 실행하기 전, 여유 시간 (애니메이션 세팅, 딜타임 등 )

        
        // 패턴 실행
        if (patternActions.TryGetValue(pattern, out var action))
        {
            StartCoroutine(action());
        }
    }

    /// <summary>
    /// 패턴 대기시간동안 할일을 이곳에 정리
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator waitPattern()
    {
        

        yield return new WaitForSeconds(1.0f);
    }


    /// <summary>
    /// 패턴 1 : 설명 적기
    /// 1번 패턴은 오버라이드하여 사용 , 나머지는 자식에서 새로 정의
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator BossPattern_1()
    {
        // 공격
        yield return new WaitForSeconds(1.0f);
        //공격
        yield return new WaitForSeconds(1.0f);
        // 공격
        StartCoroutine(bossActionSelect());
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!playerCheck) // 미 발견 상태에서 
        {
            if (collision.gameObject.CompareTag("Player")) // 플레이어가 Trigger 범위 안에 들어왔다면 ( 인식했다면 )
            {
                playerCheck = true;
            }
        }

    }

    /// <summary>
    /// 충돌을 검출하는 메서드
    /// </summary>
    /// <param name="collision"></param>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }

    /// <summary>
    /// 피해를 받았을때 실행할 함수 생성
    /// </summary>
    /// <param name="Damage">플레이어에게 받은 피해</param>
    /// <exception cref="NotImplementedException"></exception>
    public void Damaged(int Damage)
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
