using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform background;
    public SpriteRenderer backgroundSprite;
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        backgroundSprite = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        background.transform.position = new Vector2(mainCam.transform.position.x *0.8f, mainCam.transform.position.y*0.9f);
    }

    void backgroundRefresh(Sprite sprite)
    {
        backgroundSprite.sprite = sprite;
    }
}
