using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Slider = UnityEngine.UI.Slider;

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

    bool mapToggle = false;

	public QuickSlotUI[] IngameSlotUIs;
	float[] IngameSlotCount;
    CanvasGroup QuickSlotGroup;

	InventoryInput inventoryInput;
    Player player;
    public MapUI bigMap;

    Slider DashCoolSlider;
    /// <summary>
    /// 변할 색상은 0,0,255에서 255,255,255
    /// </summary>
    Image dashCoolColor;

    public GoldPanel goldPanel;

	void Awake()
	{
        QuickSlotGroup = gameObject.transform.GetChild(1).GetComponent<CanvasGroup>();
        IngameSlotUIs = QuickSlotGroup.transform.GetComponentsInChildren<QuickSlotUI>();
        inventoryInput = new InventoryInput();
        DashCoolSlider = gameObject.GetComponentInChildren<Slider>();
        Transform child =  DashCoolSlider.transform.GetChild(1);
        dashCoolColor = child.GetComponentInChildren<Image>();
        DashCoolSlider.value = 0.0f;
        dashCoolColor.color = Color.HSVToRGB(0, 0, 255f);

        Navigation navigation = DashCoolSlider.navigation;
        navigation.mode = Navigation.Mode.None;

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
        player.OnDashingCoolChanged += SetDashCoolTime;

    }
    private void OnEnable()
    {
		inventoryInput.Ingame.Enable();
        inventoryInput.Ingame.QuickSlot1.performed += OnQuickSlot1;
        inventoryInput.Ingame.QuickSlot2.performed += OnQuickSlot2;
        inventoryInput.Ingame.QuickSlot3.performed += OnQuickSlot3;
        inventoryInput.Ingame.MapToggle.performed += MapToggle;
    }


    private void OnDisable()
    {
        inventoryInput.Ingame.MapToggle.performed -= MapToggle;
        inventoryInput.Ingame.QuickSlot3.performed -= OnQuickSlot3;
        inventoryInput.Ingame.QuickSlot2.performed -= OnQuickSlot2;
        inventoryInput.Ingame.QuickSlot1.performed -= OnQuickSlot1;
		inventoryInput.Ingame.Disable();
    }


    private void Update()
    {
        DashCoolSlider.value = sliderValue;
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
    //TODO:: 현재 맵생성시 awake에서 대쉬쪽에 오류가나서 작동안하는중
    private void MapToggle(InputAction.CallbackContext context)
    {
        if (bigMap == null)
        {
            bigMap = GameObject.FindAnyObjectByType<MapUI>();
        }
        if (!mapToggle)
        {
        bigMap.ShowMap();
            mapToggle = true;
        }
        else
        {
        bigMap.HideMap();
            mapToggle = false;
        }

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
    /// 퀵슬롯 내 아이템 여부를 확인하고 존재하면 해당 아이템만큼 추가, 없으면 빈칸을 찾아 추가
    /// </summary>
    /// <param name="SlotNumber">변경할 슬롯</param>
    /// <param name="Addnum">증가시킬 아이템 갯수</param>
    public void AddQuickSlotItem(ItemCode code, int Addnum)
	{
        ItemData itemData = SetItemCodeToData(code);
        bool isItem = false;
        for (int i = 0; i<IngameSlotUIs.Length; i++)
        {
            if (IngameSlotUIs[i].SlotItemData ==  itemData)
            { 
                IngameSlotUIs[i].AddItem(Addnum);
                isItem = true;
                break;
            }
        }
        if (!isItem)
        {
            for (int i = 0; i<IngameSlotUIs.Length;i++)
            {
                if (IngameSlotUIs[i].SlotItemData == null)
                {
                    IngameSlotUIs[i].GetItemdata(itemData, Addnum);
                    break;
                }
            }
        }
        
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

    float sliderValue;
    //대쉬 쿨타임을 받아서 해당 입력을 받으면 색상과 쿨타임 바가 초기화 되게
    public void SetDashCoolTime(float coolTime, float currentTime)
    {
        sliderValue = Mathf.Lerp(0.0f,1.0f, currentTime/coolTime);
        float colorValue = Mathf.Lerp(0.0f, 255.0f, currentTime / coolTime);
        dashCoolColor.color = new Color(colorValue / 255f, colorValue / 255f, 255f);
        
        //float colorValue = Mathf.Lerp(0f, 255f, currentTime / coolTime); // 시간비레한 값으로 0부터 255값 사이값 계산
        //Color newColor = new Color(colorValue / 255f, colorValue / 255f, colorValue / 255f);
    }
    
}
