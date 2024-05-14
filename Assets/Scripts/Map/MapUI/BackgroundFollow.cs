using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundFollow : MonoBehaviour
{
    public Transform background;
    public Sprite[] backgrounds;
    Camera mainCam;

    Dictionary<string, int> sceneToBackgroundIndex;

    private void Start()
    {
        mainCam = Camera.main;
        sceneToBackgroundIndex = new Dictionary<string, int>()
        {
            {"FirstLoadScene", 0},  // Å×½ºÆ®
            {"Tutorial", 1},
        };
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (sceneToBackgroundIndex.TryGetValue(currentSceneName, out int backgroundIndex))
        {
            GetComponent<SpriteRenderer>().sprite = backgrounds[backgroundIndex];
        }
    }

    private void LateUpdate()
    {
        background.transform.position = new Vector2(mainCam.transform.position.x *0.8f, mainCam.transform.position.y*0.9f);
    }
}
