using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
	[Header("HealthInfo")]
	public Image currentHealthBar;
	public Text healthText;
	public float hitPoint = 100f;
	public float maxHitPoint = 100f;
	//==============================================================
	// Regenerate Health & Mana
	//==============================================================
	/// <summary>
	/// 체력 재생의 여부
	/// </summary>
	[Header("RegenInfo")]
	public bool Regenerate = true;
	/// <summary>
	/// interval마다의 회복량
	/// </summary>
	public float regen = 1f;
	private float timeleft = 0.0f;	// Left time for current interval
	/// <summary>
	/// 리젠시의 시간경과량
	/// </summary>
	public float regenUpdateInterval = 0.3f;

	Player player;
	void Awake()
	{

	}
	
  	void Start()
	{
		//플레이어 초기화
		player = GameManager.Instance.Player;
		maxHitPoint = player.PlayerStats.MaxHp;
		hitPoint = maxHitPoint;
		//그래픽 초기화
		UpdateGraphics();

		//시간 조절
		timeleft = regenUpdateInterval;
	}

	void Update ()
	{

		//리젠 체크 시 반복 실행
		if (Regenerate)
			Regen();
	}
	/// <summary>
	/// 리젠 여부를 변경
	/// </summary>
	/// <param name="result">변경할 bool 값</param>
	public void Rengen_OnOff(bool result)
    {
		Regenerate = result;
    }
	
	/// <summary>
	/// TimeLeft만큼 반복해서 회복
	/// </summary>
	private void Regen()
	{
		timeleft -= Time.deltaTime;

		if (timeleft <= 0.0)
		{
			HealDamage(regen);
			UpdateGraphics();
			timeleft = regenUpdateInterval;
			Debug.Log($"{regen}만큼 회복");
		}
	}

	

	/// <summary>
	/// 즉시 피해입힘
	/// </summary>
	/// <param name="Damage">입을 피해량</param>
	public void TakeDamage(float Damage)
	{
		hitPoint -= Damage;
		if (hitPoint < 1)
		{
			hitPoint = 0;
		}

		UpdateGraphics();

	}
	/// <summary>
	/// Heal만큼 즉시 회복
	/// </summary>
	/// <param name="Heal">즉시 회복할 양</param>
	public void HealDamage(float Heal)
	{
		hitPoint += Heal;
		if (hitPoint > maxHitPoint) 
			hitPoint = maxHitPoint;

		UpdateGraphics();
	}
	/// <summary>
	/// maxHealth 증가
	/// </summary>
	/// <param name="max">증가하는 수치</param>
	public void SetMaxHealth(float max)
	{
		maxHitPoint += (int)(maxHitPoint * max / 100);

		UpdateGraphics();
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
