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
    TextMeshPro mainText;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        restartButton = child.GetComponent<Button>();
        child = transform.GetChild(1);
        quitButton = child.GetComponent<Button>();
        child = transform.GetChild(2);
        mainText = GetComponent<TextMeshPro>();

        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(ReturnTitle);
    }

    private void Start()
    {
        if ( GameManager.Instance.gameClear )
        {
            mainText.text = " 게임 오버 ";
        }
        else
        {
            mainText.text = " 게임 클리어 !! ";
        }
        
    }

    private void RestartGame()
    {
        SceneManager.LoadScene("Town",LoadSceneMode.Single);
    }

    private void ReturnTitle()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }


}
