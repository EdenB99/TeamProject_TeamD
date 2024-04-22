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
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1.0f);

        isAttacking = false;
    }


    /// <summary>
    /// �÷��̾� ���� ���⸦ ����
    /// </summary>
    private void AttachToPlayer()
    {
        if (player != null)
        {
            // ���⸦ �÷��̾��� �ڽ� ��ü�� ����
            transform.SetParent(player);

            // ���⿡ ���� ���� ����
            // ���� ��ġ�� ���� ����
            transform.localPosition = new Vector3(1f, 0f, 0f); // �� ���� �����ϼ���.

            // ������ ���� ȸ���� �ʱ�ȭ
            transform.localRotation = Quaternion.Euler(0f, 0f, offsetRotation);
        }
    }

    // ���Ⱑ �÷��̾��� ������ ���� ȸ��
    private void FlipWeapon()
    {
        if (player != null)
        {
            // �÷��̾� ���� �����Ͽ� ���� ����
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
    /// ���콺 ������
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

        // hinge�� ���� ������ ������ �����մϴ�.
        Vector3 hingeDirection = worldMousePos - hinge.position;
        hingeDirection.z = 0; // z�� ���� ����

        // �÷��̾ �ٶ󺸴� ���⿡ ���� ������ ȸ�� ������ �����մϴ�.
        float angleOffset = hinge.localScale.x < 0 ? 180f : 0f;
        bool shouldFlip = mousePos.x < transform.position.x;

        // Mathf.Atan2�� ����Ͽ� ���Ⱑ ���콺 ��ġ�� �ٶ󺸵��� ȸ�������� ����մϴ�.
        float angle = Mathf.Atan2(hingeDirection.y, hingeDirection.x) * Mathf.Rad2Deg;

        // hinge�� ȸ���� �����մϴ�.
        hinge.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle + angleOffset + offsetRotation));

    }


    public void WeaponAttack()
    {
        Debug.Log("Attack!");
    }
}
