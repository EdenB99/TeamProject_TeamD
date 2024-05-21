using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScene : MonoBehaviour
{

    Button restartButton;
    Button quitButton;


    private void Awake()
    {
        Transform child = transform.GetChild(0);
        restartButton = child.GetComponent<Button>();
        child = transform.GetChild(1);
        quitButton = child.GetComponent<Button>();

        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(ReturnTitle);
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
