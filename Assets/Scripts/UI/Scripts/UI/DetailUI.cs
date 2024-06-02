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
    /// ������â�� �����ִ��� Ȯ��
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
    /// �� ����â ���� �Լ�
    /// </summary>
    /// <param name="itemData">ǥ���� ������ ������</param>
    public void Open(ItemData itemData)
    {
        if (!isOn && itemData != null)
        {
            icon.sprite = itemData.itemIcon;
            itemName.text = itemData.itemName;
            ItemCategory.text = itemData.type.ToString();
            DescriptionText.text = itemData.itemDescription;
            //ī�װ��� weapon�϶� ���� �������� �ӵ��� ǥ��
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
            canvasGroup.alpha = 0.0001f; // MovePosition�� alpha�� 0���� Ŭ���� ����Ǵ� �̸� ���ݸ� �ø���
            // ���� ���� ����(0->1)
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }
    }
    public void Close()
    {
        if (isOn)
        {
            // ���� ���� ����(1->0)
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
    }
    /// <summary>
    /// ���İ��� ���ϴ� �ӵ�
    /// </summary>
    public float alphaChangeSpeed = 10.0f;
    /// ���ĸ� 0 -> 1�� ����� �ڷ�ƾ
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
    /// ���ĸ� 1 -> 0���� ����� �ڷ�ƾ
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
