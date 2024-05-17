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
    Camera mainCamera;

    Vector2 cameraSize;
    Vector3 preCameraPosition;

    private void Awake()
    {
        mainCamera = Camera.main;
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이 로딩될 때마다 함수 호출

        cameraSize = new Vector2(mainCamera.orthographicSize * mainCamera.aspect, mainCamera.orthographicSize);
        preCameraPosition = mainCamera.transform.position;
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

    public float lerpSpeed = 2f;

    void UpdateBackgroundPosition()
    {
        Vector3 deltaPosition = mainCamera.transform.position - preCameraPosition;

        Vector2 backgroundPosition = new Vector2(background.position.x + deltaPosition.x * 0.8f, background.position.y + deltaPosition.y);

        if (boundaryCollider != null)
        {
            Vector2 backgroundSize = background.GetComponent<Renderer>().bounds.size;
            Bounds bounds = boundaryCollider.bounds;

            backgroundPosition.x = Mathf.Clamp(backgroundPosition.x, bounds.min.x + cameraSize.x, bounds.max.x - cameraSize.x);
            backgroundPosition.y = Mathf.Clamp(backgroundPosition.y, bounds.min.y + cameraSize.y, bounds.max.y - cameraSize.y);
        }

        background.position = Vector2.Lerp(background.position, backgroundPosition, lerpSpeed * Time.deltaTime);

        preCameraPosition = mainCamera.transform.position;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
