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
            switch (targetItemData.type)
            {
                case ItemType.Weapon:
                        if (SlotUI.InvenSlot.IsEquipped) EquipUseText.text = "UnEquip";
                        else EquipUseText.text = "Equip";
                    break;
                case ItemType.Accessory:
                        if (SlotUI.InvenSlot.IsEquipped) EquipUseText.text = "UnEquip";
                        else EquipUseText.text = "Equip";
                    break;
                case ItemType.Consumable:
                    EquipUseText.text = "Use";
                    break;
                default: EquipUseText.text = "None"; break;
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
