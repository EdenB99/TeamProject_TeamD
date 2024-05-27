using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

/// <summary>
/// 무기의 추가정보
/// </summary>
[System.Serializable]
public struct WeaponInfo
{
    public uint weaponDamage;
    public float attackSpeed;
    public GameObject modelPrefab;
    public WeaponType weaponType;
}

public class WeaponBase : MonoBehaviour
{
    new Rigidbody2D rigidbody;
    protected Animator animator;
    protected WeaponAction weaponInputActions;

    SpriteRenderer spriteRenderer;

    /// <summary>
    /// 무기의 세로 길이
    /// </summary>
    private float weaponLength;

    protected Player player;

    protected PlayerStats playerStats;

    /// <summary>
    /// 무기 이펙트에 적용할 무기 데이터
    /// </summary>
    [SerializeField]
    private ItemData_Weapon weaponData;

    /// <summary>
    /// 무기 이펙트 프리팹
    /// </summary>
    [SerializeField]
    private GameObject weaponEffectPrefab;
    GameObject weaponSkillPrefab;

    /// <summary>
    /// 이펙트 생성좌표
    /// </summary>
    Vector2 effectPosition;

    protected Vector3 direction;

    /// <summary>
    /// 플레이어의 힌지
    /// </summary>
    protected Transform hinge;

    /// <summary>
    /// 무기 공격력
    /// </summary>
    public int weaponDamage = 10;

    public float critical = 10.0f;

    public float rotationSpeed = 5.0f;

    /// <summary>
    /// 공격할 때 데미지의 총합
    /// </summary>
    public float totalDamage => weaponDamage + playerStats.AttackPower;

    /// <summary>
    /// 무기 공격속도, 배수로 적용됨. 높을수록 빠른값
    /// </summary>
    public float weaponSpeed = 1.0f;

    public float attackCooldown = 0.5f; // 공격 간격

    public float currentCoolTime = 0.0f;

    public bool CanAttack = true;

    /// <summary>
    /// 공격 애니메이션을 제어하기 위한 트리거
    /// </summary>
    private const string attackTrigger = "Attack";

    protected WeaponManager weaponManager;



    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        weaponInputActions = new WeaponAction();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (!IsPlayerAlive())
        {
            DeactivateWeapon();
        }
        DontDestroyOnLoad(gameObject);
    }

    protected virtual void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

        player = GameManager.Instance.Player;

        playerStats = player.PlayerStats;

        hinge = player.transform.GetChild(0);

        weaponLength = spriteRenderer.sprite.bounds.size.y * transform.localScale.y;

        weaponManager = GameManager.Instance.WeaponManager;
        weaponManager.currentWeaponindexChange += Switchweapon;

        if (weaponData != null)
        {
            WeaponInfo weaponInfo = weaponData.GetWeaponInfo();
            weaponSpeed = weaponInfo.attackSpeed;
           
            weaponDamage = (int)weaponInfo.weaponDamage;
        }   

        currentCoolTime = attackCooldown;
        CanAttack = true;

        GameManager.Instance.InventoryUI.PlayerMoveToggle += WeaponToggle;
    }

    protected virtual void OnEnable()
    {
        if (IsPlayerAlive())
        {
            weaponInputActions.Weapon.Enable();
            weaponInputActions.Weapon.Attack.performed += OnAttack;
        }
    }

    protected virtual void OnDisable()
    {
        if (IsPlayerAlive())
        {
            weaponInputActions.Weapon.Attack.performed -= OnAttack;
            weaponInputActions.Weapon.Disable();
        }
    }
    private void WeaponToggle(bool Toggle)
    {
        if (Toggle)
        {
            weaponInputActions.Weapon.Attack.performed -= OnAttack;
        }
        else weaponInputActions.Weapon.Attack.performed += OnAttack;
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        if (IsPlayerAlive() && CanAttack)
        {
            Attack();
            currentCoolTime = 0.0f;
            CanAttack = false;
        }
    }

    protected virtual void Update()
    {
        if (IsPlayerAlive())
        {
            UpdateWeaponPosition();
        }
        
        if (currentCoolTime < attackCooldown)
        {
            currentCoolTime += Time.deltaTime * weaponSpeed;
           
        } else
        {
            CanAttack = true;
        }
    }

    protected virtual void UpdateWeaponPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        transform.position = hinge.position;

        direction = mousePosition - transform.position;
        direction.z = 0;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = rotation;

        // 마우스 위치에 따라 무기의 방향(좌우 반전) 결정
        if (mousePosition.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        effectPosition = transform.position + (direction.normalized * weaponLength);
    }

    private void OnDestroy()
    {
        weaponInputActions.Weapon.Disable();
    }

    // 추가된 함수: 공격 입력을 받아 애니메이션을 재생
    protected virtual void Attack()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // 마우스 위치에 따라 애니메이션의 방향(좌우 반전) 결정
        if (mousePosition.x < transform.position.x)
        {
            // 애니메이션이 왼쪽을 향하도록 좌우 반전
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // 애니메이션이 오른쪽을 향하도록 원래대로 복구
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
            
        animator.SetTrigger(attackTrigger);

        float attackSpeed = weaponSpeed;
        float attackAnimationSpeed = playerStats.AttackSpeed * attackSpeed;
        animator.speed = attackAnimationSpeed;
        ActivateEffect(transform.position, weaponData);
    }

    /// <summary>
    /// 이펙트 활성화 함수
    /// </summary>
    public virtual void ActivateEffect(Vector2 effectPosition, ItemData_Weapon weaponData)
    {
        float angle = MathF.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject weaponEffectInstance = Instantiate(weaponEffectPrefab, this.effectPosition, rotation);

        WeaponEffect weaponEffect = weaponEffectInstance.GetComponent<WeaponEffect>();
        if (weaponEffect != null)
        {
            weaponEffect.weaponData = weaponData;
            weaponEffect.ApplyWeaponData(weaponData);
        }

    }

    /// <summary>
    /// 플레이어가 죽었을 경우 판별
    /// </summary>
    /// <returns></returns>
    protected bool IsPlayerAlive()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        return playerStats != null && playerStats.IsAlive;
    }


    /// <summary>
    /// 무기의 비활성화 함수
    /// </summary>
    protected virtual void DeactivateWeapon()
    {
        weaponInputActions.Weapon.Disable();
    }

    public void Switchweapon(int index)
    {
        if (GameManager.Instance.WeaponManager.weaponsData[weaponManager.CurrentWeaponIndex] != null)
        {
            GameManager.Instance.WeaponManager.ActivateWeaponPrefab(GameManager.Instance.WeaponManager.weaponsData[weaponManager.CurrentWeaponIndex]);
        }
    }
}