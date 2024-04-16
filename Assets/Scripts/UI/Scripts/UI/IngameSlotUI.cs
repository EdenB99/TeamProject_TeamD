using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IngameSlotUI : MonoBehaviour
{
    ItemData SlotItemData;
    Image itemimage;
    TextMeshProUGUI commandText;
    TextMeshProUGUI AmountText;

    [Header("������ �� �������� ��Ÿ��")]
    public float coolTime = 5.0f;
    private float currentTime;

    private int itemCount;
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
    private void Awake()
    {
        itemimage = transform.GetChild(0).GetComponent<Image>();
        commandText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        AmountText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        currentTime = coolTime;
    }

    //������ �����͸� �ʱ�ȭ , ����
    public void GetItemdata(ItemData itemData, int Count = 1)
    {
        if (itemData != null) {
            SlotItemData = itemData;
            ItemCount = Count;
            SetItemImage(itemData.itemIcon);
            currentTime = coolTime;
        }
    }
    public void AddItem(int Count = 1)
    {
        if (SlotItemData != null)
        {
            itemCount += Count;
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
            Debug.Log($"{SlotItemData}is on");
            IUsable usable = SlotItemData as IUsable;   // IUsable�� ��� �޾Ҵ��� Ȯ��
            if (usable != null)                     // ����� �޾�����
            {
                if (usable.Use())            // ������ ��� �õ�
                {
                    ItemCount--; //���� ����
                    currentTime = 0.0f;
                }
            }
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
        AmountText.text = Count.ToString("00");
    }
    public void ClearSlot()
    {
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
            
        }
    }
}
