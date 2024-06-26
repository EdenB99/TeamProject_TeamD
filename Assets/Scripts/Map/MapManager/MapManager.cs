using Cainos.PixelArtPlatformer_VillageProps;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MapManager : MonoBehaviour
{
    //TODO(끝내지못한것들)
    //아이콘이 여러개 일 시 겹쳐지는 오류 존재, 클릭을 위한 무언가의 이미지가 필요함 (MapUI에,현재는 빠른이동 아이콘을 눌러야 이동)
    //보상 상자가 맵나갔다오면 사라지는 오류(chest로 받아둬서 null 발생)
    // 25map의 chest가 초기화 되는 오류
    //       맵에 맵25에서 위포탈로 맵이만들어졌지만 해당맵에 밑포탈이 없어 연결끊김, 해당 맵에서 왼쪽으로 
    //       맵이 두개 만들어졌지만 두개 다 밑의 맵에 위 포탈이 없어 이어지지 않아, 3개의 빈 맵이 만들어지게 됨.
    //       하지만3개는 이어져있기 때문에 삭제되지도않고 그대로 맵생성 및 다음맵을 받아 다음스테이지로 넘어가지 못함
    //       역순 순회코드에 맵이 스타트맵과 이어져있는 지 체크하면 될 것 같음.

    /*

    찾은 오류---
    1.스타트시 인게임슬롯에서 워닝발생
    2.몬스터에 대미지가 두번씩 들어가는 오류
    3.타이틀에서 세팅을 누르면 그뒤로 아무것도 못하는 오류
    5. 죽어도 들고있는 무기는 그대로인 오류
    6. 씬체인저에 무기가 잡히지 않는 오류

    

*/
    //오류 해결 - 5월 22일부터 작성시작
    //내가 해결한 것
    /*
    1.스타트시 플레이어가 땅에박히는 오류
    2.Town에서 백그라운드가 밖에 나가있는 오류
    3.맵의 생성이 무한재귀하는 오류
    4.Town,BossMap에서 백그라운드가 밖으로 튀어나가는 오류
    5. 튜토리얼의 rightPortal이 작동하지 않는 오류
    6. npc를 만난 후 f를 누르면 전에 대화한 npc와의 대화스크립트가 작동하는 오류, 상점, 상자도 발생
    7. 빠른맵 이동이 적이 있어도 사용가능한 것, 아이템 저장 및 총알 삭제
    8. 빠른이동시 아이템에 문제가 생기는 오류

    */

    /*
    1.무기 교체시 시도가 너무 많이일어나는 문제
    2. 무기 장비중 손에 있는 무기 E 표시
    3. 무기 2개 착용시 새로운 무기 착용하면 손에 들지 않은 무기를 해제하고 인벤에도 표시
    4. 비어있는 슬롯 클릭시 버튼 표시하지 않음
    5. 머리 중앙에 있는 아이템은 안먹어지는 오류
    6.아이템을 장착해제해도 손에 들고있으면 사용가능함
    7.창이랑 검이랑 대미지가 같음
    8.왕의 검 프리팹 문제
    9.백그라운드가 가끔 메인카메라 밖으로 나가짐 
    10.빈아이템 칸에 마우스 클릭하면 창이 열림
    11.마찬가지로 다른아이템으로 교체해도 E가 사라지지않는 오류 존재
    12.몬스터의 공격 스프라이트가 깨지는 오류
    13.절대반지 장착 해제해도 능력치가 남음
    14.튜토리얼에서 상점창의 확인창이 인벤토리보다 뒤에있어서 누르려면 인벤토리를 꺼야하는 오류
    15. 상점에 아이템을 팔면 재산이 0원이 되는 오류
    16. 인게임 상점창이 골드를 가리는 오류
    17. 튜토리얼의 백그라운드가 화면밖으로 나가는 오류
    18. 로딩씬에서 게이지가 안차는 오류
    19.장비를 장착하고 클리어 후 타이틀->타운 들어가니 오류 발생

    */
    [Header("변수")]

    private Player player;
    [Header("전체 맵 개수(+-1~3)")]
    [SerializeField] private int mapSize = 20;
    [Header("월드맵크기(MapSize*MapSize)")]
    [SerializeField] private int worldMapSize = 7;
    public int WorldMapSize => worldMapSize;
    [SerializeField] private int currentMapCount = 0;

    private int regenerateMapCount = 0;
    [Header("스테이지맵 프리팹")]
    [SerializeField] private GameObject[] mapPrefabs;
    private MapData[] mapScenes;
    private Dictionary<Vector2Int, MapData> worldMap = new Dictionary<Vector2Int, MapData>();
    private HashSet<string> usedMapScenes = new HashSet<string>();
    private int centerX;
    private int centerY;
    Vector2Int currentPosition;
    //맵 생성 제어
    private bool isGeneratingMap = false;

    //활성화된 상자 변수
    Chest selectedChest;


    //특수 맵 관련
    private bool hasNextStageMap = false;
    MapData startMapData;
    //정해진 스타트맵
    MapData startMap;

    [Header("시작맵 프리팹")]
    [SerializeField] private GameObject[] startMapPrefabs;
    private MapData[] startMapScenes;
    [Header("다음스테이지맵 프리팹")]
    [SerializeField] private GameObject[] nextStageMapPrefabs;
    private MapData[] nextStageMapScenes;

    private MapData currentMap;
    public MapData CurrentMap
    {
        get { return currentMap; }
        private set { currentMap = value; }
    }
    private MapUI mapUI;
    //적관련
    private GameObject enemyParent;
    EnemyBase[] enemies;
    protected bool isEnemyDead = false;

    //포탈 관련
    private Portal[] portals;
    private Dictionary<Portal, SpriteRenderer[]> portalSpriteRenderers = new Dictionary<Portal, SpriteRenderer[]>();

    // 카메라
    private MainCamera mainCamera;

    //빠른이동 포탈
    GameObject quickPortal;

    private void Awake()
    {
        centerX = worldMapSize / 2;
        centerY = worldMapSize / 2;
        AutoFillMapData();
        LoadStartMap();
    }

    //시작맵 랜덤으로 하나 정하기
    private void LoadStartMap()
    {
        MapData randomStartMap = startMapScenes[UnityEngine.Random.Range(0, startMapScenes.Length)];

        startMap = new MapData
        {
            mapX = centerX,
            mapY = centerY,
            sceneName = randomStartMap.sceneName,
            isVisited = true,
            hasQuickPortal = true,
            HasUpPortal = randomStartMap.HasUpPortal,
            HasDownPortal = randomStartMap.HasDownPortal,
            HasLeftPortal = randomStartMap.HasLeftPortal,
            HasRightPortal = randomStartMap.HasRightPortal,
            upPortalObject = randomStartMap.upPortalObject,
            downPortalObject = randomStartMap.downPortalObject,
            leftPortalObject = randomStartMap.leftPortalObject,
            rightPortalObject = randomStartMap.rightPortalObject
        };

        worldMap[new Vector2Int(centerX, centerY)] = startMap;

        startMapData = new MapData
        {
            mapX = centerX,
            mapY = centerY,
            isVisited = true,
            hasQuickPortal = true,
            sceneName = randomStartMap.sceneName,
            HasUpPortal = randomStartMap.HasUpPortal,
            HasDownPortal = randomStartMap.HasDownPortal,
            HasLeftPortal = randomStartMap.HasLeftPortal,
            HasRightPortal = randomStartMap.HasRightPortal,
            upPortalObject = randomStartMap.upPortalObject,
            downPortalObject = randomStartMap.downPortalObject,
            leftPortalObject = randomStartMap.leftPortalObject,
            rightPortalObject = randomStartMap.rightPortalObject
        };


        MapCheck(new Vector2Int(centerX, centerY));

    }





    private void Start()
    {
        player = GameManager.Instance.Player;
        mainCamera = GameManager.Instance.MainCamera;
        mapUI = GameObject.FindAnyObjectByType<MapUI>();

        StartCoroutine(GenerateWorldMapCoroutine());
    }

    //프리팹을 기반으로 자동으로 MapData[] 씬들을 채워넣는 함수
    private void AutoFillMapData()
    {
        // startMapScenes 처리
        startMapScenes = new MapData[startMapPrefabs.Length];
        for (int i = 0; i < startMapPrefabs.Length; i++)
        {
            GameObject mapPrefab = startMapPrefabs[i];
            if (mapPrefab == null)
            {
                Debug.LogWarning($"StartMapScenes {i}가 비어있습니다.");
                continue;
            }

            MapData mapData = new MapData();
            mapData.sceneName = mapPrefab.name;

            // 포탈 정보 수집
            Transform upPortalTransform = mapPrefab.transform.Find("UpPortal");
            mapData.HasUpPortal = upPortalTransform != null;
            mapData.upPortalObject = upPortalTransform?.gameObject;

            Transform downPortalTransform = mapPrefab.transform.Find("DownPortal");
            mapData.HasDownPortal = downPortalTransform != null;
            mapData.downPortalObject = downPortalTransform?.gameObject;

            Transform leftPortalTransform = mapPrefab.transform.Find("LeftPortal");
            mapData.HasLeftPortal = leftPortalTransform != null;
            mapData.leftPortalObject = leftPortalTransform?.gameObject;

            Transform rightPortalTransform = mapPrefab.transform.Find("RightPortal");
            mapData.HasRightPortal = rightPortalTransform != null;
            mapData.rightPortalObject = rightPortalTransform?.gameObject;

            // 다른 포탈 정보도 동일하게 수집

            startMapScenes[i] = mapData;
        }

        //mapScenes 처리
        mapScenes = new MapData[mapPrefabs.Length];

        for (int i = 0; i < mapPrefabs.Length; i++)
        {
            GameObject mapPrefab = mapPrefabs[i];

            if (mapPrefab == null)
            {
                Debug.LogWarning($"MapScenes {i}가 비어있습니다.");
                continue;
            }

            MapData mapData = new MapData();
            mapData.sceneName = mapPrefab.name;

            // 포탈 정보 수집
            Transform upPortalTransform = mapPrefab.transform.Find("UpPortal");
            mapData.HasUpPortal = upPortalTransform != null;
            mapData.upPortalObject = upPortalTransform?.gameObject;

            Transform downPortalTransform = mapPrefab.transform.Find("DownPortal");
            mapData.HasDownPortal = downPortalTransform != null;
            mapData.downPortalObject = downPortalTransform?.gameObject;

            Transform leftPortalTransform = mapPrefab.transform.Find("LeftPortal");
            mapData.HasLeftPortal = leftPortalTransform != null;
            mapData.leftPortalObject = leftPortalTransform?.gameObject;

            Transform rightPortalTransform = mapPrefab.transform.Find("RightPortal");
            mapData.HasRightPortal = rightPortalTransform != null;
            mapData.rightPortalObject = rightPortalTransform?.gameObject;

            mapData.hasEnemies = false;
            mapScenes[i] = mapData;

        }

        //nextStageMapScenes 처리
        nextStageMapScenes = new MapData[nextStageMapPrefabs.Length];

        for (int i = 0; i < nextStageMapPrefabs.Length; i++)
        {
            GameObject mapPrefab = nextStageMapPrefabs[i];

            if (mapPrefab == null)
            {
                Debug.LogWarning($"MapScenes {i}가 비어있습니다.");
                continue;
            }

            MapData mapData = new MapData();
            mapData.sceneName = mapPrefab.name;

            // 포탈 정보 수집
            Transform upPortalTransform = mapPrefab.transform.Find("UpPortal");
            mapData.HasUpPortal = upPortalTransform != null;
            mapData.upPortalObject = upPortalTransform?.gameObject;

            Transform downPortalTransform = mapPrefab.transform.Find("DownPortal");
            mapData.HasDownPortal = downPortalTransform != null;
            mapData.downPortalObject = downPortalTransform?.gameObject;

            Transform leftPortalTransform = mapPrefab.transform.Find("LeftPortal");
            mapData.HasLeftPortal = leftPortalTransform != null;
            mapData.leftPortalObject = leftPortalTransform?.gameObject;

            Transform rightPortalTransform = mapPrefab.transform.Find("RightPortal");
            mapData.HasRightPortal = rightPortalTransform != null;
            mapData.rightPortalObject = rightPortalTransform?.gameObject;

            nextStageMapScenes[i] = mapData;
        }

    }

    public IEnumerator GenerateWorldMapCoroutine()
    {
        isGeneratingMap = true;
        worldMap.Clear();
        usedMapScenes.Clear();
        hasNextStageMap = false;
        StopCoroutine(ResetGenerateWorldMap());

        currentMapCount = 1;
        Debug.Log("맵생성을 시작");

        //시작맵 데이터 초기화하고 넣기
        worldMap[new Vector2Int(centerX, centerY)] = startMapData;

        Queue<MapData> mapQueue = new Queue<MapData>();
        mapQueue.Enqueue(worldMap[new Vector2Int(centerX, centerY)]);

        usedMapScenes = new HashSet<string>();
        usedMapScenes.Add(worldMap[new Vector2Int(centerX, centerY)].sceneName);

        while (currentMapCount < mapSize)
        {

            int maxGenCount = 1000;
            int genCount = 0;

            while (mapQueue.Count > 0 && currentMapCount < mapSize && genCount < maxGenCount && isGeneratingMap == true)
            {
                MapData currentMap = mapQueue.Dequeue();
                GenerateAdjacentMaps(currentMap, mapQueue);


                if (currentMapCount > mapSize * 0.5f && currentMapCount > 5f)
                {
                    CheckWorldMap();
                    RemoveInvalidMaps();
                    GenerateAdditionalMaps();
                }

                genCount++;

                if (genCount >= maxGenCount)
                {
                    Debug.LogError("맵 생성이 중단되었습니다.");
                    break;
                }
            }

            if (currentMapCount < mapSize)
            {
                if (currentMapCount < mapSize || mapQueue.Count <= 0)
                {
                    //맵 무한 재생성 오류날 대 currentMapCount가 14, mapSize가 14에 QueCount는 계속증가하고있지만 CheckAndActivatePortals를 하고 여기로 돌아옴,currentMapCount는 1이됨.
                    //일단 수정해놨음, 나오면 다시 해결할 것
                    Debug.LogWarning("맵의 개수가 부족하거나 맵큐가 비어있어 모든 맵을 삭제하고 다시 생성합니다.");
                    currentMapCount = 1;
                    genCount = 0;
                    ResetAndRestartMapGeneration();
                }

            }
        }

        // worldMap을 역순으로 순회
        var reversedWorldMap = worldMap.Reverse();
        int mapIndex = 1;

        foreach (var kvp in reversedWorldMap)
        {
            MapData mapData = kvp.Value;

            if (!hasNextStageMap)
            {
                if (HasValidPortalConnections(mapData))
                {
                    // 유효성 검사를 통과한 첫 번째 맵에 NextStageMap 배치
                    Vector2Int position = new Vector2Int(mapData.mapX, mapData.mapY);
                    Debug.Log($"뒤에서 {mapIndex}번째 맵,  {mapData.sceneName}씬에 NextStageMap 할당."); // 맵 인덱스 출력
                    Debug.Log($"NextMap의 좌표:{position}");
                    mapData = SelectRandomMapFromNextStageMapScenes();
                    mapData.mapX = position.x;
                    mapData.mapY = position.y;
                    mapData.isNextStageRoom = true;
                    mapData.hasQuickPortal = true;
                    worldMap[position] = mapData;

                    hasNextStageMap = true;
                    CheckAndActivatePortals();
                }

            }

            //4번째 맵마다 빠른이동용 포탈 활성화
            if (mapIndex % 4 == 0 || mapIndex == 1 || mapIndex == 6)
            {
                mapData.hasQuickPortal = true;
                Debug.Log($"뒤에서 {mapIndex}번째 맵,  {mapData.sceneName}씬에 포탈생성."); // 맵 인덱스 출력
            }

            mapIndex++; // 맵 인덱스 증가



        }

        if (!hasNextStageMap)
        {
            Debug.LogWarning("유효성 검사를 통과하는 맵을 찾지 못했습니다. 맵을 다시 생성합니다.");
            ResetAndRestartMapGeneration();
            yield return null;
        }
        isGeneratingMap = false;

        foreach (var kvp in worldMap)
        {
            MapData mapData = kvp.Value;
            CheckAndDisablePortal(mapData);
        }

        CheckAndActivatePortals();
    }

    //리셋
    private IEnumerator ResetGenerateWorldMap()
    {
        isGeneratingMap = false;
        // 모든 포탈 객체 활성화
        foreach (var kvp in worldMap)
        {
            MapData mapData = kvp.Value;
            ResetPortalObjects(mapData, true);
        }


        // worldMap과 usedMapScenes 초기화
        worldMap.Clear();
        usedMapScenes.Clear();
        currentMapCount = 0;

        startMapData = new MapData
        {
            mapX = centerX,
            mapY = centerY,
            isVisited = true,
            hasQuickPortal = true,
            sceneName = startMap.sceneName,
            HasUpPortal = startMap.HasUpPortal,
            HasDownPortal = startMap.HasDownPortal,
            HasLeftPortal = startMap.HasLeftPortal,
            HasRightPortal = startMap.HasRightPortal,
            upPortalObject = startMap.upPortalObject,
            downPortalObject = startMap.downPortalObject,
            leftPortalObject = startMap.leftPortalObject,
            rightPortalObject = startMap.rightPortalObject
        };

        worldMap[new Vector2Int(centerX, centerY)] = startMapData;
        usedMapScenes.Add(startMapData.sceneName);

        regenerateMapCount++;
        if (regenerateMapCount >= 12)
        {
            Debug.LogError($"맵 리셋횟수가 12회 이상 반복되어 스크립트를 재시작합니다.");
            GeneratorRestart restarter = FindAnyObjectByType<GeneratorRestart>();
            restarter.RestartGen();
            StopAllCoroutines();
            regenerateMapCount = 0;
            yield return null;
        }
        else
        {
            yield return StartCoroutine(GenerateWorldMapCoroutine());
        }
    }
    private void ResetAndRestartMapGeneration()
    {
        StopAllCoroutines();
        StartCoroutine(ResetGenerateWorldMap());
    }


    private void ActivatePortal(MapData mapData, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                mapData.HasUpPortal = true;
                break;
            case Direction.Down:
                mapData.HasDownPortal = true;
                break;
            case Direction.Left:
                mapData.HasLeftPortal = true;
                break;
            case Direction.Right:
                mapData.HasRightPortal = true;
                break;
        }
    }

    private void CheckAndActivatePortals()
    {
        foreach (var kvp in worldMap)
        {
            MapData mapData = kvp.Value;
            if (mapData == null)
            {
                continue;
            }

            Vector2Int currentPosition = new Vector2Int(mapData.mapX, mapData.mapY);

            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                Vector2Int adjacentPosition = GetAdjacentPosition(currentPosition.x, currentPosition.y, dir);
                if (worldMap.ContainsKey(adjacentPosition))
                {
                    MapData adjacentMapData = worldMap[adjacentPosition];
                    if (adjacentMapData != null && IsConnectedToPreviousMap(mapData, adjacentMapData, OppositeDirection(dir)))
                    {
                        ActivatePortal(mapData, OppositeDirection(dir));
                    }
                }
            }
        }
    }






    //맵 생성을 시작하는 함수
    private void GenerateAdjacentMaps(MapData currentMap, Queue<MapData> mapQueue)
    {
        List<Direction> availableDirections = GetAvailableDirections(currentMap);

        foreach (Direction direction in availableDirections)
        {
            Vector2Int newPosition = GetAdjacentPosition(currentMap.mapX, currentMap.mapY, direction);

            if (!worldMap.ContainsKey(newPosition))
            {
                string randomMapScene;
                randomMapScene = SelectRandomMapScene(direction);


                MapData selectedMap = FindMapWithPortal(randomMapScene, direction);

                if (selectedMap != null && IsConnectedToPreviousMap(currentMap, selectedMap, direction))
                {
                    MapData newMap = CreateNewMap(newPosition, selectedMap);
                    if (newMap != null)
                    {
                        worldMap[newPosition] = newMap;
                        currentMapCount++;
                        mapQueue.Enqueue(newMap);


                    }
                }
            }
        }
    }

    private bool CanCreateMapAtPosition(Vector2Int position, Direction direction)
    {
        if (!IsValidPosition(position))
            return false;

        foreach (Direction dir in Enum.GetValues(typeof(Direction)))
        {
            Vector2Int adjacentPosition = GetAdjacentPosition(position.x, position.y, dir);
            if (worldMap.ContainsKey(adjacentPosition))
            {
                MapData adjacentMapData = worldMap[adjacentPosition];
                if (IsValidPortalConnection(adjacentMapData, OppositeDirection(dir)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    //사용 가능한 방향 체크
    private List<Direction> GetAvailableDirections(MapData mapData)
    {

        if (mapData == null)
        {
            return new List<Direction>();
        }

        List<Direction> availableDirections = new List<Direction>();

        if (mapData.HasUpPortal)
            availableDirections.Add(Direction.Up);
        if (mapData.HasDownPortal)
            availableDirections.Add(Direction.Down);
        if (mapData.HasLeftPortal)
            availableDirections.Add(Direction.Left);
        if (mapData.HasRightPortal)
            availableDirections.Add(Direction.Right);

        return availableDirections;
    }



    private bool IsConnectedToPreviousMap(MapData previousMap, MapData currentMap, Direction direction)
    {

        if (previousMap == null || currentMap == null)
            return false;

        return direction switch
        {
            Direction.Up => previousMap.HasUpPortal && currentMap.HasDownPortal,
            Direction.Down => previousMap.HasDownPortal && currentMap.HasUpPortal,
            Direction.Left => previousMap.HasLeftPortal && currentMap.HasRightPortal,
            Direction.Right => previousMap.HasRightPortal && currentMap.HasLeftPortal,
            _ => false,
        };
    }





    //다음스테이지로 가는 씬 불러오는 함수
    private MapData SelectRandomMapFromNextStageMapScenes()
    {
        if (nextStageMapScenes.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, nextStageMapScenes.Length);
            return nextStageMapScenes[randomIndex];
        }

        return null;
    }



    //--------맵 체크함수
    private bool CheckWorldMap()
    {
        foreach (var kvp in worldMap)
        {
            MapData mapData = kvp.Value;
            if (!HasValidPortalConnections(mapData))
            {
                return false;
            }
        }
        return true;
    }

    //모든방향의 포탈연결 확인
    private bool HasValidPortalConnections(MapData mapData)
    {
        if (mapData == null)
        {
            return false;
        }

        //다음맵으로 가는 맵의 포탈은 열어놓기
        if (nextStageMapScenes.Any(m => m.sceneName == mapData.sceneName))
        {
            return true;
        }

        if (mapData.HasUpPortal && !IsValidPortalConnection(mapData, Direction.Up))
            return false;
        if (mapData.HasDownPortal && !IsValidPortalConnection(mapData, Direction.Down))
            return false;
        if (mapData.HasLeftPortal && !IsValidPortalConnection(mapData, Direction.Left))
            return false;
        if (mapData.HasRightPortal && !IsValidPortalConnection(mapData, Direction.Right))
            return false;

        return true;
    }

    //해당포탈이 있으면 연결 체크
    private bool IsValidPortalConnection(MapData mapData, Direction direction)
    {
        if (mapData == null)
        {
            return false;
        }

        Vector2Int adjacentPosition = GetAdjacentPosition(mapData.mapX, mapData.mapY, direction);

        if (!IsValidPosition(adjacentPosition))
            return false;

        if (worldMap.ContainsKey(adjacentPosition))
        {
            MapData adjacentMapData = worldMap[adjacentPosition];

            if (adjacentMapData != null)
            {
                switch (direction)
                {
                    case Direction.Up:
                        return adjacentMapData.HasDownPortal;
                    case Direction.Down:
                        return adjacentMapData.HasUpPortal;
                    case Direction.Left:
                        return adjacentMapData.HasRightPortal;
                    case Direction.Right:
                        return adjacentMapData.HasLeftPortal;
                }
            }
        }

        return false;
    }




    //=========맵체크(삭제)

    private void RemoveInvalidMaps()
    {
        List<Vector2Int> invalidPositions = new List<Vector2Int>();
        List<string> invalidSceneNames = new List<string>();

        foreach (var kvp in worldMap)
        {
            Vector2Int position = kvp.Key;
            MapData mapData = kvp.Value;

            if (mapData != null && !HasValidPortalConnections(mapData))
            {
                bool canRemove = true;
                foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                {
                    Vector2Int adjacentPosition = GetAdjacentPosition(position.x, position.y, dir);
                    if (worldMap.ContainsKey(adjacentPosition))
                    {
                        MapData adjacentMapData = worldMap[adjacentPosition];
                        if (adjacentMapData != null && HasValidPortalConnections(adjacentMapData))
                        {
                            canRemove = false;
                            break;
                        }
                    }
                }

                if (canRemove)
                {
                    invalidPositions.Add(position);
                    invalidSceneNames.Add(mapData.sceneName);

                    // nextStageMapScenes에서 선택된 맵이 지워지면 usedMapScenes에서 nextStageMapScenes의 모든 맵 sceneName 제거
                    if (nextStageMapScenes.Any(m => m.sceneName == mapData.sceneName))
                    {
                        Debug.Log($"다음스테이지로 이동맵이 RemoveInvalidMaps에서 삭제되었습니다.");
                        foreach (var nextStageMap in nextStageMapScenes)
                        {
                            usedMapScenes.Remove(nextStageMap.sceneName);
                        }

                    }
                }
            }
        }

        foreach (Vector2Int position in invalidPositions)
        {
            worldMap.Remove(position);
            currentMapCount--;
        }

        foreach (string sceneName in invalidSceneNames)
        {
            usedMapScenes.Remove(sceneName);
        }
    }


    //-----------맵체크(재생성)
    private void GenerateAdditionalMaps()
    {
        List<Vector2Int> newMapPositions = new List<Vector2Int>();

        foreach (var kvp in worldMap)
        {
            Vector2Int position = kvp.Key;
            MapData mapData = kvp.Value;

            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                Vector2Int newPosition = GetAdjacentPosition(position.x, position.y, dir);

                if (!worldMap.ContainsKey(newPosition) && IsValidPosition(newPosition) && CanCreateMapAtPosition(newPosition, dir))
                {
                    string randomMapScene = SelectRandomMapScene(dir);
                    MapData selectedMap = FindMapWithPortal(randomMapScene, dir);

                    if (selectedMap != null && IsConnectedToPreviousMap(mapData, selectedMap, dir))
                    {
                        MapData newMap = CreateNewMap(newPosition, selectedMap);
                        if (newMap != null)
                        {
                            newMapPositions.Add(newPosition);
                            currentMapCount++;
                        }
                    }
                }
            }
        }

        foreach (Vector2Int position in newMapPositions)
        {
            worldMap[position] = CreateNewMap(position, FindMapWithPortal(SelectRandomMapScene(GetRandomDirection()), GetRandomDirection()));
        }
    }


    private Direction GetRandomDirection()
    {
        Array values = Enum.GetValues(typeof(Direction));
        return (Direction)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }



    //----------포탈 끄기
    //TODO::포탈 생성조건을 살짝 완화할필요가있음
    private void CheckAndDisablePortal(MapData mapData)
    {
        if (mapData == null)
        {
            return;
        }

        // 현재 맵의 포탈 방향에 따라 반대 방향의 포탈 체크
        if (mapData.HasUpPortal)
        {


            Vector2Int oppositePosition = GetAdjacentPosition(mapData.mapX, mapData.mapY, Direction.Up);
            if (IsValidPosition(oppositePosition) && worldMap.ContainsKey(oppositePosition))
            {
                MapData oppositeMapData = worldMap[oppositePosition];
                if (!IsConnectedToPreviousMap(mapData, oppositeMapData, Direction.Up))
                {
                    DisablePortal(mapData, Direction.Up);
                }
            }
            else
            {
                DisablePortal(mapData, Direction.Up);
            }
        }

        if (mapData.HasDownPortal)
        {
            Vector2Int oppositePosition = GetAdjacentPosition(mapData.mapX, mapData.mapY, Direction.Down);
            if (IsValidPosition(oppositePosition) && worldMap.ContainsKey(oppositePosition))
            {
                MapData oppositeMapData = worldMap[oppositePosition];
                if (!IsConnectedToPreviousMap(mapData, oppositeMapData, Direction.Down))
                {
                    DisablePortal(mapData, Direction.Down);
                }
            }
            else
            {
                DisablePortal(mapData, Direction.Down);
            }
        }

        if (mapData.HasLeftPortal)
        {
            Vector2Int oppositePosition = GetAdjacentPosition(mapData.mapX, mapData.mapY, Direction.Left);
            if (IsValidPosition(oppositePosition) && worldMap.ContainsKey(oppositePosition))
            {
                MapData oppositeMapData = worldMap[oppositePosition];
                if (!IsConnectedToPreviousMap(mapData, oppositeMapData, Direction.Left))
                {
                    DisablePortal(mapData, Direction.Left);
                }
            }
            else
            {
                DisablePortal(mapData, Direction.Left);
            }
        }

        if (mapData.HasRightPortal)
        {
            Vector2Int oppositePosition = GetAdjacentPosition(mapData.mapX, mapData.mapY, Direction.Right);
            if (IsValidPosition(oppositePosition) && worldMap.ContainsKey(oppositePosition))
            {
                MapData oppositeMapData = worldMap[oppositePosition];
                if (!IsConnectedToPreviousMap(mapData, oppositeMapData, Direction.Right))
                {
                    DisablePortal(mapData, Direction.Right);
                }
            }
            else
            {
                DisablePortal(mapData, Direction.Right);
            }
        }
    }

    private void DisablePortal(MapData mapData, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                if (mapData.upPortalObject != null)
                {
                    mapData.HasUpPortal = false;
                    mapData.upPortalObject.SetActive(false);
                }
                break;
            case Direction.Down:
                if (mapData.downPortalObject != null)
                {
                    mapData.HasDownPortal = false;
                    mapData.downPortalObject.SetActive(false);
                }
                break;
            case Direction.Left:
                if (mapData.leftPortalObject != null)
                {
                    mapData.HasLeftPortal = false;
                    mapData.leftPortalObject.SetActive(false);
                }
                break;
            case Direction.Right:
                if (mapData.rightPortalObject != null)
                {
                    mapData.HasRightPortal = false;
                    mapData.rightPortalObject.SetActive(false);
                }
                break;
        }
    }



    //-------맵 생성 함수
    private MapData CreateNewMap(Vector2Int position, MapData selectedMap)
    {
        if (this.usedMapScenes.Contains(selectedMap.sceneName))
        {
            return null;
        }

        this.usedMapScenes.Add(selectedMap.sceneName);

        return new MapData
        {
            mapX = position.x,
            mapY = position.y,
            sceneName = selectedMap.sceneName,
            HasUpPortal = selectedMap.HasUpPortal,
            HasDownPortal = selectedMap.HasDownPortal,
            HasLeftPortal = selectedMap.HasLeftPortal,
            HasRightPortal = selectedMap.HasRightPortal,
            upPortalObject = selectedMap.upPortalObject,
            downPortalObject = selectedMap.downPortalObject,
            leftPortalObject = selectedMap.leftPortalObject,
            rightPortalObject = selectedMap.rightPortalObject
        };
    }

    private string SelectRandomMapScene(Direction direction)
    {
        List<MapData> availableMaps = mapScenes.Where(mapData =>
        {
            return direction switch
            {
                Direction.Up => mapData.HasDownPortal,
                Direction.Down => mapData.HasUpPortal,
                Direction.Left => mapData.HasRightPortal,
                Direction.Right => mapData.HasLeftPortal,
                _ => false,
            };
        }).ToList();

        if (availableMaps.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableMaps.Count);
            return availableMaps[randomIndex].sceneName;
        }

        return string.Empty;
    }


    private MapData FindMapWithPortal(string sceneName, Direction direction)
    {
        MapData mapData;
        //TODO:: null 등장
        if (nextStageMapScenes.Any(m => m.sceneName == sceneName))
        {
            mapData = nextStageMapScenes.FirstOrDefault(m => m.sceneName == sceneName);
        }
        else
        {

            mapData = mapScenes.FirstOrDefault(m => m.sceneName == sceneName);
        }

        if (mapData != null)
        {
            return direction switch
            {
                Direction.Up => mapData.HasDownPortal ? mapData : null,
                Direction.Down => mapData.HasUpPortal ? mapData : null,
                Direction.Left => mapData.HasRightPortal ? mapData : null,
                Direction.Right => mapData.HasLeftPortal ? mapData : null,
                _ => null,
            };
        }

        return null;
    }

    public Vector2Int GetAdjacentPosition(int x, int y, Direction direction)
    {
        return direction switch
        {
            Direction.Up => new Vector2Int(x, y + 1),
            Direction.Down => new Vector2Int(x, y - 1),
            Direction.Left => new Vector2Int(x - 1, y),
            Direction.Right => new Vector2Int(x + 1, y),
            _ => new Vector2Int(x, y),
        };
    }



    public bool IsValidPosition(Vector2Int position)
    {
        return position.x >= 0 && position.x < worldMapSize &&
                     position.y >= 0 && position.y < worldMapSize;
    }



    //---------맵 로드 함수
    public void MapCheck(Vector2Int position, Direction direction = Direction.Right)
    {
        if (currentMap != null)
        {
            StartCoroutine(UnloadAndLoadMap(position, direction));
        }
        else if (IsValidPosition(position))
        {
            LoadMap(position, direction);
        }
        else
        {
            Debug.LogError($"유효하지 않은 맵 위치입니다. 위치: ({position.x}, {position.y})");
            // 사용 중인 포탈 비활성화
            GameObject portalObject = GetPortalObject(direction);
            if (portalObject != null)
            {
                portalObject.SetActive(false);
            }

            player.transform.position = new Vector2(-9f, -8f);
            SceneManager.LoadScene("ErrorScene", LoadSceneMode.Additive);

        }
    }

    private IEnumerator UnloadAndLoadMap(Vector2Int position, Direction direction)
    {
        AsyncOperation unloadOperation = null;

        // 기존 맵 언로드
        if (currentMap != null)
        {
            unloadOperation = SceneManager.UnloadSceneAsync(currentMap.sceneName);

            // unloadOperation이 null이 아닌 경우에만 진행
            if (unloadOperation != null)
            {
                while (!unloadOperation.isDone)
                {
                    yield return null;
                }
            }
            else
            {
                Debug.LogError($"씬 언로드에 실패: {currentMap.sceneName}");
            }
        }

        // 새 맵 로드
        LoadMap(position, direction);
    }



    private void LoadMap(Vector2Int position, Direction direction)
    {
        if (worldMap.ContainsKey(position))
        {
            MapData mapToLoad = worldMap[position];

            if (mapToLoad != null)
            {
                mapToLoad.enteredDirection = direction;
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mapToLoad.sceneName, LoadSceneMode.Additive);

                if (asyncLoad == null)
                {
                    Debug.LogError($"불러오는데 실패한 맵: {mapToLoad.sceneName}");

                    player.transform.position = new Vector2(-9f, -8f);
                    SceneManager.LoadScene("ErrorScene", LoadSceneMode.Additive);
                    //TODO:: 해당 맵에 들어오는데 사용한 포탈 제거, 나갈수잇게하기
                }

                asyncLoad.completed += _ =>
                {
                    Scene loadedScene = SceneManager.GetSceneByName(mapToLoad.sceneName);
                    SceneManager.SetActiveScene(loadedScene);
                    currentMap = mapToLoad;
                    FindPortalsAndSpriteRenderers();
                    CheckEnemysInScene(mapToLoad);
                    LoadMapState(mapToLoad);
                    currentMap.isVisited = true;
                    if (mapUI != null)
                    {
                        mapUI.UpdateMapUI();
                    }
                    if (mapToLoad.isTrevel == false)
                    {
                        SetPlayerPosition(OppositeDirection(mapToLoad.enteredDirection));
                    }
                    else
                    {
                        TrevelPlayerPosition(mapToLoad);
                    }
                    mainCamera = GameManager.Instance.MainCamera;
                    mainCamera.cameraReturn = true;
                };
            }
            else
            {
                Debug.LogError($"해당좌표의 맵 데이터를 찾을 수 없습니다.: ({position.x}, {position.y})");
                // 사용 중인 포탈 비활성화
                GameObject portalObject = GetPortalObject(direction);
                if (portalObject != null)
                {
                    portalObject.SetActive(false);
                }

                player.transform.position = new Vector2(-9f, -8f);
                SceneManager.LoadScene("ErrorScene", LoadSceneMode.Additive);
            }
        }

        else
        {
            Debug.LogError($"해당좌표에 맵이 없습니다.: ({position.x}, {position.y})");
            // 사용 중인 포탈 비활성화
            GameObject portalObject = GetPortalObject(direction);
            if (portalObject != null)
            {
                portalObject.SetActive(false);
            }

            player.transform.position = new Vector2(-9f, -8f);
            SceneManager.LoadScene("ErrorScene", LoadSceneMode.Additive);
        }
    }

    private void TrevelPlayerPosition(MapData mapdata)
    {
        Transform quickPortalTransform = quickPortal.transform;
        player.transform.position = new Vector3(quickPortalTransform.position.x, quickPortalTransform.position.y
                                                , player.transform.position.z);
        mapdata.isTrevel = false;
    }

    private void FindPortalsAndSpriteRenderers()
    {
        portals = FindObjectsOfType<Portal>();
        portalSpriteRenderers.Clear();

        foreach (Portal portal in portals)
        {
            SpriteRenderer[] spriteRenderers = portal.GetComponentsInChildren<SpriteRenderer>();
            portalSpriteRenderers[portal] = spriteRenderers;
        }
    }
    //적 체크 함수
    public void CheckEnemysInScene(MapData mapData)
    {
        if (enemyParent == null)
        {
            enemyParent = GameObject.Find("Enemy");
        }
        if (enemyParent != null)
        {
            enemies = enemyParent.GetComponentsInChildren<EnemyBase>();

            if (mapData.isVisited && !currentMap.hasEnemies)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    Destroy(enemies[i].gameObject);
                }
                mapData.hasEnemies = false;
                UpdatePortalState(currentMap.hasEnemies);
            }
            else
            {

                bool hasEnemies = enemies.Length > 0;

                if (currentMap.hasEnemies != hasEnemies)
                {
                    currentMap.hasEnemies = hasEnemies;
                    if (!hasEnemies)
                    {
                        ActivateRandomChest();
                    }
                    UpdatePortalState(currentMap.hasEnemies);
                }

            }
        }
        else
        {
            currentMap.hasEnemies = false;
            UpdatePortalState(false);
        }
    }

    //보상 상자 활성화 함수
    private void ActivateRandomChest()
    {
        GameObject chestsParent = GameObject.Find("Chests");
        if (chestsParent != null)
        {
            GameObject[] chests = new GameObject[chestsParent.transform.childCount];
            for (int i = 0; i < chestsParent.transform.childCount; i++)
            {
                chests[i] = chestsParent.transform.GetChild(i).gameObject;
            }

            if (chests.Length > 0)
            {
                GameObject nearestChest = null;
                float nearestDistance = float.MaxValue;

                foreach (GameObject chest in chests)
                {
                    float distance = Vector2.Distance(player.transform.position, chest.transform.position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearestChest = chest;
                    }
                }

                if (nearestChest != null)
                {
                    selectedChest = nearestChest.GetComponent<Chest>();

                    selectedChest.gameObject.SetActive(true);
                }
            }
        }
    }

    public void QuickTrevel(MapData mapData)
    {

        if (!currentMap.hasEnemies)
        {
            mapData.isTrevel = true;
            currentPosition = new Vector2Int(currentMap.mapX, currentMap.mapY);
            SaveMapState(currentPosition);
            MapCheck(new Vector2Int(mapData.mapX, mapData.mapY));
            mapUI.HideMap();
        }

    }

    //에러맵의 포탈 지우기용 함수
    private GameObject GetPortalObject(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return currentMap.upPortalObject;
            case Direction.Down:
                return currentMap.downPortalObject;
            case Direction.Left:
                return currentMap.leftPortalObject;
            case Direction.Right:
                return currentMap.rightPortalObject;
            default:
                return null;
        }
    }

    //포탈 색 변경 함수
    private void UpdatePortalState(bool hasEnemies)
    {

        foreach (Portal portal in portals)
        {
            if (portalSpriteRenderers.TryGetValue(portal, out SpriteRenderer[] portalSp))
            {
                foreach (SpriteRenderer portalSpriteRenderer in portalSp)
                {
                    portal.PortalColorChange(hasEnemies);

                }
            }
        }
    }

    private Direction OppositeDirection(Direction direction)
    {
        return direction switch
        {
            Direction.Right => Direction.Left,
            Direction.Left => Direction.Right,
            Direction.Up => Direction.Down,
            Direction.Down => Direction.Up,
            _ => direction,
        };
    }


    private void SetPlayerPosition(Direction direction)
    {
        if (player == null)
        {
            return;
        }


        GameObject portalObject = GameObject.Find($"{direction}Portal");
        if (portalObject != null)
        {
            Transform playerSpawnPoint = portalObject.transform.GetChild(0);
            if (playerSpawnPoint != null)
            {
                player.transform.position = playerSpawnPoint.position;
            }
            else
            {
                Debug.LogWarning($"{direction}방향의 포탈에서 플레이어가 스폰될 지점을 찾지 못했습니다.");
            }
        }
        else
        {

            GameObject.FindAnyObjectByType<ReturnZone>().transform.parent.Find($"{direction}Portal").gameObject.SetActive(true);
            portalObject = GameObject.FindAnyObjectByType<ReturnZone>().transform.parent.Find($"{direction}Portal").gameObject;
            if (portalObject != null)
            {
                Debug.LogWarning($"현재맵에 {direction}방향의 포탈이 있지만 꺼져있었습니다.");
                UpdatePortalState(currentMap.hasEnemies);
                Transform playerSpawnPoint = portalObject.transform.GetChild(0);
                if (playerSpawnPoint != null)
                {
                    player.transform.position = playerSpawnPoint.position;
                }
            }
            else
            {
                Debug.LogWarning($"현재맵에 {direction}방향의 포탈이 없습니다.");
                player.transform.position = new Vector3(0, 0, player.transform.position.z);
            }
        }
    }

    //포탈 들어갔을 때
    public void EnterPortal(Direction direction)
    {
        currentPosition = new Vector2Int(currentMap.mapX, currentMap.mapY);
        Vector2Int MapPosition = GetAdjacentPosition(currentMap.mapX, currentMap.mapY, direction);
        if (IsValidPosition(MapPosition))
        {
            SaveMapState(currentPosition); // 현재 맵의 상태를 저장
            MapCheck(MapPosition, direction);


        }


    }
    public MapData GetMapData(int x, int y)
    {
        Vector2Int position = new Vector2Int(x, y);
        if (worldMap.ContainsKey(position))
        {
            return worldMap[position];
        }
        return null;
    }

    //맵 저장,로드==============================================



    /// <summary>
    /// 맵을 저장하기 위해 사용하는 함수
    /// </summary>
    /// <param name="position"></param>
    private void SaveMapState(Vector2Int position)
    {



        // 아이템이 저장되는 맵
        MapData map = GetMapData(position.x, position.y);

        List<SaveItemData> saveItemDatas = new List<SaveItemData>();

        // 맵에서 아이템 가져오기
        ItemObject[] itemObjects = FindObjectsOfType<ItemObject>();

        // 아이템으로 리스트 작성
        for (int i = 0; i < itemObjects.Length; i++)
        {
            saveItemDatas.Add(new SaveItemData
            {
                itemCode = itemObjects[i].ItemData.code,
                itemPositions = (Vector2)itemObjects[i].transform.position
            });

            // 그 후 비활성화
            GameObject obj = itemObjects[i].gameObject;
            obj.SetActive(false);
        }

        // 맵에 작성한 리스트 넣기
        if (map != null)
        {
            map.mapItemDatas = saveItemDatas;
        }


        BulletObject[] bulletObjects = FindObjectsOfType<BulletObject>();

        if (bulletObjects != null && bulletObjects.Length > 0)
        {
            // 모든 총알 비활성화
            foreach (BulletObject bulletObject in bulletObjects)
            {
                bulletObject.gameObject.SetActive(false);
            }
        }

        SceneChanger sceneChanger = FindAnyObjectByType<SceneChanger>();
        sceneChanger.SceneChanged();
    }



    /// <summary>
    /// 맵을 불러오기 위해 사용하는 함수
    /// </summary>
    /// <param name="mapData"></param>
    private void LoadMapState(MapData mapData)
    {
        // 아이템 불러오기
        if (mapData.mapItemDatas != null)
        {
            foreach (SaveItemData itemData in mapData.mapItemDatas)
            {
                Factory.Instance.MakeItems(itemData.itemCode, 1, itemData.itemPositions);
            }
        }

        if (mapData.hasQuickPortal)
        {
            quickPortal = GameObject.FindGameObjectWithTag("QuickPortal");
            if (quickPortal != null)
            {
                quickPortal.transform.GetChild(0).gameObject.SetActive(true);
            }
        }


    }




    private void OnEnable()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {

        // 1. 맵 데이터 정리
        foreach (var kvp in worldMap)
        {
            MapData mapData = kvp.Value;
            ResetPortalObjects(mapData, true);
            if (mapData != null)
            {
                // 맵 데이터 내의 게임 오브젝트 및 컴포넌트 정리
                mapData.upPortalObject = null;
                mapData.downPortalObject = null;
                mapData.leftPortalObject = null;
                mapData.rightPortalObject = null;
            }
        }


        worldMap.Clear();
        StopAllCoroutines();
        currentMap = null;
        player = null;
        mapUI = null;
        usedMapScenes.Clear();

    }

    private void ResetPortalObjects(MapData mapData, bool activateMode)
    {

        if (mapData == null)
        {
            return;
        }

        if (mapData.upPortalObject != null)
        {
            mapData.upPortalObject.SetActive(activateMode);
            mapData.HasUpPortal = activateMode;
        }

        if (mapData.downPortalObject != null)
        {
            mapData.downPortalObject.SetActive(activateMode);
            mapData.HasDownPortal = activateMode;
        }

        if (mapData.leftPortalObject != null)
        {
            mapData.leftPortalObject.SetActive(activateMode);
            mapData.HasLeftPortal = activateMode;
        }

        if (mapData.rightPortalObject != null)
        {
            mapData.rightPortalObject.SetActive(activateMode);
            mapData.HasRightPortal = activateMode;
        }
    }







}














