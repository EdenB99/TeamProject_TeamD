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
        //AttachToPlayer();

        /*transform.SetParent(hinge, false);

        transform.localPosition = new Vector3(0f, 0f, 0f);
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);*/
    }

    private void Update()
    {
      
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
        //animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1.0f);

        isAttacking = false;
    }


    /// <summary>
    /// 플레이어 한테 무기를 붙임
    /// </summary>
   /* private void AttachToPlayer()
    {
        if (player != null)
        {
            // 무기를 플레이어의 자식 객체로 설정
            transform.SetParent(player);

            // 여기에 무기 로컬 설정
            // 손의 위치에 따라 조정
            transform.localPosition = new Vector3(1f, 0f, 0f); // 

            // 무기의 로컬 회전을 초기화
            transform.localRotation = Quaternion.Euler(0f, 0f, offsetRotation);
        }
    }*/

    // 무기가 플레이어의 방향을 따라 회전
    private void FlipWeapon()
    {
        if (player != null)
        {
            // 플레이어 무기 스케일에 따라 조정
            transform.position = new Vector3(hinge.localScale.x, 1f, 1f);
        }
    }


   
   

    private void FollowMousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = hinge.position;

        Vector3 direction = mousePos - transform.position;
        direction.z = 0;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, direction);
        hinge.rotation = rotation;

        if(mousePos.x < transform.position.x)
        {
            sprite.flipX = true;
            hinge.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            sprite.flipX = false;
            hinge.localScale = new Vector3(1, -1, 1);
        }
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
    }

    private IEnumerator ResetHingeRotation()
    {
       
        yield return new WaitForSeconds(0.5f); 
        hinge.rotation = Quaternion.identity; 
    }


    public void WeaponAttack()
    {
        Debug.Log("Attack!");
    }
}
