using UnityEngine;

public enum Direction { Right, Left, Up, Down }

[System.Serializable]
public class MapData
{
    public string sceneName;
    public bool hasUpPortal;
    public bool hasDownPortal;
    public bool hasLeftPortal;
    public bool hasRightPortal;
    public int mapX;
    public int mapY;
    public Direction enteredDirection;
    public bool IsLoaded { get; set; }
    public bool hasItem;
    public bool hasBossRoom;
    public bool hasShop;
    public bool hasHeal;

    public GameObject upPortalObject;
    public GameObject downPortalObject;
    public GameObject leftPortalObject;
    public GameObject rightPortalObject;

}

