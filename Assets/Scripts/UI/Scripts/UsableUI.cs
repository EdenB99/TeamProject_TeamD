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
    /// ������ �и�â�� �ݴ� �Լ�
    /// </summary>
    public void Close()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Ŭ���ϸ� ����Ǵ� �Լ�(UI���� Ŭ���ߴ����� üũ�ϱ� ���� �뵵)
    /// </summary>
    /// <param name="_"></param>
    private void OnClick(InputAction.CallbackContext _)
    {
        if (!MousePointInRect())  // ���콺 �����Ͱ� UI�� rect�ȿ� �ִ��� Ȯ��
        {
            Close();    // UI ���� ���� Ŭ�������� �ݴ´�.
        }
    }

    /// <summary>
    /// ���콺 �����Ͱ� UI rect �ȿ� �ִ��� Ȯ���ϴ� �Լ�
    /// </summary>
    /// <returns>true�� �ȿ� �ִ�. false�� �ۿ� �ִ�.</returns>
    bool MousePointInRect()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 diff = screenPos - (Vector2)transform.position; // �� UI�� �Ǻ����� ���콺 �����Ͱ� �󸶳� ������ �ִ��� ���

        RectTransform rectTransform = (RectTransform)transform;
        return rectTransform.rect.Contains(diff);
    }
}
