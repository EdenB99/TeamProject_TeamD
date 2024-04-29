using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class StoreUI : MonoBehaviour
{
    /* public GameObject storeSlotPrefab; 
     public Transform storeSlotContainer;;*/
    public ItemCode[] allItemCode;
    private ItemCode[] StoreItemCode = new ItemCode[4];
    StoreSlot[] storeSlots;


    ItemDataManager itemDataManager;


    private void Awake()
    {
        
        Transform child = gameObject.transform.GetChild(1);
        child = child.transform.GetChild(0);
        child = child.transform.GetChild(0);
        storeSlots = child.GetComponentsInChildren<StoreSlot>(); //������ �ִ� ���� �ʱ�ȭ
        /*InitializeStoreSlots();*/
    }
    private void Start()
    {
        itemDataManager = GameManager.Instance.ItemData; //������ �Ŵ��� �ʱ�ȭ
        SetSlotItemData();
    }
    /// <summary>
    /// ���� ������ŭ �� ���Կ� ������ �ڵ带 ����
    /// </summary>
    /// <param name="SlotNum">������ ����</param>
    private void SetSlotItemData(int SlotNum = 4)
    {
        GetItemCodetoRandom(SlotNum); //������ ������ŭ ������ ����
        for (int i = 0; i < storeSlots.Length; i++)
        {
            ItemData itemData = GameManager.Instance.ItemData[StoreItemCode[i]];
            storeSlots[i].SetItemCode(itemData);
            //�� ���Կ� ������ ������ �ڵ� �Է�
        }
    }

    /// <summary>
    /// ���� ���� StoreItemCode�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="Count">StoreItemCode�� ����</param>
    private void GetItemCodetoRandom(int Count)
    {
        //���ο� ������ ����Ʈ�� ����, �̸� �غ�� ������ �ڵ带 ����Ʈ�� �����ϰ�,
        List<ItemCode> items = new List<ItemCode>();
        for (int i = 0; i < allItemCode.Length; i++)  items.Add(allItemCode[i]);

        //���� �غ�� ������ �ڵ��� ���� ī��Ʈ���� �����ϴٸ�
        if (items.Count < Count)
        {
            while (items.Count >= Count) //�غ�� ������ �ڵ��� ���� Count���� Ŀ��������
            {
                items.Add(itemDataManager.itemDatas[0].code); //��ü ������ �߿��� ù��° �������� �߰��Ѵ�. ����� ���
            }
        }
        // ������ ������ ����Ʈ���� �������� ������ �߰��� �������� ������ ����Ʈ���� ����
        for (int i = 0; i < StoreItemCode.Length; i++)
        {
            int rand = Random.Range(0, items.Count);
            StoreItemCode[i] = items[rand];
            items.RemoveAt(rand);
        }
    }

    /*private void InitializeStoreSlots()
    {
        for (int i = 0; i < allItemCode.Length; i++)
        {
            GameObject slotObj = Instantiate(storeSlotPrefab, storeSlotContainer);
            StoreSlot storeSlot = slotObj.GetComponent<StoreSlot>();

            if (storeSlot != null)
            {
                storeSlot.SetItemCode(allItemCode[i]);
                Debug.Log(allItemCode[i]);
            }
        }
    }*/
}
