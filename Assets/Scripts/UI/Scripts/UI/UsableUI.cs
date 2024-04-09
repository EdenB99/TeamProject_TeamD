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
    InvenSlotUI targetSlot;

    //각각의 버튼이 눌러졌을때 실행되는 델리게이트
    public Action<ItemData> ClickedEquip_Use;

    public Action ClickedDiscard;

    public Action ClickedExit;

    CanvasGroup canvasGroup;

    /// <summary>
    /// 사용창이 켜져있는지 확인
    /// </summary>
    public bool isOn = false;

    
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Close();

    }
    public void Open(InvenSlotUI SlotUI)
    {
        isOn = true;
        canvasGroup.alpha = 0.0001f;
        MovePosition(Mouse.current.position.ReadValue());
        // 보이기 전에 커서 위치와 상세 정보창 옮기기
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if (SlotUI != null)
        {
            targetSlot = SlotUI;
        }
        if (SlotUI.InvenSlot.ItemData != null)
        {
            targetItemData = SlotUI.InvenSlot.ItemData;
        }
    }
    /// <summary>
    /// 해당 창을 종료하는 함수
    /// </summary>
    public void Close()
    {
        isOn = false;
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
    public void Equip_UseButton()
    {
        ClickedEquip_Use?.Invoke(targetItemData);
        Close();
    }
    public void DiscardButton()
    {
        ClickedDiscard?.Invoke();
        Close();
    }
    public void exitButton()
    {
        ClickedExit?.Invoke();
        Close();
    }
}
