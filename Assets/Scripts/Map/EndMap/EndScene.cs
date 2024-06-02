using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{

    Button restartButton;
    Button quitButton;
    TextMeshProUGUI mainText;
    TextMeshProUGUI playtimeText;
    TextMeshProUGUI killText;
    TextMeshProUGUI coinText;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        restartButton = child.GetComponent<Button>();
        child = transform.GetChild(1);
        quitButton = child.GetComponent<Button>();
        child = transform.GetChild(2);
        mainText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(4);
        child = child.transform.GetChild(0);
        playtimeText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        child = child.transform.GetChild(1);
        killText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        child = child.transform.GetChild(2);
        coinText = child.GetComponent<TextMeshProUGUI>();


        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(ReturnTitle);
    }

    private void Start()
    {
        if ( GameManager.Instance.gameClear )
        {
            mainText.text = " 게임 클리어 !! ";
        }
        else
        {
            mainText.text = " 게임 오버 ";
        }

        int totaltime = (int)GameManager.Instance.PlayTime;


        playtimeText.text = $"{ totaltime/60 } : { totaltime % 60} ";
        killText.text = $"{GameManager.Instance.KillCount}";
        coinText.text = $"{GameManager.Instance.goldCount}";
    }

    private void RestartGame()
    {
        GameManager.Instance.GameReset();
        SceneManager.LoadScene("Town",LoadSceneMode.Single);
    }

    private void ReturnTitle()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }


}
