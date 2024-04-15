using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class IngameUI : MonoBehaviour
{
	[Header("HealthInfo")]
	public Image currentHealthBar;
	public Text healthText;
	public float hitPoint = 100f;
	public float maxHitPoint = 100f;
	
	IngameSlotUI[] IngameSlotUIs;
	float[] IngameSlotCount;
    CanvasGroup QuickSlotGroup;

	InventoryInput inventoryInput;

    /// <summary>
    /// 아이템 데이터 매니저(생성필요)
    /// </summary>
    ItemDataManager itemDataManager;

    //ItemData data = itemDataManager[code]; 데이터 매니저에서 아이템 코드를 받아와설정

    Player player;
	void Awake()
	{
        QuickSlotGroup = gameObject.transform.GetChild(1).GetComponent<CanvasGroup>();
        IngameSlotUIs = QuickSlotGroup.transform.GetComponentsInChildren<IngameSlotUI>();
        inventoryInput = new InventoryInput();
        itemDataManager = GameManager.Instance.ItemData;
    }
	
  	void Start()
	{
		//플레이어 초기화
		player = GameManager.Instance.Player;

        maxHitPoint = player.PlayerStats.MaxHp;
		hitPoint = maxHitPoint;
		//그래픽 초기화
		UpdateGraphics();
    }
    private void OnEnable()
    {
		inventoryInput.Ingame.Enable();
        inventoryInput.Ingame.QuickSlot1.performed += OnQuickSlot1;
        inventoryInput.Ingame.QuickSlot2.performed += OnQuickSlot2;
        inventoryInput.Ingame.QuickSlot3.performed += OnQuickSlot3;
    }
    private void OnDisable()
    {
		inventoryInput.Ingame.Disable();
        inventoryInput.Ingame.QuickSlot3.canceled -= OnQuickSlot3;
        inventoryInput.Ingame.QuickSlot2.canceled -= OnQuickSlot2;
        inventoryInput.Ingame.QuickSlot1.canceled -= OnQuickSlot1;
    }

    void Update ()
	{
		
    }

	private void OnQuickSlot1(InputAction.CallbackContext context)
	{
		UseQuickSlotItem(1);
	}
    private void OnQuickSlot2(InputAction.CallbackContext context)
    {

    }
    private void OnQuickSlot3(InputAction.CallbackContext context)
    {

    }
	/// <summary>
	/// 퀵슬롯 내 아이템을 일정 갯수만큼 변경
	/// </summary>
	/// <param name="SlotNumber">변경할 슬롯</param>
	/// <param name="itemData">입력할 아이템데이터</param>
	/// <param name="itemAmount">추가될 아이템 갯수</param>
    public void SetQuickSlotItem(int SlotNumber, ItemData itemData, int itemAmount)
	{
       
        IngameSlotUIs[SlotNumber].GetItemdata(itemData, itemAmount);
	}
    /// <summary>
    /// 퀵슬롯 내 아이템을 변경하여 하나만 추가
    /// </summary>
    /// <param name="SlotNumber">변경할 슬롯</param>
    /// <param name="itemData">입력할 아이템데이터</param>
    public void ChangeQuickSlotItem(int SlotNumber, ItemData itemData)
    {
        IngameSlotUIs[SlotNumber].GetItemdata(itemData);
    }
    /// <summary>
    /// 퀵슬롯 내 아이템의 갯수를 일정 수치만큼 증가
    /// </summary>
    /// <param name="SlotNumber">변경할 슬롯</param>
    /// <param name="Addnum">증가시킬 아이템 갯수</param>
    public void AddQuickSlotItem(int SlotNumber, int Addnum)
	{
		IngameSlotUIs[SlotNumber].AddItem(Addnum);
	}
    /// <summary>
    /// 퀵슬롯 내 아이템의 정보를 삭제
    /// </summary>
    /// <param name="SlotNumber">변경할 슬롯</param>
    public void ClearQuickSlotItem(int SlotNumber)
	{
		IngameSlotUIs[SlotNumber].ClearSlot();
	}
    /// <summary>
    /// 퀵슬롯 내 아이템을 사용
    /// </summary>
    /// <param name="SlotNumber">변경할 슬롯</param>
    public void UseQuickSlotItem(int SlotNumber)
	{
		IngameSlotUIs[SlotNumber].UseItem();
	}
   
    /// <summary>
    /// Hp바 그래픽 초기화
    /// </summary>
    private void UpdateHealthBar()
	{
		float ratio = hitPoint / maxHitPoint;
		currentHealthBar.rectTransform.localPosition = new Vector3(currentHealthBar.rectTransform.rect.width * ratio - currentHealthBar.rectTransform.rect.width, 0, 0);
		healthText.text = maxHitPoint.ToString("0.0") + "/" + hitPoint.ToString("0.0");
	}
	/// <summary>
	/// 그래픽 초기화
	/// </summary>
	private void UpdateGraphics()
	{
		UpdateHealthBar();
	}
	public void SetQuickSlotOnOff(bool Onoff)
	{
		if (Onoff)
		{
			QuickSlotGroup.alpha = 1.0f;
		} else
		{
			QuickSlotGroup.alpha = 0.0f;
		}
	}
}
