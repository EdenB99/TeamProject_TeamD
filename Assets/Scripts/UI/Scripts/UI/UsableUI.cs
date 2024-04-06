using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UsableUI : MonoBehaviour
{
    
    //���õ� �����۵����Ϳ� ����
    ItemData targetItemData;
    InvenSlot targetSlot;

    //������ ��ư�� ���������� ����Ǵ� ��������Ʈ
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
        // ���̱� ���� Ŀ�� ��ġ�� �� ����â �ű��
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if (itemData != null)
        {
            targetItemData = itemData;
        }
    }
    /// <summary>
    /// �ش� â�� �����ϴ� �Լ�
    /// </summary>
    public void Close()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// �� ����â�� �����̴� �Լ�
    /// </summary>
    /// <param name="screenPos">��ũ�� ��ǥ</param>
    public void MovePosition(Vector2 screenPos)
    {
        // Screen.width;   // ȭ���� ���� �ػ�

        if (canvasGroup.alpha > 0.0f)  // ���̴� ��Ȳ���� Ȯ��
        {
            RectTransform rect = (RectTransform)transform;
            int over = (int)(screenPos.x + rect.sizeDelta.x) - Screen.width;    // �󸶳� ���ƴ��� Ȯ��            
            screenPos.x -= Mathf.Max(0, over);  // over�� ����θ� ���(�����϶��� ���� ó�� �ʿ����)
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
