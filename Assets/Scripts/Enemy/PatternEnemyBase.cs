using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class PatternEnemyBase : MonoBehaviour, IEnemy
{
    //컴포넌트 불러오기
    Rigidbody2D rb;
    protected Animator animator;
    public SpriteRenderer sprite;
    Sprite sprite2d;
    protected Texture2D texture;

    protected readonly int Texture2DID = Shader.PropertyToID("_Texture2D");
    protected readonly int FadeID = Shader.PropertyToID("_Fade");
    protected readonly int HitID = Shader.PropertyToID("_Hit");
    float fade = 0.0f;

    /// <summary>
    /// 플레이어 불러오기
    /// </summary>
    protected Player player;

    /// <summary>
    /// 플레이어 위치 타게팅
    /// </summary>
    protected Vector2 playerPos;

    /// <summary>
    /// 플레이어 감지 범위 ( 보스는 컬라이더가 아닌 코드로 플레이어를 탐지한다 )
    /// </summary>
    public float sightRange = 1.0f;

    protected enum BossState
    {
        Wait,       // 대기
        Chase,      // 플레이어 추적
        Attack,     // 공격 패턴
        SpAttack,   // 특수 패턴
        Dead        // 죽음
    }

    /// <summary>
    /// 보스 상태
    /// </summary>
    BossState state = BossState.Wait;

    /// <summary>
    /// 상태 변경
    /// </summary>
    Action stateUpdate;

    /// <summary>
    /// 상태 프로퍼티
    /// </summary>
    protected BossState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                switch (state)  // 상태에 진입할 때 할 일들 처리
                {
                    case BossState.Wait:
                        State_Wait();
                        stateUpdate = Update_Wait; break;

                    case BossState.Chase:
                        State_Chase();
                        stateUpdate = Update_Chase; break;

                    case BossState.Attack:
                        State_Attack();
                        stateUpdate = Update_Attack; break;

                    case BossState.SpAttack:
                        State_SpAttack();
                        stateUpdate = Update_Attack; break;

                    case BossState.Dead:
                        Die();
                        stateUpdate = Update_Attack; break;
                }
            }
        }
    }

    protected virtual void State_Wait()
    {

    }

    protected virtual void State_Chase()
    {

    }

    protected virtual void State_Attack() // 단순 패턴이 있을경우
    {

    }

    protected virtual void State_SpAttack()
    {

    }


    protected float hp = 100.0f;
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            hp = Mathf.Max(hp, 0);

            Debug.Log(hp);

            // Hp가 0 이하면 사망
            if (hp <= 0)
            {
                Die();
            }

        }
    }


    public float maxHP = 100.0f;
    public float MaxHP => maxHP;

    /// <summary>
    /// 생존 여부
    /// </summary>
    bool IsLive = true;

    /// <summary>
    /// 적 개체의 데미지 ( 부딪히는 경우만 )
    /// </summary>
    public uint Attackpower = 1;
    public uint AttackPower => Attackpower;
    public Action onDie { get; set; }

    /// <summary>
    /// 패턴 대기시간
    /// </summary>
    protected float waitTime;

    /// <summary>
    /// 테스트용 uint
    /// </summary>
    public uint TestPattern = 0;

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
            if (checkLR != value && State != BossState.Attack && State != BossState.SpAttack ) // 값이 변경 되었고, 공격중이 아니라면
            {
                checkLR = value;
                gameObject.transform.localScale = new Vector3(1.0f * checkLR, 1.0f, 1.0f); // 이 방법으로 구현해야, 자식 컬라이더가 따라감.
            }

        }
    }

   
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        InitializePatterns();

        stateUpdate = Update_Wait;

        // 몹 메테리얼 가져오기
        sprite.material = GameManager.Instance.Mobmaterial; // 메테리얼을 가져온다.
        sprite2d = sprite.sprite;                           // 스프라이트의 스프라이트를 , 
        texture = sprite2d.texture;                         // 텍스쳐로 변환
        sprite.material.SetTexture(Texture2DID, texture);   // 스프라이트의 메테리얼을 현재 스프라이트로 전환
    }

    protected virtual void Start()
    {
        HP = MaxHP;
        player = GameManager.Instance.Player;   
    }

    protected virtual void Update()
    {
        if (IsLive)
        {
            // 플레이어의 위치를 받는다.
            playerPos = player.transform.position;
            // 플레이어의 위치에따라 좌우 
            if (playerPos.x < rb.position.x) CheckLR = 1;
            else CheckLR = -1;

            stateUpdate();
        }
        else // 죽었다면,
        {
            fade += Time.deltaTime * 0.25f; // 보스는 느리게 사라짐
            sprite.material.SetFloat(FadeID, 1 - fade);

            if (fade > 1)
            {
                Destroy(this.gameObject); // 4초후 삭제
            }
        }

    }

    

    protected virtual void Update_Wait()
    {


    }

    protected virtual void Update_Chase()
    {

    }

    protected virtual void Update_Attack()
    {

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
    /// 개전시 행동할 패턴
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator AwakeAction()
    {
        yield return null;
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
        //공격
        StartCoroutine(bossActionSelect());
    }


    /// <summary>
    /// 플레이어를 탐지하는 불을 리턴하는 메서드 SightRange 안에 들어오면 플레이어가 있는것.
    /// </summary>
    /// <returns>리턴 true = 플레이어가 범위내에 있다.</returns>
    protected bool playerCheck()
    {
        // 범위 내에
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, sightRange, LayerMask.GetMask("Player"));

        // 플레이어가 있다면
        if (colliders.Length > 0)
        {
            return true;
        }
        return false;
    }

    public void Attack()
    {
        // 플레이어에게 피해주는것과 관련된 행동 적기
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<IAttack>() != null )            // IAttack을 가지고 있고, 무적상태가 아닐때만
        {
            IAttack attack = collision.GetComponent<IAttack>();     // 컴포넌트 가져와서

            TakeDamage(attack.AttackPower);                         // 해당 컴포넌트의 AttackPower만큼 피해를 받음.
        }
    }


    /// <summary>
    /// 피해를 받았을때 실행할 함수 생성
    /// </summary>
    /// <param name="Damage">플레이어에게 받은 피해</param>
    public void TakeDamage(float damage)
    {
        if (IsLive)
        {
            sprite.material.SetFloat(HitID, 1);
            StartCoroutine(onHit());
            HP -= damage;
        }
    }

    IEnumerator onHit()
    {
        yield return new WaitForSeconds(0.1f);
        sprite.material.SetFloat(HitID, 0);
    }

    /// <summary>
    /// 죽었을때 실행 될 메서드
    /// </summary>
    public virtual void Die()
    {
        StopAllCoroutines();

        IsLive = false;
        sprite.material.SetFloat(HitID, 0);
    }


}
