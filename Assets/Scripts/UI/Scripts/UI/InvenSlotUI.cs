using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvenSlotUI : SlotUI_Base, IPointerClickHandler, 
    IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{

    
    /// <summary>
    /// 마우스 클릭을 알리는 델리게이트(uint: 클릭이 된 슬롯의 인덱스)
    /// </summary>
    public Action<uint> onClick;

    /// <summary>
    /// 마우스 커서가 슬롯 위로 올라왔다.(uint: 들어간 슬롯의 인덱스)
    /// </summary>
    public Action<uint> onPointerEnter;

    /// <summary>
    /// 마우스 커서가 슬롯에서 나갔다.
    /// </summary>
    public Action onPointerExit;

    /// <summary>
    /// 마우스 커서가 슬롯위에서 움직인다.(Vector2: 마우스 포인터의 스크린 좌표)
    /// </summary>
    public Action<Vector2> onPointerMove;
    /// <summary>
    /// 장비여부 표시용 텍스트
    /// </summary>
    TextMeshProUGUI equipText;


    protected override void Awake()
    {
        base.Awake();
        Transform child = transform.GetChild(1);
        equipText = child.GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(Index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
        onPointerEnter?.Invoke(Index);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        onPointerExit?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        //onPointerMove?.Invoke(eventData.position);
    }
}
