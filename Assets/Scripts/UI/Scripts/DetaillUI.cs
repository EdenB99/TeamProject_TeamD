using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DetaillUI : MonoBehaviour
{
    Image icon;
    TextMeshProUGUI itemName;
    TextMeshProUGUI damageText;
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
        child = transform.GetChild(4);
        damageText = child.GetComponent<TextMeshProUGUI>();
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

            canvasGroup.alpha = 0.0001f; // MovePosition�� alpha�� 0���� Ŭ���� ����Ǵ� �̸� ���ݸ� �ø���
            MovePosition(Mouse.current.position.ReadValue()); // ���̱� ���� Ŀ�� ��ġ�� �� ����â �ű��

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
    /// �� ����â�� �����̴� �Լ�
    /// </summary>
    /// <param name="screenPos">��ũ�� ��ǥ</param>
    public void MovePosition(Vector2 screenPos)
    {
        // Screen.width;   // ȭ���� ���� �ػ�

        if (canvasGroup.alpha > 0.0f)  // ���̴� ��Ȳ���� Ȯ��
        {
            RectTransform rect = (RectTransform)transform;
            int over = (int)(screenPos.x + rect.sizeDelta.x) - Screen.width;    // �󸶳� ���ƴ��� Ȯ��            
            screenPos.x -= Mathf.Max(0, over);  // over�� ����θ� ���(�����϶��� ���� ó�� �ʿ����)
            rect.position = screenPos;
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
