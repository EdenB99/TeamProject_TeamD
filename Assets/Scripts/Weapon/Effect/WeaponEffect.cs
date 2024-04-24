using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponEffect : RecycleObject
{
    Rigidbody2D rigidbody2d;

    Animator animator;
    
    protected Player player;

    protected PlayerStats playerStats;

    public GameObject weapon;

    WeaponData weaponData;

    protected Vector2 mosPosition;

    /// <summary>
    /// 무기 공격력
    /// </summary>
    public int weaponDamage = 10;

    /// <summary>
    /// 공격할 때 데미지의 총합
    /// </summary>
    public float totalDamage => weaponDamage + playerStats.attackPower;

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
        if (coolTime < 0)
        {
            foreach (EnemyBase_ enemy in enemies)
            {
                enemy.Damaged(totalDamage);
            }
            coolTime = effectTick;
        }
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

        yield return new WaitForSeconds(currentClipLength);

        isDestroyed = true;
        Destroy(this.gameObject);
    }
}