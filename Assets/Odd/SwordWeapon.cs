using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SwordWeapon : MonoBehaviour, IWeapon
{
    public Transform player;
    /// <summary>
    /// ���� ������ ȸ���߰�
    /// </summary>
    public float offsetRotation;

    public Transform hinge;
    Animator animator;
    WeaponAction inputActions;
    SpriteRenderer sprite;

    /// <summary>
    /// ���� ȸ��
    /// </summary>
    /*private float weaponAngle;

    Vector2 angleMouse;
    Vector2 target;*/

    /// <summary>
    /// ���� üũ
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
       Debug.Log("����");
       animator.SetTrigger("Attack");
    }



    public void Attack()
    {
        if (isAttacking) return;    // �̹� �������̸� ����
    }

    /// <summary>
    /// �˱�?(swing)�ִϸ��̼� 
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
    /// �÷��̾� ���� ���⸦ ����
    /// </summary>
   /* private void AttachToPlayer()
    {
        if (player != null)
        {
            // ���⸦ �÷��̾��� �ڽ� ��ü�� ����
            transform.SetParent(player);

            // ���⿡ ���� ���� ����
            // ���� ��ġ�� ���� ����
            transform.localPosition = new Vector3(1f, 0f, 0f); // 

            // ������ ���� ȸ���� �ʱ�ȭ
            transform.localRotation = Quaternion.Euler(0f, 0f, offsetRotation);
        }
    }*/

    // ���Ⱑ �÷��̾��� ������ ���� ȸ��
    private void FlipWeapon()
    {
        if (player != null)
        {
            // �÷��̾� ���� �����Ͽ� ���� ����
            transform.position = new Vector3(hinge.localScale.x, 1f, 1f);
        }
    }





    private void FollowMousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;


        Vector3 direction = mousePos - hinge.position;
        direction.z = 0;

        // Mathf.Atan2 �Լ��� ����Ͽ� ���� ���Ϳ� ���� ������ ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ���� ������Ʈ�� z�� ȸ������ �����Ͽ� ���콺 ��ġ�� ����Ű���� ����
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        if (mousePos.x < hinge.position.x)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
        transform.position = hinge.position;
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
