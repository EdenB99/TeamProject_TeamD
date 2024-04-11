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


	CanvasGroup QuickSlotGroup;

	InventoryInput inventoryInput;

	Player player;
	void Awake()
	{
        QuickSlotGroup = gameObject.transform.GetChild(1).GetComponent<CanvasGroup>();
	}
	
  	void Start()
	{
		//플레이어 초기화
		player = GameManager.Instance.Player;
		maxHitPoint = player.PlayerStats.MaxHp;
		hitPoint = maxHitPoint;
		//그래픽 초기화
		UpdateGraphics();

        inventoryInput = new InventoryInput();
    }
    private void OnEnable()
    {
		inventoryInput.Inventory.Enable();
        inventoryInput.Inventory.QuickSlot1.performed += OnQuickSlot1;
        inventoryInput.Inventory.QuickSlot2.performed += OnQuickSlot2;
        inventoryInput.Inventory.QuickSlot3.performed += OnQuickSlot3;

    }
    private void OnDisable()
    {
		inventoryInput.Inventory.Disable();
        inventoryInput.Inventory.QuickSlot3.canceled -= OnQuickSlot3;
        inventoryInput.Inventory.QuickSlot2.canceled -= OnQuickSlot2;
        inventoryInput.Inventory.QuickSlot1.canceled -= OnQuickSlot1;
    }

    void Update ()
	{
       
    }

	private void OnQuickSlot1(InputAction.CallbackContext context)
	{
		Debug.Log("Is slot1");
	}
    private void OnQuickSlot2(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
        QuickSlotGroup.alpha = 0;
    }
    private void OnQuickSlot3(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

	public void QuickSlotsAlpha()
	{

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
}
