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
    /// <summary>
    /// 현재 체력
    /// </summary>
	public float hitPoint;
    /// <summary>
    /// 최대 체력
    /// </summary>
	public float maxHitPoint;
	
	IngameSlotUI[] IngameSlotUIs;
	float[] IngameSlotCount;
    CanvasGroup QuickSlotGroup;

	InventoryInput inventoryInput;
    Player player;

	void Awake()
	{
        QuickSlotGroup = gameObject.transform.GetChild(1).GetComponent<CanvasGroup>();
        IngameSlotUIs = QuickSlotGroup.transform.GetComponentsInChildren<IngameSlotUI>();
        inventoryInput = new InventoryInput();

        
    }
	
  	void Start()
	{
		//플레이어 초기화
		player = GameManager.Instance.Player;

        maxHitPoint = player.PlayerStats.MaxHp;
		hitPoint = player.PlayerStats.CurrentHp;
		//그래픽 초기화
		UpdateGraphics();
        player.PlayerStats.onHealthChange += SetHpbar;
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
    private void SetHpbar(float currentHp, float MaxHp)
    {
        maxHitPoint = MaxHp;
        hitPoint = currentHp;
        UpdateGraphics();
    }
	private void OnQuickSlot1(InputAction.CallbackContext context)
	{
		UseQuickSlotItem(0);
	}
    private void OnQuickSlot2(InputAction.CallbackContext context)
    {
        UseQuickSlotItem(1);
    }
    private void OnQuickSlot3(InputAction.CallbackContext context)
    {
        UseQuickSlotItem(2);
    }
    /// <summary>
    /// 아이템 코드를 참조해서 아이템 데이터로 변환
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public ItemData SetItemCodeToData(ItemCode code)
    {
        ItemData data = GameManager.Instance.ItemData[code];
        return data;
    }
	/// <summary>
	/// 퀵슬롯 내 아이템을 일정 갯수만큼 변경
	/// </summary>
	/// <param name="SlotNumber">변경할 슬롯</param>
	/// <param name="itemData">입력할 아이템데이터</param>
	/// <param name="itemAmount">추가될 아이템 갯수</param>
    public void SetQuickSlotItem(int SlotNumber, ItemCode code, int itemAmount)
	{
        ItemData itemData = SetItemCodeToData(code);
        IngameSlotUIs[SlotNumber].GetItemdata(itemData, itemAmount);
	}
    /// <summary>
    /// 퀵슬롯 내 아이템을 변경하여 하나만 추가
    /// </summary>
    /// <param name="SlotNumber">변경할 슬롯</param>
    /// <param name="itemData">입력할 아이템데이터</param>
    public void ChangeQuickSlotItem(int SlotNumber, ItemCode code)
    {
        ItemData itemData = SetItemCodeToData(code);
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
