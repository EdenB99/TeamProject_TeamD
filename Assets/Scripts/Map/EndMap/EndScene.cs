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
        Player player = FindAnyObjectByType<Player>();
        MainCamera maincamera = FindAnyObjectByType<MainCamera>();
        BackgroundFollow background = FindAnyObjectByType<BackgroundFollow>();
        player.transform.position = Vector3.zero;
        maincamera.transform.position = new Vector3(0, -5f, maincamera.transform.position.z);
        background.transform.position = new Vector3(0, -5f, background.transform.position.z);
        SceneManager.LoadScene("Town",LoadSceneMode.Single);

    }

    private void ReturnTitle()
    {
        SceneManager.LoadScene("Title", LoadSceneMode.Single);
    }


}
