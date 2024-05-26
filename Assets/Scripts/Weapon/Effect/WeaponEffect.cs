using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

/// <summary>
/// 이펙트의 추가정보
/// </summary>
[System.Serializable]
public struct EffectInfo
{
    public uint effectSize;
    public float effectSpeed;
    public GameObject modelPrefab;
}

public class WeaponEffect : RecycleObject, IAttack
{
    Rigidbody2D rigidbody2d;

    protected Animator animator;

    protected Player player;

    protected PlayerStats playerStats;

    public ItemData_Weapon weaponData;

    protected GameObject weapon;

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

    protected bool isDestroyed = false;

    public List<EnemyBase_> enemies = new List<EnemyBase_>(1);

    protected EnemyBase_ enemy;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        if (weaponData != null)
        {
            ApplyWeaponData(weaponData);
        }


        player = GameManager.Instance.Player;

        playerStats = player.PlayerStats;

        transform.localScale = new Vector3(effectSize, effectSize, 1.0f);           // 인스펙터 창에서 이펙트의 사이즈 조절
    }

    public void ApplyWeaponData(ItemData_Weapon data)
    {
        weaponDamage = data.GetWeaponDamage();
        EffectInfo effectInfo = data.GetEffectInfo();
        effectSize = effectInfo.effectSize;
        float effectSpeed = effectInfo.effectSpeed;
    }

    protected override void OnEnable()
    {
        StartCoroutine(DeactivateEffectAfterAnimation());
    }

    protected IEnumerator DeactivateEffectAfterAnimation()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);      // 현재 재생중인 애니메이션 정보 가져오기

        float currentClipLength = stateInfo.length;         // 애니메이션의 재생 길이를 가져오기


        animator.speed = effectSpeed;

        float animationLength = currentClipLength / effectSpeed;        // 이펙트 재생속도를 조절할 수 있는 변수 부여

        yield return new WaitForSeconds(animationLength);

        isDestroyed = true;
        Destroy(this.gameObject);   
    }
}