using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuickSlotUI : MonoBehaviour
{
    public ItemData SlotItemData;
    Image itemimage;
    TextMeshProUGUI commandText;
    TextMeshProUGUI AmountText;

    [Header("������ �� �������� ��Ÿ��")]
    private float coolTime = 5.0f;

    private float currentTime;
    private bool ReadytoUseItem;

    private int itemCount = 0;
    public int ItemCount
    {
        get => itemCount;
        set
        {
            itemCount = value;
            if (itemCount <= 0) { //���� �������� ������
                itemCount = 0; //���� �ʱ�ȭ
                ClearSlot();  //���� �ʱ�ȭ
            } else //���� �������� ������
            {
                SetAmountText(itemCount); //�ؽ�Ʈ ����
            }
        }
    }

    public Action<ItemData> ItemUse;

    private void Awake()
    {
        itemimage = transform.GetChild(0).GetComponent<Image>();
        commandText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        AmountText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        currentTime = coolTime;
        SetAmountText(itemCount);
    }

    //������ �����͸� �ʱ�ȭ , ����
    public void GetItemdata(ItemData itemData, int Count = 1)
    {
        if (itemData != null) {
            SlotItemData = itemData;
            ItemCount = Count;
            SetItemImage(itemData.itemIcon);
            coolTime = itemData.cooltime;
            currentTime = coolTime;
            ReadytoUseItem = true;
        }
    }
    public void AddItem(int Count = 1)
    {
        if (SlotItemData != null)
        {
            itemCount += Count;
            SetAmountText(itemCount);
        }
    }
    public void ClearItemData()
    {
        SlotItemData = null;
        ClearSlot();
    }
    //������ �����Ϳ��� usable���� �� ���
    public void UseItem()
    {
        if (SlotItemData != null)
        {
            IUsable usable = SlotItemData as IUsable;   // IUsable�� ��� �޾Ҵ��� Ȯ��
            if (usable != null)                     // ����� �޾�����
            {
                if (ReadytoUseItem)
                {
                    if (usable.Use())            // ������ ��� �õ�
                    {
                        ItemUsed();
                    }
                }
            }

            IBuff buff = SlotItemData as IBuff;   // IBuff�� ��� �޾Ҵ��� Ȯ��
            if (buff != null)                     // ����� �޾�����
            {
                if (ReadytoUseItem)
                {
                    if ( buff.BuffActive() > 0 ) // ���� �������� 0�� �̻��� �ð��� ������. ��� �������� ���Ǵ� ��� ����
                    
                    ItemUsed();
                }
            }

            IActivatable active = SlotItemData as IActivatable;     // Iactive�� ��� �޾Ҵ��� Ȯ��
            if (active != null)                                     // ����� �޾�����
            {
                if (ReadytoUseItem)
                {
                    if (active.ItemActive(Camera.main.ScreenToWorldPoint(Input.mousePosition) ) )
                    {
                        ItemUsed();
                    }
                }
            }
        } else
        {
            Debug.Log("������ ������ �����ϴ�.");
        }
    }

    public void ItemUsed()
    {
        ItemUse?.Invoke(SlotItemData);
        ItemCount--; //���� ����
        if ( ItemCount != 0 )
        {
            currentTime = 0.0f;
            ReadytoUseItem = false;
        }

    }

    public void SetItemImage(Sprite itemSprite)
    {
        itemimage.sprite = itemSprite;
    }
    public void SetComandText(string text)
    {
        commandText.text = text;
    }
    public void SetAmountText(int Count) 
    {
        if (Count <= 0)
        {
            AmountText.text = null;
        } else
        {
            AmountText.text = Count.ToString("00");
        }
    }
    public void ClearSlot()
    {
        SlotItemData = null;
        itemimage.sprite = null;
        AmountText.text = null;
    }

    void Update()
    {
       //currentTime�� coolTime�����϶��� ����
       if (currentTime <= coolTime)
        {
            currentTime += Time.deltaTime;
            float colorValue = Mathf.Lerp(0f, 255f, currentTime / coolTime); // �ð����� ������ 0���� 255�� ���̰� ���
            Color newColor = new Color(colorValue / 255f, colorValue / 255f, colorValue / 255f);
            itemimage.color = newColor;
        } else
        {
            ReadytoUseItem = true;
        }
    }
}
