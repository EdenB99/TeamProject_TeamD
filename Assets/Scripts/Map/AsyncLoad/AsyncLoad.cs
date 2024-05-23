using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AsyncLoad : MonoBehaviour
{
    /// <summary>
    /// ����Ƽ���� �񵿱� ��� ó���� ���� �ʿ��� Ŭ����
    /// </summary>
    AsyncOperation async;

    /// <summary>
    /// ���� ����� �ڷ�ƾ
    /// </summary>
    IEnumerator loadingTextCoroutine;

    /// <summary>
    /// �ε� �Ϸ� ǥ��(true�� �Ϸ�, false �̿�)
    /// </summary>
    //bool loadingDone = false;

    // UI
    Slider loadingSlider;
    TextMeshProUGUI loadingText;


    void Start()
    {
        loadingSlider = FindAnyObjectByType<Slider>();
        loadingText = FindAnyObjectByType<TextMeshProUGUI>();
    }

    private void Update()
    {
        // �����̴��� value�� loadRatio�� �� ������ ��� ����
        /*if (loadingSlider.value < loadRatio)
        {
            loadingSlider.value += Time.deltaTime * loadingBarSpeed;
        }*/
    }
}
