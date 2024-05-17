using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI GoldText;

    Player player;
    Action<int> goldChange;

    public float TextOutputTime = 2.0f; // ������ �ð�
    public float alphaChangeSpeed = 10.0f;
    public float numberChangeSpeed = 1.0f; // ���� ���� �ӵ�

    private bool isInvenOn;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        GoldText = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        canvasGroup.alpha = 0.0f;

        player  = GameManager.Instance.Player;
        player.OnGoldChange += GoldChange;
    }

    private void GoldChange(uint currentGold)
    {
        StopAllCoroutines();
        StartCoroutine(FadeInAndOut(currentGold));
    }

    IEnumerator FadeInAndOut(uint currentGold)
    {
        yield return StartCoroutine(FadeIn(currentGold));
        yield return new WaitForSeconds(TextOutputTime);
        yield return StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn(uint targetGold)
    {
        int startGold = int.Parse(GoldText.text);
        float elapsedTime = 0f;

        while (canvasGroup.alpha < 1.0f || elapsedTime < 1.0f / numberChangeSpeed)
        {
            if (canvasGroup.alpha < 1.0f)
                canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;

            elapsedTime += Time.deltaTime;
            GoldText.text = Mathf.Lerp(startGold, targetGold, elapsedTime * numberChangeSpeed).ToString("F0");

            yield return null;
        }

        canvasGroup.alpha = 1.0f;
        GoldText.text = targetGold.ToString();
    }

    IEnumerator FadeOut()
    {
        if (!isInvenOn)
        {
            while (canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
            canvasGroup.alpha = 0.0f;
        }
    }
    public void PanelToggle(bool Toggle)
    {
        if (Toggle)
        {
            canvasGroup.alpha = 1.0f;
            isInvenOn = true;
        }
        else
        {
            canvasGroup.alpha = 0.0f;
            isInvenOn = false;
        }
    }
}
