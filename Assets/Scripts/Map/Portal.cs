using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    [Header("'PlayerLocation' 자식이 필요하다.")]
    Direction direction;
    MapManager mapManager;
    GameObject mainCamera;
    [SerializeField]private float useCooldown = 1f;
    private float cooldownTime = 0f;


    private void Start()
    {
        mapManager = GameObject.FindAnyObjectByType<MapManager>();
        mainCamera = FindAnyObjectByType<MainCamera>().gameObject;

        string portalName = gameObject.name;
        switch (portalName)
        {
            case "RightPortal":
                direction = Direction.Right;
                break;
            case "LeftPortal":
                direction = Direction.Left;
                break;
            case "UpPortal":
                direction = Direction.Up;
                break;
            case "DownPortal":
                direction = Direction.Down;
                break;
            default:
                Debug.LogError("포탈이름이 잘못되었습니다. PortalName =: " + portalName);
                break;
        }
    }

    private void Update()
    {
        cooldownTime += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && cooldownTime > useCooldown)
        {
            mapManager.EnterPortal(direction);
            cooldownTime = 0f;
            mainCamera.transform.position = new Vector3(0,0,mainCamera.transform.position.z);

        }
    }

    public Direction GetDirection()
    {
        return direction;
    }

}

//TODO::              1.포탈 사운드(음향 효과)
//                           2.포탈 애니메이션(시각적 효과)


