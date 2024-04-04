using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEffect : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    BoxCollider2D slashCollider;
    BoxCollider2D stabCollider;
    Animator animator;

    public GameObject weapon;

    protected Vector2 mosPosition;

    protected Player player;

    protected PlayerStats playerStats;

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

    List<EnemyBase_> enemies = new List<EnemyBase_>(1);

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        slashCollider = GetComponent<BoxCollider2D>();
        BoxCollider2D[] collider2Ds = GetComponents<BoxCollider2D>();
        if (collider2Ds[0].isTrigger)
        {
            slashCollider = collider2Ds[0];
            stabCollider = collider2Ds[1];
        }
        else
        {
            stabCollider = collider2Ds[0];
            slashCollider = collider2Ds[1];
        }
    }

    protected virtual void Start()
    {
        player = GameManager.Instance.Player;

        playerStats = player.PlayerStats;
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

    /// <summary>
    /// 슬래시 이펙트 애니메이션 재생
    /// </summary>
    public void PlaySlashEffect()
    {
        animator.SetTrigger("SlashAttack");
    }

    /// <summary>
    /// 스탭 이펙트 애니메이션 재생
    /// </summary>
    public void PlayStabEffect()
    {
        animator.SetTrigger("StabAttack");
    }

    /// <summary>
    /// 이펙트 활성화
    /// </summary>
    public void ActivateEffect()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 이펙트 비활성화
    /// </summary>
    public void DeactivateEffect()
    {
        stabCollider.enabled = false;
        slashCollider.enabled = false;
        gameObject.SetActive(false);
    }


    /// <summary>
    /// 스탭 콜라이더만 작동시키기
    /// </summary>
    public void OnStabAnimationEvent()
    {
        stabCollider.enabled = true;
        slashCollider.enabled = false;
    }

    /// <summary>
    /// 슬래시 콜라이더만 작동시키기
    /// </summary>
    public void OnSlashAnimationEvent()
    {
        stabCollider.enabled = false;
        slashCollider.enabled = true;
    }
}


// 웨폰 오브젝트를 불러와서 입력이 들어오면 비활성화된 이펙트 활성화 -> 웨폰의 특정 포지션에서 작동하게끔

// 이펙트 오브젝트 풀은 합치고 나서 작성
// 일단은 이펙트 활성화 비활성화 정도로 구현만 해두기
// 애니메이션은 두개를 통일 시켜서 스위치로 둘중에 원하는 무기의 형태로 작동시키기
// Mathf 각도를 부여해서 회전시키기
// 

///// < summary >
///// 마우스 버튼이 인벤토리 영역 밖에서 떨어졌을 때 실행되는 함수
///// </summary>
///// <param name="screenPosition">마우스 커서의 스크린좌표 위치</param>
//public void OnDrop(Vector2 screenPosition)
//{
//    // 일단 아이템이 있을 때만 처리
//    if (!InvenSlot.IsEmpty)
//    {
//        Ray ray = Camera.main.ScreenPointToRay(screenPosition); // 스크린좌표를 이용해서 레이 생성
//        if (Physics.Raycast(ray, out RaycastHit hitInfo, 1000.0f, LayerMask.GetMask("Ground")))
//        {
//            // 레이를 이용해서 레이캐스트 실행(Ground레이어에 있는 컬라이더랑만 체크)
//            Vector3 dropoPsition = hitInfo.point;
//            dropPosition.y = 0;

//            Vector3 dropDir = dropPosition - owner.transform.position;
//            if (dropDir.sqrMagnitude > owner.ItemPickupRange * owner.ItemPickupRange)
//            {
//                dropPosition = dropDir.normalized * owner.ItemPickupRange + owner.transform.position;
//            }

//            // 충돌지점에 아이템 생성
//            Factory.Instance.MakeItems(InvenSlot.ItemData.code, InvenSlot.ItemCount,
//                dropPosition, InvenSlot.ItemCount > 1);
//            InvenSlot.ClearSlotItem();      // 임시 슬롯 비우기
//        }
//    }
//}