using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//빠른이동은 canvasGroup레이캐스트 활용해서 만들기
public class MapUI : MonoBehaviour
{
    private CanvasGroup canvasGroupUI;
    private CanvasGroup canvasGroupMini;

    [SerializeField] private MapManager mapManager;
    [SerializeField] private GameObject mapTilePrefab;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private Sprite healSprite;
    [SerializeField] private Sprite shopSprite;
    [SerializeField] private Sprite bossSprite;
    [SerializeField] private Sprite portalSprites;

    private Image[,] mapTiles;
    private Dictionary<string, GameObject> portalIcons = new Dictionary<string, GameObject>();

    private RectTransform MapPosition;
    private bool isDragging = false;
    private Vector2 MousePosition;
    [SerializeField] private float dragSpeed = 2f;

    private void Start()
    {
        mapManager = GameObject.FindObjectOfType<MapManager>();
        canvasGroupMini = GameObject.FindAnyObjectByType<MiniMap>().GetComponent<CanvasGroup>();
        canvasGroupUI = GetComponent<CanvasGroup>();
        MapPosition = transform.GetChild(0).GetComponent<RectTransform>();

        mapTiles = new Image[mapManager.WorldMapSize, mapManager.WorldMapSize];
        GenerateMapUI();
    }



    private void GenerateMapUI()
    {
        int worldMapSize = mapManager.WorldMapSize;

        float tileWidth = MapPosition.rect.width / worldMapSize;
        float tileHeight = MapPosition.rect.height / worldMapSize;

        for (int y = 0; y < worldMapSize; y++)
        {
            for (int x = 0; x < worldMapSize; x++)
            {

                GameObject mapTile = Instantiate(mapTilePrefab, MapPosition);
                RectTransform tileTransform = mapTile.GetComponent<RectTransform>();

                float tilePositionX = -MapPosition.rect.width / 2 + tileWidth * x;
                float tilePositionY = -MapPosition.rect.height / 2 + tileHeight * y;

                tileTransform.anchoredPosition = new Vector2(tilePositionX, tilePositionY);
                tileTransform.sizeDelta = new Vector2(tileWidth, tileHeight);

                Image tileImage = mapTile.GetComponent<Image>();
                tileImage.color = new Color(1f, 1f, 1f, 0f);

                mapTiles[x, y] = tileImage;
            }
        }
    }

    private void Update()
    {//TODO:: 나중에 바꾸기
        UpdateMapUI();
        DragMap();

    }

    private void DragMap()
    {
        if (canvasGroupUI.alpha > 0 && canvasGroupUI.interactable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                MousePosition = Input.mousePosition;
                isDragging = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }

            if (isDragging)
            {
                Vector2 currentMousePosition = Input.mousePosition;
                Vector2 deltaMousePosition = currentMousePosition - MousePosition;


                MapPosition.anchoredPosition += new Vector2(
                                 deltaMousePosition.x * dragSpeed,
                                 deltaMousePosition.y * dragSpeed
                             );

                MousePosition = currentMousePosition;
            }
        }
    }

    //TODO:: 키를 통해 열고닫을 때 마다 맵업데이트
    public void ShowMap()
    {
        UpdateMapUI();
        canvasGroupUI.alpha = 1f;
        canvasGroupMini.alpha = 0f;
    }

    
    public void HideMap()
    {
        canvasGroupUI.alpha = 0f;
        canvasGroupUI.blocksRaycasts = false;
        canvasGroupMini.alpha = 1f;

        UpdateMapUI();
    }


    //TODO:: 빠른이동.
    public void FastTrevelEnble()
    {
        UpdateMapUI();
        canvasGroupUI.alpha = 1f;
        canvasGroupMini.alpha = 0f;
        canvasGroupUI.blocksRaycasts = true;
        //맵을누르면 빠른이동이 가능해지게하는 변수 bool?
    }


    private void UpdateMapUI()
    {
        for (int y = 0; y < mapManager.WorldMapSize; y++)
        {
            for (int x = 0; x < mapManager.WorldMapSize; x++)
            {
                MapData mapData = mapManager.GetMapData(x, y);
                // 맵 데이터를 가져오기
                if (mapData != null && mapData.IsLoaded)
                {
                    mapTiles[x, y].color = Color.white; // 플레이어가 있는 맵은 보이도록 설정

                    // 맵에 상호작용 가능한 요소 표시
                    //TODO:: 앵커포지션 바꾸기
                    if (mapData.hasItem)
                    {
                        GameObject itemIcon = new("ItemIcon");
                        itemIcon.transform.SetParent(mapTiles[x, y].transform);
                        itemIcon.AddComponent<Image>().sprite = itemSprite;
                        RectTransform itemRect = itemIcon.GetComponent<RectTransform>();
                        itemRect.anchorMin = new Vector2(0.5f, 0.5f);
                        itemRect.anchorMax = new Vector2(0.5f, 0.5f);
                        itemRect.anchoredPosition = new Vector2(-10f, -10f);
                    }
                    if (mapData.hasShop)
                    {
                        GameObject shopIcon = new("ShopIcon");
                        shopIcon.transform.SetParent(mapTiles[x, y].transform);
                        shopIcon.AddComponent<Image>().sprite = shopSprite;
                        RectTransform shopRect = shopIcon.GetComponent<RectTransform>();
                        shopRect.anchorMin = new Vector2(0.5f, 0.5f);
                        shopRect.anchorMax = new Vector2(0.5f, 0.5f);
                        shopRect.anchoredPosition = new Vector2(10f, -10f);
                    }
                    if (mapData.hasBossRoom)
                    {
                        GameObject bossIcon = new("BossIcon");
                        bossIcon.transform.SetParent(mapTiles[x, y].transform);
                        bossIcon.AddComponent<Image>().sprite = bossSprite;
                        RectTransform bossRect = bossIcon.GetComponent<RectTransform>();
                        bossRect.anchorMin = new Vector2(0.5f, 0.5f);
                        bossRect.anchorMax = new Vector2(0.5f, 0.5f);
                        bossRect.anchoredPosition = new Vector2(0f, 10f);
                    }


                    ShowPortalIcon(mapData, Direction.Up, new Vector2(0f, 275f));
                    ShowPortalIcon(mapData, Direction.Down, new Vector2(0f, -265f));
                    ShowPortalIcon(mapData, Direction.Left, new Vector2(-480f, 0f));
                    ShowPortalIcon(mapData, Direction.Right, new Vector2(480f, 0f));
                }
            }
        }
    }

    //포탈프리팹 생성안되는 버그
    private void ShowPortalIcon(MapData mapData, Direction direction, Vector2 iconPosition)
    {
        bool hasPortal = false;

        switch (direction)
        {
            case Direction.Up:
                hasPortal = mapData.hasUpPortal;
                break;
            case Direction.Down:
                hasPortal = mapData.hasDownPortal;
                break;
            case Direction.Left:
                hasPortal = mapData.hasLeftPortal;
                break;
            case Direction.Right:
                hasPortal = mapData.hasRightPortal;
                break;
        }

        if (hasPortal)
        {
            string portalName = $"{direction}PortalIcon";
            Transform parentTransform = mapTiles[mapData.mapX, mapData.mapY].transform;

            if (!portalIcons.ContainsKey(portalName))
            {
                GameObject portalIcon = new GameObject(portalName);
                portalIcon.transform.SetParent(parentTransform);

                Image portalImage = portalIcon.AddComponent<Image>();
                portalImage.sprite = portalSprites;

                RectTransform portalRect = portalIcon.GetComponent<RectTransform>();
                portalRect.anchorMin = new Vector2(0.5f, 0.5f);
                portalRect.anchorMax = new Vector2(0.5f, 0.5f);
                portalRect.anchoredPosition = iconPosition;

                float iconSize = 20f;
                portalRect.sizeDelta = new Vector2(iconSize, iconSize);

                portalIcons[portalName] = portalIcon;
            }
        }
    }

    /// <summary>
    /// TODO:: 빠른이동 구현
    /// </summary>
    public void FastTravel()
    {


    }

}