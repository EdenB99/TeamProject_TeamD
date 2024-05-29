using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//빠른이동은 canvasGroup레이캐스트 활용해서 만들기
public class MapUI : MonoBehaviour
{
    private CanvasGroup canvasGroupUI;
    private CanvasGroup canvasGroupMini;

    [SerializeField] private MapManager mapManager;
    [SerializeField] private GameObject mapTilePrefab;
    [SerializeField] private Sprite currentMapSprite;
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private Sprite healSprite;
    [SerializeField] private Sprite nextStageSprite;
    [SerializeField] private Sprite portalSprites;
    [SerializeField] private Sprite quickPortalSprite;
    private Image[,] mapTiles;

    private RectTransform mapPosition;

    RectTransform currentMapTileRect;
    Vector3 currentMapTilePosition;

    private bool isDragging = false;
    private Vector2 MousePosition;
    [SerializeField] private float dragSpeed = 2f;

    public bool isQuickTrevelActive = false;

    //외부
    IngameUI ingameUI;
    Player player;
    private void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        canvasGroupMini = FindAnyObjectByType<MiniMap>().GetComponent<CanvasGroup>();
        canvasGroupUI = GetComponent<CanvasGroup>();
        mapPosition = transform.GetChild(0).GetComponent<RectTransform>();
        ingameUI = FindAnyObjectByType<IngameUI>();
        mapTiles = new Image[mapManager.WorldMapSize, mapManager.WorldMapSize];
        GenerateMapUI();
    }
    private void Update()
    {
        DragMap();

    }


    //지도 맵전체 생성하기======================================================================
    private void GenerateMapUI()
    {
        int worldMapSize = mapManager.WorldMapSize;

        float tileWidth = mapPosition.rect.width / worldMapSize;
        float tileHeight = mapPosition.rect.height / worldMapSize;

        for (int y = 0; y < worldMapSize; y++)
        {
            for (int x = 0; x < worldMapSize; x++)
            {



                GameObject mapTile = Instantiate(mapTilePrefab, mapPosition);
                RectTransform tileTransform = mapTile.GetComponent<RectTransform>();

                float tilePositionX = -mapPosition.rect.width / 2 + tileWidth * x;
                float tilePositionY = -mapPosition.rect.height / 2 + tileHeight * y;

                GameObject currentMapIndicator = new GameObject("CurrentMapIndicator");
                currentMapIndicator.transform.SetParent(mapTile.transform);
                Image currentMapImage = currentMapIndicator.AddComponent<Image>();
                currentMapImage.sprite = currentMapSprite;
                currentMapIndicator.transform.localScale = new Vector3(1f, 1f, 1f);
                Outline currentOutline = currentMapImage.AddComponent<Outline>();
                currentOutline.effectColor = Color.yellow;
                currentMapImage.enabled = false; // 초기에는 비활성화


                // UI 이미지 위치 및 크기 조정
                RectTransform indicatorRect = currentMapIndicator.GetComponent<RectTransform>();
                indicatorRect.anchorMin = Vector2.zero;
                indicatorRect.anchorMax = Vector2.one;
                indicatorRect.offsetMin = Vector2.zero;
                indicatorRect.offsetMax = Vector2.zero;

                tileTransform.anchoredPosition = new Vector2(tilePositionX, tilePositionY);
                tileTransform.sizeDelta = new Vector2(tileWidth, tileHeight);
                Image tileImage = mapTile.GetComponent<Image>();
                tileImage.color = new Color(1f, 1f, 1f, 0f); // 초기에 모든 맵 타일을 투명하게 설정

                mapTiles[x, y] = tileImage;
            }
        }
    }


    //맵 드래그하기
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


                mapPosition.anchoredPosition += new Vector2(
                                 deltaMousePosition.x * dragSpeed,
                                 deltaMousePosition.y * dragSpeed
                             );

                MousePosition = currentMousePosition;
            }
        }
    }

    //TODO::UpdateMapUI방식바꾸기
    //맵 열기, 닫기, 빠른이동===================================================================
    public void ShowMap()
    {
        UpdateMapUI();
        canvasGroupUI.alpha = 1f;
        canvasGroupMini.alpha = 0f;
        mapPosition.anchoredPosition = currentMapTilePosition;
        canvasGroupUI.blocksRaycasts = true;
        canvasGroupUI.interactable = true;
        ingameUI.mapToggle = true;
        isDragging = false;

    }


    public void HideMap()
    {
        canvasGroupUI.alpha = 0f;
        canvasGroupUI.blocksRaycasts = false;
        canvasGroupUI.interactable = false;
        isQuickTrevelActive = false;
        ingameUI.mapToggle = false;
        isDragging = false;
        canvasGroupMini.alpha = 1f;
        UpdateMapUI();
    }


    //TODO:: 빠른이동.
    public void QuickTrevel()
    {
        UpdateMapUI();
        canvasGroupUI.alpha = 1f;
        canvasGroupMini.alpha = 0f;
        mapPosition.anchoredPosition = currentMapTilePosition;
        canvasGroupUI.blocksRaycasts = true;
        canvasGroupUI.interactable = true;
        isQuickTrevelActive = true;
        ingameUI.mapToggle = true;
        isDragging = false;

            RegisterMapButtonEvents(); // 맵 버튼 이벤트 등록 함수 호출

    }


    private void RegisterMapButtonEvents()
    {

        for (int y = 0; y < mapManager.WorldMapSize; y++)
        {
            for (int x = 0; x < mapManager.WorldMapSize; x++)
            {
                MapData mapData = mapManager.GetMapData(x, y);
                if (mapData != null && mapData.isVisited )
                {
                    Button mapButton = mapTiles[x, y].GetComponent<Button>();
                    if (mapButton == null)
                    {
                        mapButton = mapTiles[x, y].gameObject.AddComponent<Button>();
                    }
                    mapButton.onClick.AddListener(() => OnMapButtonClicked(mapData));
                }
            }
        }

    }

    private void OnMapButtonClicked(MapData mapData)
    {
        if (mapData.isVisited && isQuickTrevelActive && mapData.hasQuickPortal)
        {
            // 해당 맵의 좌표로 이동
            mapManager.QuickTrevel(mapData);
        }
    }



    //맵 그리기 관련 함수=======================================================================

    //맵 상태 업데이트
    public void UpdateMapUI()
    {
        for (int y = 0; y < mapManager.WorldMapSize; y++)
        {
            for (int x = 0; x < mapManager.WorldMapSize; x++)
            {
                MapData mapData = mapManager.GetMapData(x, y);
                if (mapData != null && mapData.isVisited)
                {
                    mapTiles[x, y].color = Color.white;

                    // 현재 맵 표시용 스프라이트 활성화/비활성화
                    bool isCurrentMap = (mapData == mapManager.CurrentMap);
                    mapTiles[x, y].transform.Find("CurrentMapIndicator").gameObject.SetActive(isCurrentMap);
                    if (isCurrentMap)
                    {
                        Image currentMapImage = mapTiles[x, y].transform.Find("CurrentMapIndicator").GetComponent<Image>();
                        currentMapImage.enabled = true;
                        //ShowMap시 맵을 플레이어가 있는 곳으로 바꾸기 위한 변수 저장
                        currentMapTileRect = mapTiles[x, y].GetComponent<RectTransform>();
                        currentMapTilePosition = -currentMapTileRect.anchoredPosition;

                    }

                    

                    ShowPortalIcon(mapData, Direction.Up, new Vector2(0f, 275f));
                    ShowPortalIcon(mapData, Direction.Down, new Vector2(0f, -265f));
                    ShowPortalIcon(mapData, Direction.Left, new Vector2(-480f, 0f));
                    ShowPortalIcon(mapData, Direction.Right, new Vector2(480f, 0f));


                    ShowInteractiveIcon(mapData, mapData.isNextStageRoom, nextStageSprite, new Vector2(0f, 10f));
                    ShowInteractiveIcon(mapData, mapData.hasQuickPortal, quickPortalSprite, new Vector2(0f, -10f));
                    // 아이템 아이콘 표시
                    ShowItemIcons(mapData);


                }
                else
                {
                    mapTiles[x, y].color = new Color(1f, 1f, 1f, 0f);
                    mapTiles[x, y].transform.Find("CurrentMapIndicator").gameObject.SetActive(false);
                }
            }
        }
    }

    //아이콘 생성
    private void ShowInteractiveIcon(MapData mapData, bool hasInteractive, Sprite iconSprite, Vector2 iconPosition)
    {
        Transform iconTransform = mapTiles[mapData.mapX, mapData.mapY].transform.Find("InteractiveIcon");

        if (hasInteractive)
        {
            if (iconTransform == null)
            {
                // 아이콘 GameObject 생성
                GameObject iconObject = new GameObject("InteractiveIcon");
                iconObject.transform.SetParent(mapTiles[mapData.mapX, mapData.mapY].transform);

                Image iconImage = iconObject.AddComponent<Image>();
                iconImage.sprite = iconSprite;

                RectTransform iconRect = iconObject.GetComponent<RectTransform>();
                iconRect.anchorMin = new Vector2(0.5f, 0.5f);
                iconRect.anchorMax = new Vector2(0.5f, 0.5f);
                iconRect.anchoredPosition = iconPosition;
            }
            else
            {
                // 기존 아이콘 GameObject 재사용
                Image iconImage = iconTransform.GetComponent<Image>();
                iconImage.sprite = iconSprite;
                iconTransform.GetComponent<RectTransform>().anchoredPosition = iconPosition;
                iconTransform.gameObject.SetActive(true);
            }
        }
        else if (iconTransform != null)
        {
            // 기존 아이콘 GameObject 비활성화
            iconTransform.gameObject.SetActive(false);
        }
    }

    //포탈 아이콘 만들기
    private void ShowPortalIcon(MapData mapData, Direction direction, Vector2 iconPosition)
    {
        bool hasPortal = false;

        switch (direction)
        {
            case Direction.Up:
                hasPortal = mapData.HasUpPortal;
                break;
            case Direction.Down:
                hasPortal = mapData.HasDownPortal;
                break;
            case Direction.Left:
                hasPortal = mapData.HasLeftPortal;
                break;
            case Direction.Right:
                hasPortal = mapData.HasRightPortal;
                break;
        }

        if (hasPortal)
        {
            string portalName = $"{direction}PortalIcon_{mapData.mapX}_{mapData.mapY}"; // 고유한 포탈 이름 생성
            Transform parentTransform = mapTiles[mapData.mapX, mapData.mapY].transform;

            if (parentTransform.Find(portalName) == null) // 해당 포탈 아이콘이 없는 경우에만 생성
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
            }
        }
    }



    // 아이템 아이콘 표시
    private void ShowItemIcons(MapData mapData)
    {
        Transform iconTransform = mapTiles[mapData.mapX, mapData.mapY].transform.Find("ItemIcon");

        if (mapData.mapItemDatas != null && mapData.mapItemDatas.Count > 0)
        {
            if (iconTransform == null)
            {
                // 아이콘 GameObject 생성
                GameObject iconObject = new GameObject("ItemIcon");
                iconObject.transform.SetParent(mapTiles[mapData.mapX, mapData.mapY].transform);

                Image iconImage = iconObject.AddComponent<Image>();
                iconImage.sprite = itemSprite;

                RectTransform iconRect = iconObject.GetComponent<RectTransform>();
                iconRect.anchorMin = new Vector2(0.5f, 0.5f);
                iconRect.anchorMax = new Vector2(0.5f, 0.5f);
                iconRect.anchoredPosition = new Vector2(0f, 0f);
            }
            else
            {
                // 기존 아이콘 GameObject 재사용
                Image iconImage = iconTransform.GetComponent<Image>();
                iconImage.sprite = itemSprite;
                iconTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
                iconTransform.gameObject.SetActive(true);
            }
        }
        else if (iconTransform != null)
        {
            // 기존 아이콘 GameObject 비활성화
            iconTransform.gameObject.SetActive(false);
        }
    }


}