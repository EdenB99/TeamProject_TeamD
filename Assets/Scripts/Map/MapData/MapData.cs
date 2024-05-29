using Cainos.PixelArtPlatformer_VillageProps;
using System.Collections.Generic;
using UnityEngine;

public enum Direction { Right, Left, Up, Down }

[System.Serializable]
public class SaveItemData
{
    public ItemCode itemCode;
    public Vector2 itemPositions; // 아이템 위치
}

[System.Serializable]
public class MapData
{
    public string sceneName;
    public int mapX;
    public int mapY;
    public Direction enteredDirection;

    public bool hasEnemies = false;
    public bool isVisited = false;
    public bool hasItem;
    public bool hasQuickPortal = false;
    public bool isTrevel = false;
    public bool isNextStageRoom;
    public bool hasHeal;
    public GameObject upPortalObject;
    public GameObject downPortalObject;
    public GameObject leftPortalObject;
    public GameObject rightPortalObject;

    public List<SaveItemData> mapItemDatas;
    public Dictionary<int, Vector3> itemPositions = new Dictionary<int, Vector3>();
    public Chest chest;
    public bool isChestOpen = false;


    private bool hasUpPortal;
    public bool HasUpPortal
    {
        get
        {
            if (upPortalObject == null)
                return false;
            return hasUpPortal;
        }
        set
        {
            hasUpPortal = value;
            if (upPortalObject != null)
                upPortalObject.SetActive(value);
        }
    }

    private bool hasDownPortal;
    public bool HasDownPortal
    {
        get
        {
            if (downPortalObject == null)
                return false;
            return hasDownPortal;
        }
        set
        {
            hasDownPortal = value;
            if (downPortalObject != null)
                downPortalObject.SetActive(value);
        }
    }

    private bool hasLeftPortal;
    public bool HasLeftPortal
    {
        get
        {
            if (leftPortalObject == null)
                return false;
            return hasLeftPortal;
        }
        set
        {
            hasLeftPortal = value;
            if (leftPortalObject != null)
                leftPortalObject.SetActive(value);
        }
    }

    private bool hasRightPortal;
    public bool HasRightPortal
    {
        get
        {
            if (rightPortalObject == null)
                return false;
            return hasRightPortal;
        }
        set
        {
            hasRightPortal = value;
            if (rightPortalObject != null)
                rightPortalObject.SetActive(value);
        }
    }


}

