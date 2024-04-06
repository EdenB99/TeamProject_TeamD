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

    CanvasGroup canvasGroup;



    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Close();


        Transform child = transform.GetChild(0);
        Button EquipButton = child.GetComponent<Button>();

        child = transform.GetChild(1);
        Button UseButton = child.GetComponent<Button>();

        child = transform.GetChild(2);
        Button DiscardButton = child.GetComponent<Button>();


    }
    public void Open(ItemData itemData)
    {
        MovePosition(Mouse.current.position.ReadValue());
        // 보이기 전에 커서 위치와 상세 정보창 옮기기
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if (itemData != null)
        {
            targetItemData = itemData;
        }
    }
    /// <summary>
    /// 해당 창을 종료하는 함수
    /// </summary>
    public void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 상세 정보창을 움직이는 함수
    /// </summary>
    /// <param name="screenPos">스크린 좌표</param>
    public void MovePosition(Vector2 screenPos)
    {
        // Screen.width;   // 화면의 가로 해상도

        if (canvasGroup.alpha > 0.0f)  // 보이는 상황인지 확인
        {
            RectTransform rect = (RectTransform)transform;
            int over = (int)(screenPos.x + rect.sizeDelta.x) - Screen.width;    // 얼마나 넘쳤는지 확인            
            screenPos.x -= Mathf.Max(0, over);  // over를 양수로만 사용(음수일때는 별도 처리 필요없음)
            rect.position = screenPos;
        }
    }
    private void EquipButton()
    {

    }
    private void UseButton()
    {

    }
    private void DiscardButton()
    {

    }
}
