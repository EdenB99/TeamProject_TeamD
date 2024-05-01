using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class EnemyBase_ : MonoBehaviour, IEnemy , IAttack
{
    //컴포넌트 불러오기
    protected Rigidbody2D rb;
    Collider2D col;
    SpriteRenderer sprite;
    Material material;

    /// <summary>
    /// 플레이어 불러오기
    /// </summary>
    protected Player player;

    /// <summary>
    /// 플레이어 위치 타게팅
    /// </summary>
    protected Vector3 targetPos;

    /// <summary>
    /// 플레이어 발견 여부
    /// </summary>
    [SerializeField]
    protected bool playerDetected;

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
    /// 적의 시야 범위
    /// </summary>
    public float sightRange = 1.0f;

    /// <summary>
    /// HP설정용
    /// </summary>
    protected float hp = 1.0f;

    /// <summary>
    /// HP
    /// </summary>
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            hp = Mathf.Max(hp, 0);
            Debug.Log(hp);
            // Hp가 0 이하면 사망
            if (hp <= 0 && IsLive)
            {
                Die();
            }
        }
    }

    protected bool IsLive = true;

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
            if (checkLR != value && IsLive) // 값이 변경 되었다면
            {
                checkLR = value;
                // 스프라이트 방향 전환 
                if (checkLR == 1) sprite.flipX = false; else { sprite.flipX = true; }
            }

        }
    }

    public float maxHP = 100.0f;
    public float MaxHP => maxHP;

    /// <summary>
    /// 적 개체의 데미지 ( 부딪히는 경우만 )
    /// </summary>
    public uint Attackpower = 1;
    public uint AttackPower => Attackpower;

    /// <summary>
    /// 죽음 처리
    /// </summary>
    public Action onDie { get; set; }

    readonly int Texture2DID = Shader.PropertyToID("_Texture2D");
    readonly int FadeID = Shader.PropertyToID("_Fade");
    float fade = 0.0f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // 몹 메테리얼 가져오기
        sprite.material = GameManager.Instance.Mobmaterial;
        material = sprite.material;
        Sprite sprite2d = sprite.sprite;
        Texture2D texture = sprite2d.texture;
        material.SetTexture(Texture2DID, texture);
    }

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected virtual void FixedUpdate()
    {
        if ( playerDetected && IsLive) // 플레이어 발견시 행동
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
        else if ( IsLive) // 플레이어 미발견시 행동
        {
            
            playerCheck();
            idleAction();
        }

        if ( !IsLive ) // 죽을시
        {
            fade += Time.deltaTime * 0.5f;
            material.SetFloat(FadeID, 1 - fade);

            if ( fade > 1 )
            {
                Destroy(this.gameObject); // 1초후 삭제
            }
        }
    }

    /// <summary>
    /// Update에서 실행될 코드 ( 플레이어 발견 )
    /// </summary>
    protected virtual void attackAction()
    {

    }

    /// <summary>
    /// Update에서 실행될 코드 ( 플레이어 미발견 )
    /// </summary>
    protected virtual void idleAction()
    {

    }

    /// <summary>
    /// 피격
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<IAttack>() != null)            // IAttack을 가지고 있고
        {
            IAttack attack = collision.GetComponent<IAttack>();     // 컴포넌트 가져와서

            TakeDamage(attack.AttackPower);                         // 해당 컴포넌트의 AttackPower만큼 피해를 받음.
        }
    }


    /// <summary>
    /// 플레이어를 탐지하는 불을 리턴하는 메서드 SightRange 안에 들어오면 플레이어가 있는것.
    /// </summary>
    /// <returns>리턴 true = 플레이어가 범위내에 있다.</returns>
    private bool playerCheck()
    {
        // 범위 내에
        Collider2D colliders = Physics2D.OverlapCircle(transform.position, sightRange, LayerMask.GetMask("Player"));

        // 플레이어가 있다면
        if (colliders != null && IsLive)
        {
            

            if (!playerDetected)
            {
                playerDetected = true;
                firstAction();
            }

            return true;
        }
        return false;
    }

    /// <summary>
    /// 플레이어를 첫 조우했을때 할 행동 ( 일반적으로 1회 실행 )
    /// </summary>
    protected virtual void firstAction()
    {

    }
    /// <summary>
    /// 피해를 주는 메서드
    /// </summary>
    public void Attack()
    {
        // 플레이어에게 피해주는것과 관련된 행동 적기
    }

    /// <summary>
    /// 피해를 받는 메서드
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {

        HP -= damage;
    }

    [System.Serializable]
    public struct ItemDropInfo
    {
        public ItemCode code;       // 아이템 종류
        [Range(0, 1)]
        public float dropRatio;     // 드랍 확율(1.0f = 100%)
        public uint dropCount;      // 최대 드랍 개수
    }

    public ItemDropInfo[] dropItems;

    /// <summary>
    /// 아이템 드랍 메서드 / 일반적으로 Die에서 실행
    /// </summary>
    public void ItemDrop()
    {
        foreach(var item in dropItems)
        {
            if ( item.dropRatio > Random.value )
            {
                Factory.Instance.MakeItems(item.code,item.dropCount,transform.position);
            }
        }
    }

    /// <summary>
    /// 죽었을때 실행 될 메서드
    /// </summary>
    public void Die()
    {
        Debug.Log("죽었다.");
        IsLive = false;
        StopAllCoroutines();
        ItemDrop();
        rb.freezeRotation = false;
        col.isTrigger = false;
        
    }
}
