using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    Animator animator;
    protected Player player;

    /// <summary>
    /// 공격 이펙트 오브젝트
    /// </summary>
    public GameObject weaponEffect;

    /// <summary>
    /// 공격 이펙트 생성 지점
    /// </summary>
    public Transform effectSpawnPoint;

    /// <summary>
    /// 이펙트 활성화 여부
    /// </summary>
    private bool isEffectActive = false;

    Transform hinge;

    /// <summary>
    /// 무기 공격력
    /// </summary>
    public int weaponDamage = 10;

    public float critical = 10.0f;


    public float rotationSpeed = 5.0f;

    /// <summary>
    /// 공격할 때 데미지의 총합
    /// </summary>
    public int totalDamage => weaponDamage + GameManager.Instance.Player.attackPower;

    /// <summary>
    /// 무기의 공격 속도
    /// </summary>
    public float weaponSpeed = 10.0f;


    public float attackCooldown = 0.5f; // 공격 간격

    
    private float lastAttackTime = 0f; // 마지막 공격 시간

    /// <summary>
    /// 무기의 초기 위치
    /// </summary>
    protected Vector2 originalPosition;

    /// <summary>
    /// 마우스의 현재 위치
    /// </summary>
    protected Vector2 mousePos;

    /// <summary>
    /// 공격 애니메이션을 제어하기 위한 트리거
    /// </summary>
    private const string attackTrigger = "Attack";

    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    protected virtual void Start()
    {
        originalPosition = transform.localPosition;

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("웨폰베이스스타트");

        player = GameManager.Instance.Player;

        hinge = player.transform.GetChild(0);

        
    }

    protected void Update()
    {
        UpdateWeaponPosition();

        // 공격 입력 처리
        if (Input.GetButtonDown("Fire1") && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    /// <summary>
    /// 업데이트된 무기의 위치좌표
    /// </summary>
    protected virtual void UpdateWeaponPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);        // 마우스 위치 = 스크린상 월드 좌표
        mousePosition.z = 0f;
        transform.position = hinge.position;            // 힌지 위치 불러오기


        Vector2 direction = mousePosition - transform.position;
        transform.Rotate(direction);
        Debug.Log($"{mousePosition},{direction}");


    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                int totalDamage = weaponDamage + player.attackPower;
                enemy.Damaged(totalDamage);
            }
        }
    }

    // 추가된 함수: 공격 입력을 받아 애니메이션을 재생
    protected virtual void Attack()
    {
        animator.SetTrigger("Attack");

        ActivateEffect();

        // 공격 속도에 따라 애니메이션 속도 조절
        float attackAnimationSpeed = player.attackSpeed;
        animator.SetFloat("weaponSpeed", attackAnimationSpeed);
    }

    /// <summary>
    /// 이펙트 활성화 함수
    /// </summary>
    protected void ActivateEffect()
    {
        if (isEffectActive)
            return;

        weaponEffect.SetActive(true);           // 무기 이펙트 활성화
        weaponEffect.transform.position = effectSpawnPoint.position;

        isEffectActive = true;      // 무기 이펙트 활성화 확인
        Debug.Log("무기 이펙트 활성화");
    }

    protected void DeactivateEffect()
    {
        weaponEffect.SetActive(false);      // 무기 이펙트 비활성화

        isEffectActive = false;     // 무기 이펙트 비활성화 확인

        Debug.Log("무기 이펙트 비활성화");
    }

    public void OnAnimationEnd()
    {
        DeactivateEffect();
    }
}



