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

    [Header("퀵슬롯 내 아이템의 쿨타임")]
    public float coolTime = 5.0f;
    private float currentTime;

    private int itemCount;
    public int ItemCount
    {
        get => itemCount;
        set
        {
            itemCount = value;
            if (itemCount <= 0) { //남은 아이템이 없으면
                itemCount = 0; //갯수 초기화
                ClearSlot();  //슬롯 초기화
            } else //남은 아이템이 있으면
            {
                SetAmountText(itemCount); //텍스트 변경
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

    //아이템 데이터를 초기화 , 삭제
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
    //아이템 데이터에서 usable설정 및 사용
    public void UseItem()
    {
        if (SlotItemData != null)
        {
            Debug.Log($"{SlotItemData}is on");
            IUsable usable = SlotItemData as IUsable;   // IUsable을 상속 받았는지 확인
            if (usable != null)                     // 상속을 받았으면
            {
                if (usable.Use())            // 아이템 사용 시도
                {
                    ItemCount--; //갯수 줄임
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
       //currentTime이 coolTime이하일때만 증가
       if (currentTime <= coolTime)
        {
            currentTime += Time.deltaTime;
            float colorValue = Mathf.Lerp(0f, 255f, currentTime / coolTime); // 시간비레한 값으로 0부터 255값 사이값 계산
            Color newColor = new Color(colorValue / 255f, colorValue / 255f, colorValue / 255f);
            itemimage.color = newColor;
            
        }
    }
}
