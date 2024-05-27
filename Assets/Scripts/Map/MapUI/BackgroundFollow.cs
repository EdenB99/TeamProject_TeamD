using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class BackgroundFollow : MonoBehaviour
{
    public Sprite[] backgrounds;
    Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이 로딩될 때마다 함수 호출

        // cameraSize = new Vector2(mainCamera.orthographicSize * mainCamera.aspect, mainCamera.orthographicSize);

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateBackground();
    }

    void UpdateBackground()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "Town" || currentSceneName == "Tutorial")
        {
            GetComponent<SpriteRenderer>().sprite = backgrounds[1];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = backgrounds[0];
        }
    }

    private void LateUpdate()
    {

        UpdateBackgroundPosition();
    }



    public float lerpSpeed = 2f;

    void UpdateBackgroundPosition()
    {
        Vector2 backgroundPosition = new Vector2(mainCamera.transform.position.x * 0.9f, mainCamera.transform.position.y * 0.95f);
        transform.position = Vector2.Lerp(transform.position, backgroundPosition, lerpSpeed * Time.deltaTime);
    }




    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
