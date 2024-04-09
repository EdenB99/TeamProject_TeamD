using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemIcon : MonoBehaviour
{
    private float idle_speed = 2f; // 속도
    private float height = 0.15f; // 왕복 높이
    private Vector3 startPos; // 초기 위치
    private float timeElapsed;
    

    Transform parent;

    private void Start()
    {
        if ( transform.parent != null )
        {
            parent = transform.parent.GetComponent<Transform>();
        }
        
    }

    private void Update()
    {
        // 위아래로 흔들린다. ( checkLR 변동 x )
        timeElapsed += Time.deltaTime;
        float newY = Mathf.Sin(timeElapsed * idle_speed) * height;
        transform.position = parent.position + new Vector3(0, newY+0.15f, 0);
    }


}
