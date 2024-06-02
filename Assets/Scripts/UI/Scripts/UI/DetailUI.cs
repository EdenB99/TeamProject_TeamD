using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DetailUI : MonoBehaviour
{
    Image icon;
    TextMeshProUGUI itemName;
    Image damageIcon;
    TextMeshProUGUI damageText;
    Image SpeedIcon;
    TextMeshProUGUI speedText;
    TextMeshProUGUI DescriptionText;
    TextMeshProUGUI ItemCategory;
    CanvasGroup canvasGroup;

    /// <summary>
    /// 디테일창이 켜져있는지 확인
    /// </summary>
    public bool isOn = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.0f;

        Transform child = transform.GetChild(0);
        icon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        itemName = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        ItemCategory = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        damageIcon = child.GetComponent<Image>();
        child = transform.GetChild(4);
        damageText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(5);
        SpeedIcon = child.GetComponent<Image>();
        child = transform.GetChild(6);
        speedText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(7);
        DescriptionText = child.GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// 상세 정보창 여는 함수
    /// </summary>
    /// <param name="itemData">표시할 아이템 데이터</param>
    public void Open(ItemData itemData)
    {
        if (!isOn && itemData != null)
        {
            icon.sprite = itemData.itemIcon;
            itemName.text = itemData.itemName;
            ItemCategory.text = itemData.type.ToString();
            DescriptionText.text = itemData.itemDescription;
            //카테고리가 weapon일때 무기 데미지와 속도값 표시
            Color setColor = new Color(255,255,255,255);
            if (itemData.type == ItemType.Weapon)
            {
                IWeapon weapon = itemData as IWeapon;
                damageText.text = weapon.GetWeaponDamage().ToString();
                damageIcon.color = setColor;
                speedText.text = weapon.GetWeaponSpeed().ToString();
                SpeedIcon.color = setColor;
            } else
            {
                setColor = new Color(255, 255, 255, 0);
                damageText.text = null;
                damageIcon.color = setColor;
                speedText.text = null;
                SpeedIcon.color = setColor;
            }
            canvasGroup.alpha = 0.0001f; // MovePosition이 alpha가 0보다 클때만 실행되니 미리 조금만 올리기
            // 알파 변경 시작(0->1)
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }
    public void Close()
    {
        if (isOn)
        {
            // 알파 변경 시작(1->0)
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }
    /// <summary>
    /// 알파값이 변하는 속도
    /// </summary>
    public float alphaChangeSpeed = 10.0f;
    /// 알파를 0 -> 1로 만드는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1.0f)
        {
            canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 1.0f;
        isOn = true;
    }
    /// <summary>
    /// 알파를 1 -> 0으로 만드는 코루틴
    /// <summary>
    IEnumerator FadeOut()
    {
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha += -Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        canvasGroup.alpha = 0.0f;
        isOn = false;
    }
}
