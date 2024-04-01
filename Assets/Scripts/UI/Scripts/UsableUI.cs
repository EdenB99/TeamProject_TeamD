using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UsableUI : MonoBehaviour
{
    
    //선택된 아이템데이터와 슬롯
    ItemData targetItemData;
    InvenSlot targetSlot;

    //각각의 버튼이 눌러졌을때 실행되는 델리게이트
    public Action ClickedEquip;

    public Action ClickedUse;

    public Action ClickedDiscard;

    

    

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        Button EquipButton = child.GetComponent<Button>();

        child = transform.GetChild(1);
        Button UseButton = child.GetComponent<Button>();

        child = transform.GetChild(2);
        Button DiscardButton = child.GetComponent<Button>();

    }
    public void Open(ItemData itemData)
    {
        if (itemData != null)
        {
            targetItemData = itemData;
        }
    }
    /// <summary>
    /// 아이템 분리창을 닫는 함수
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 클릭하면 실행되는 함수(UI밖을 클릭했는지를 체크하기 위한 용도)
    /// </summary>
    /// <param name="_"></param>
    private void OnClick(InputAction.CallbackContext _)
    {
        if (!MousePointInRect())  // 마우스 포인터가 UI의 rect안에 있는지 확인
        {
            Close();    // UI 영역 밖을 클릭했으면 닫는다.
        }
    }

    /// <summary>
    /// 마우스 포인터가 UI rect 안에 있는지 확인하는 함수
    /// </summary>
    /// <returns>true면 안에 있다. false면 밖에 있다.</returns>
    bool MousePointInRect()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 diff = screenPos - (Vector2)transform.position; // 이 UI의 피봇에서 마우스 포인터가 얼마나 떨어져 있는지 계산

        RectTransform rectTransform = (RectTransform)transform;
        return rectTransform.rect.Contains(diff);
    }
}
