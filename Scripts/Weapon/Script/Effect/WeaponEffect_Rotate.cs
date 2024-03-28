using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffect_Rotate : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    new CircleCollider2D collider2D;
    
    public GameObject weapon;
    
    protected Vector2 mosPosition;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

        
    }

    

    // 웨폰 오브젝트를 불러와서 입력이 들어오면 비활성화된 이펙트 활성화 -> 웨폰의 특정 포지션에서 작동하게끔
}


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