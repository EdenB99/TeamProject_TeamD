using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponEffect : RecycleObject, IAttack
{
    Rigidbody2D rigidbody2d;

    Animator animator;
    
    protected Player player;

    protected PlayerStats playerStats;

    public GameObject weapon;

    ItemData_Weapon weaponData;

    protected Vector2 mosPosition;

    /// <summary>
    /// 이펙트의 공격 속도
    /// </summary>
    public float effectSpeed = 1.0f;

    /// <summary>
    /// 이펙트의 크기 조절
    /// </summary>
    public float effectSize = 1.0f;

    /// <summary>
    /// 무기 공격력
    /// </summary>
    public int weaponDamage = 10;

    /// <summary>
    /// 공격할 때 데미지의 총합 ( IAttack )
    /// </summary>
    public uint AttackPower => (uint)(weaponDamage + playerStats.AttackPower);

    /// <summary>
    /// 이펙트가 데미지를 주는 간격
    /// </summary>
    public float effectTick = 0.5f;

    /// <summary>
    /// 데미지 쿨 타임
    /// </summary>
    float coolTime = 0.0f;

    protected bool isDestroyed = false;

    public List<EnemyBase_> enemies = new List<EnemyBase_>(1);

    protected EnemyBase_ enemy;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected virtual void Start()
    {
        player = GameManager.Instance.Player;

        playerStats = player.PlayerStats;

        transform.localScale = new Vector3(effectSize, effectSize, 1.0f);           // 인스펙터 창에서 이펙트의 사이즈 조절
    }

    void SetAnimationState()
    {
        switch (weaponData.weaponType)
        {
            case WeaponType.Slash:
                animator.SetTrigger("SlashAttack");
                Debug.Log("슬래시 어택 트리거");
                break;

            case WeaponType.Stab:
                animator.SetTrigger("StabAttack");
                break;
            default:
                break;
        }
        Debug.Log($"{weaponData.weaponType}");
    }

    protected override void OnEnable()
    {
        StartCoroutine(DeactivateEffectAfterAnimation());
        Debug.Log("코루틴 발동");
    }

    private void Update()
    {
        coolTime = -Time.deltaTime;

    }

    //public void OnStabAnimationEvent()
    //{
    //    stabCollider.enabled = true;
    //    slashCollider.enabled = false;
    //}

    //public void OnSlashAnimation()
    //{
    //    stabCollider.enabled = false;
    //    slashCollider.enabled = true;
    //}

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            /* 추후에 인터페이스로 에너미 찾을 예정
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                float totalDamage = weaponDamage + playerStats.attackPower;
                enemy.Damaged(totalDamage);
            }
            */
        }
    }

    protected IEnumerator DeactivateEffectAfterAnimation()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);      // 현재 재생중인 애니메이션 정보 가져오기

        float currentClipLength = stateInfo.length;         // 애니메이션의 재생 길이를 가져오기
        Debug.Log($"{currentClipLength}");

        float animationLength = currentClipLength * effectSpeed;        // 이펙트 재생속도를 조절할 수 있는 변수 부여

        yield return new WaitForSeconds(animationLength);

        isDestroyed = true;
        Destroy(this.gameObject);
    }
}