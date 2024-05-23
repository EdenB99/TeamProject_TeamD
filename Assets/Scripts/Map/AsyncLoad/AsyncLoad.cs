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
    /// 유니티에서 비동기 명령 처리를 위해 필요한 클래스
    /// </summary>
    AsyncOperation async;

    /// <summary>
    /// 글자 변경용 코루틴
    /// </summary>
    IEnumerator loadingTextCoroutine;

    /// <summary>
    /// 로딩 완료 표시(true면 완료, false 미완)
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
        // 슬라이더의 value는 loadRatio가 될 때까지 계속 증가
        /*if (loadingSlider.value < loadRatio)
        {
            loadingSlider.value += Time.deltaTime * loadingBarSpeed;
        }*/
    }
}
