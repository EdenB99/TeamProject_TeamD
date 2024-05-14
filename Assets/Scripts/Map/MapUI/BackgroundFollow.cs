using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform background;
    Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        background.transform.position = new Vector2(mainCam.transform.position.x *0.8f, mainCam.transform.position.y);
    }
}
