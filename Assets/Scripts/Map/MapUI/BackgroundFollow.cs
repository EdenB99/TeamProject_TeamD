using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundFollow : MonoBehaviour
{
    public Transform background;
    public Sprite[] backgrounds;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이 로딩될 때마다 함수 호출
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateBackground();
    }

    void UpdateBackground()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Tutorial")
        {
            GetComponent<SpriteRenderer>().sprite = backgrounds[1];
        }
        else if (currentSceneName == "Town")
        {
            GetComponent<SpriteRenderer>().sprite = backgrounds[2];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = backgrounds[0];
        }
    }

    private void LateUpdate()
    {
        background.transform.position = new Vector2(mainCam.transform.position.x * 0.8f, mainCam.transform.position.y * 0.9f + 1.5f);
    }
}
