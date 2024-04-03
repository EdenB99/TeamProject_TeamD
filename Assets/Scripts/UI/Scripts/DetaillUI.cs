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
        child = transform.GetChild(4);
        damageText = child.GetComponent<TextMeshProUGUI>();
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

            canvasGroup.alpha = 0.0001f; // MovePosition이 alpha가 0보다 클때만 실행되니 미리 조금만 올리기
            MovePosition(Mouse.current.position.ReadValue()); // 보이기 전에 커서 위치와 상세 정보창 옮기기

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
    /// 상세 정보창을 움직이는 함수
    /// </summary>
    /// <param name="screenPos">스크린 좌표</param>
    public void MovePosition(Vector2 screenPos)
    {
        // Screen.width;   // 화면의 가로 해상도

        if (canvasGroup.alpha > 0.0f)  // 보이는 상황인지 확인
        {
            RectTransform rect = (RectTransform)transform;
            int over = (int)(screenPos.x + rect.sizeDelta.x) - Screen.width;    // 얼마나 넘쳤는지 확인            
            screenPos.x -= Mathf.Max(0, over);  // over를 양수로만 사용(음수일때는 별도 처리 필요없음)
            rect.position = screenPos;
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
