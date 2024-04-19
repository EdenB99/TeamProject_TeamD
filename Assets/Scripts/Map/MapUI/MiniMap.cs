using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    CanvasGroup canvasGroup;
    CanvasGroup bigMapCanvasGroup;
    MapUI mapUIManager;

    //TODO:: MapUIMangager�� �̴ϸʰ����ϰ� BigMap�ϳ� �����
    public float CanvasGroupAlpha
    {
        get => canvasGroup.alpha;
        set
        {
            if(CanvasGroupAlpha> 0.1)
            {
                bigMapCanvasGroup.alpha = 1;
            }
            else
            {
                bigMapCanvasGroup.alpha = 0;
            }
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        mapUIManager = GameObject.FindAnyObjectByType<MapUI>();
        bigMapCanvasGroup = mapUIManager.gameObject.GetComponent<CanvasGroup>();
    }

}
