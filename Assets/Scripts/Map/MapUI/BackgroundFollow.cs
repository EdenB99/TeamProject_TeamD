using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundFollow : MonoBehaviour
{
    public Transform background;
    public Sprite[] backgrounds;
    public GameObject boundaryObject;
    BoxCollider2D boundaryCollider;
    Camera maincamera;

    private void Awake()
    {
        maincamera = Camera.main;
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이 로딩될 때마다 함수 호출
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
        if (boundaryCollider == null)
        {
            FindBoundaryObject();
        }
        UpdateBackgroundPosition();
    }

    private void FindBoundaryObject()
    {
        GameObject foundBoundaryObject = GameObject.FindGameObjectWithTag("Boundary");
        if (foundBoundaryObject != null)
        {
            boundaryObject = foundBoundaryObject;
            boundaryCollider = boundaryObject.GetComponent<BoxCollider2D>();
        }
    }

    public float lerpSpeed = 2.0f;

    float backgroundWidth = 3;
    float backgroundHeight = 5;

    void UpdateBackgroundPosition()
    {
        Vector2 backgroundPosition = background.position;

        Bounds bounds = boundaryCollider.bounds;

        backgroundPosition.x = Mathf.Clamp(backgroundPosition.x, bounds.min.x + backgroundWidth / 2, bounds.max.x - backgroundWidth / 2);
        backgroundPosition.y = Mathf.Clamp(backgroundPosition.y, bounds.min.y + backgroundHeight / 2, bounds.max.y - backgroundHeight / 2);

        background.position = backgroundPosition;
        background.transform.position = Vector2.Lerp(background.transform.position, backgroundPosition, lerpSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
