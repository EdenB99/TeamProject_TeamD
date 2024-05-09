using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    [Header("'PlayerLocation' �ڽ��� �ʿ��ϴ�.")]
    Direction direction;
    MapManager mapManager;
    GameObject mainCamera;
    SpriteRenderer[] spriteRenderers;
    [SerializeField]private float useCooldown = 1f;
    private float cooldownTime = 0f;

    private void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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
                Debug.LogError("��Ż�̸��� �߸��Ǿ����ϴ�. PortalName =: " + portalName);
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
            MapData currentMap = mapManager.CurrentMap;
            if (currentMap != null && !currentMap.hasEnemies)
            {
                mapManager.EnterPortal(direction);
                cooldownTime = 0f;
                mainCamera.transform.position = new Vector3(0, 0, mainCamera.transform.position.z);
            }
            else if(currentMap != null &&  currentMap.hasEnemies) // �÷��̾��ϰ� �΋H���� �۵�, �Ⱥ΋H���� �۵��ǰ� �ϰ����,�ٸ� ȿ���� �ְ� ���������� ���°͵� ��������
            {
                for(int i = 0; i <spriteRenderers.Length; i++)
                {
                    spriteRenderers[i].color = Color.black;
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


