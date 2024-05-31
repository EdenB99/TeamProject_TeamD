using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;


public class Portal : MonoBehaviour
{
    [Header("'PlayerLocation' �ڽ��� �ʿ��ϴ�.")]
    Direction direction;
    MapManager mapManager;
    GameObject mainCamera;
    SpriteRenderer spriteRenderer;
    private float useCooldown = 0.3f;
    private float cooldownTime = 0f;

    readonly int LightRangeID = Shader.PropertyToID("_LightRange");
    readonly int EmissionID = Shader.PropertyToID("_Emission");

    private void Awake()
    {
        Transform child = transform.GetChild(2);

        spriteRenderer = child.GetComponent<SpriteRenderer>();
    }

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
                Debug.LogError("��Ż�̸��� �߸��Ǿ����ϴ�. PortalName : " + portalName);
                break;
        }
    }


    private void Update()
    {
        cooldownTime += Time.deltaTime;
    }

    public void PortalColorChange(bool hasEnemies)
    {
        if (hasEnemies)
        {
            spriteRenderer.material.SetFloat(EmissionID, 0.0f);
            spriteRenderer.material.SetFloat(LightRangeID, 0.0f);
        }
        else
        {
            spriteRenderer.material.SetFloat(EmissionID, 0.5f);
            spriteRenderer.material.SetFloat(LightRangeID,0.0015f);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && cooldownTime > useCooldown)
        {
            MapData currentMap = mapManager.CurrentMap;
            if (currentMap != null)
            {
                if (!currentMap.hasEnemies)
                {
                    mapManager.EnterPortal(direction);
                    cooldownTime = 0f;
                    mainCamera.transform.position = new Vector3(0, 0, mainCamera.transform.position.z);
                }
            }
            else
            {
                // currentMap�� null�� ��� (ErrorScene�� ���ɼ��� ����)
                string currentSceneName = SceneManager.GetActiveScene().name;
                if (currentSceneName == "ErrorScene")
                {
                    Vector2Int startMapPosition = new Vector2Int(mapManager.WorldMapSize / 2, mapManager.WorldMapSize / 2);
                    mapManager.MapCheck(startMapPosition);
                }
            }
        }
    }

    public Direction GetDirection()
    {
        return direction;
    }

}

//TODO::              1.��Ż ����(���� ȿ��)
//                           2.��Ż �ִϸ��̼�(�ð��� ȿ��)


