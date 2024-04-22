using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SwordWeapon : MonoBehaviour, IWeapon
{
    public Transform player;
    /// <summary>
    /// 무기 오프셋 회전추가
    /// </summary>
    public float offsetRotation;

    public Transform hinge;
    Animator animator;
    WeaponAction inputActions;
    SpriteRenderer sprite;

    /// <summary>
    /// 무기 회전
    /// </summary>
    /*private float weaponAngle;

    Vector2 angleMouse;
    Vector2 target;*/

    /// <summary>
    /// 공격 체크
    /// </summary>
    private bool isAttacking = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputActions = new WeaponAction();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        AttachToPlayer();

        transform.SetParent(hinge, false);

        transform.localPosition = new Vector3(0f, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void Update()
    {
        if (hinge.localScale.x < 0)
        {
            transform.localScale = new Vector3(1f, -1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        FlipWeapon();
        FollowMousePosition();
        //Testangle();
    }


    private void OnEnable()
    {
        inputActions.Weapon.Enable();
        inputActions.Weapon.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.Weapon.Attack.performed -= OnAttack;
        inputActions.Weapon.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("공격");
        animator.SetTrigger("Attack");
    }



    public void Attack()
    {
        if (isAttacking) return;    // 이미 공격중이면 리턴
    }

    /// <summary>
    /// 검기?(swing)애니메이션 
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwingEffect()
    {
        isAttacking = transform;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1.0f);

        isAttacking = false;
    }


    /// <summary>
    /// 플레이어 한테 무기를 붙임
    /// </summary>
    private void AttachToPlayer()
    {
        if (player != null)
        {
            // 무기를 플레이어의 자식 객체로 설정
            transform.SetParent(player);

            // 여기에 무기 로컬 설정
            // 손의 위치에 따라 조정
            transform.localPosition = new Vector3(1f, 0f, 0f); // 이 값을 조정하세요.

            // 무기의 로컬 회전을 초기화
            transform.localRotation = Quaternion.Euler(0f, 0f, offsetRotation);
        }
    }

    // 무기가 플레이어의 방향을 따라 회전
    private void FlipWeapon()
    {
        if (player != null)
        {
            // 플레이어 무기 스케일에 따라 조정
            transform.localScale = new Vector3(player.localScale.x, 1f, 1f);
        }
    }

    /* private void Testangle()
     {
         angleMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
         weaponAngle = Mathf.Atan2(angleMouse.y - target.y, angleMouse.x - target.x) * Mathf.Rad2Deg;
         this.transform.rotation = Quaternion.AngleAxis(weaponAngle - 90, Vector3.forward);

     }*/

    /// <summary>
    /// 마우스 포지션
    /// </summary>
    /*private void FollowMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = transform.position.z - Camera.main.transform.position.z;
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePos);

        float dy = target.y - transform.position.y;
        float dx = target.x - transform.position.x;

        float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotateDegree + offsetRotation);
    }*/

    private void FollowMousePosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        // hinge를 통해 무기의 방향을 결정합니다.
        Vector3 hingeDirection = worldMousePos - hinge.position;
        hingeDirection.z = 0; // z축 방향 무시

        // 플레이어가 바라보는 방향에 따라 무기의 회전 방향을 결정합니다.
        float angleOffset = hinge.localScale.x < 0 ? 180f : 0f;
        bool shouldFlip = mousePos.x < transform.position.x;

        // Mathf.Atan2를 사용하여 무기가 마우스 위치를 바라보도록 회전각도를 계산합니다.
        float angle = Mathf.Atan2(hingeDirection.y, hingeDirection.x) * Mathf.Rad2Deg;

        // hinge의 회전을 설정합니다.
        hinge.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + angleOffset + offsetRotation));

    }


    public void WeaponAttack()
    {
        Debug.Log("Attack!");
    }
}
