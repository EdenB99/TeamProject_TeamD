using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIcon : MonoBehaviour
{
    private float idle_speed = 2f; // �ӵ�
    private float height = 0.15f; // �պ� ����
    private Vector3 startPos; // �ʱ� ��ġ

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
        // ���Ʒ��� ��鸰��. ( checkLR ���� x )
        float newY = Mathf.Sin(Time.time * idle_speed) * height;
        transform.position = parent.position + new Vector3(0, newY, 0);
    }


}
