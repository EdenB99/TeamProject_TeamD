using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UsableUI : MonoBehaviour
{
    
    //���õ� �����۵����Ϳ� ����
    InvenSlotUI targetSlot;

    //������ ��ư�� ���������� ����Ǵ� ��������Ʈ
    public Action<InvenSlotUI> ClickedEquip_Use;

    public Action<InvenSlotUI> ClickedDiscard;

    public Action ClickedExit;

    public TextMeshProUGUI EquipUseText;
    CanvasGroup canvasGroup;

    /// <summary>
    /// ���â�� �����ִ��� Ȯ��
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
        // ���̱� ���� Ŀ�� ��ġ�� �� ����â �ű��
        canvasGroup.alpha = 1.0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        if (SlotUI != null)
        {
            targetSlot = SlotUI;
            ItemData targetItemData = targetSlot.InvenSlot.ItemData;
            IUsable usable = targetItemData as IUsable;   // IUsable�� ��� �޾Ҵ��� Ȯ��
            IEquipable equipable = targetItemData as IEquipable; //Iequipable�� ��ӹ޾Ҵ��� Ȯ��
            if (equipable != null)                     // ����� �޾�����
            {
                EquipUseText.text = "Equip";
            } else if (usable != null)  
            {
                EquipUseText.text = "Use";
            } else
            {
                EquipUseText.text = "Use";
            }
        }
    }
    /// <summary>
    /// �ش� â�� �����ϴ� �Լ�
    /// </summary>
    public void Close()
    {
        isOn = false;
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
    public void Equip_UseButton()
    {
        ClickedEquip_Use?.Invoke(targetSlot);
        Close();
    }
    public void DiscardButton()
    {
        ClickedDiscard?.Invoke(targetSlot);
        Close();
    }
    public void exitButton()
    {
        ClickedExit?.Invoke();
        Close();
    }
}
