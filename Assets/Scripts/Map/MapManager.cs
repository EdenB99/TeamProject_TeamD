using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;



public class MapManager : MonoBehaviour
{
    //TODO L:: 맵 로딩만들기 
    //TODO:: 오류방 만들기
    //TODO:: 빠른이동구현
    //TODO:: 함정 제작해야함.

    //찾은 오류 :: 집컴퓨터에서 보스가 빠르게 움직이는 경우가있음
    //플레이어가 대시가 끝나 레이어가 다시 돌아왔는데 대쉬가 끝나지않은 경우 적과 부딫혀서 적을 멀리 밀 수 있음.
    //적을 빠르게 공격하면 무기 이펙트에서 null 발생하는 경우가 존재(주로 스켈레톤을 공격 시 발생)
    //적이 플레이어밑에있거나 파란해골이 플레이어에게 붙을 경우 무한 도리도리
    //아이템이 맵을 나갔다 돌아올때마다 다른아이템이 되어있음(먹었을 땐 같은아이템이였음.)
    
    //계속 나갔다 들어오면 이상한곳으로 이동되며 아이템이 몇개 먹어지며 
    //indexOutOfRangeException: Index was outside the bounds of the array., Assets/Scripts/Core/Factory.cs:76 
    //Assets/Scripts/Map/MapManager.cs:1251)  (at Assets/Scripts/Map/MapManager.cs:1026)
    //오류가 생김



    [Header("변수")]

    private Player player;
    [Header("전체 맵 개수(+-1~3)")]
    [SerializeField] private int mapSize = 20;
    [Header("월드맵크기(MapSize*MapSize)")]
    [SerializeField] private int worldMapSize = 7;
    public int WorldMapSize => worldMapSize;
    [SerializeField] private int currentMapCount = 0;
    [Header("스테이지맵 프리팹")]
    [SerializeField] private GameObject[] mapPrefabs;
    private MapData[] mapScenes;
    private Dictionary<Vector2Int, MapData> worldMap = new Dictionary<Vector2Int, MapData>();
    private HashSet<string> usedMapScenes = new HashSet<string>();
    private int centerX;
    private int centerY;
    Vector2Int currentPosition;


    //특수 맵 관련
    private bool hasNextStageMap = false;
    MapData startMapData;


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
    EnemyBase_[] enemies;
    protected bool isEnemyDead = false;

    //포탈 관련
    private Portal[] portals;
    private Dictionary<Portal, SpriteRenderer[]> portalSpriteRenderers = new Dictionary<Portal, SpriteRenderer[]>();

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

        MapData startMap = new MapData
        {
            mapX = centerX,
            mapY = centerY,
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

        worldMap[new Vector2Int(centerX, centerY)] = startMap;
        startMapData = startMap;
        MapCheck(new Vector2Int(centerX, centerY));

    }





    private void Start()
    {
        player = GameObject.FindAnyObjectByType<Player>();
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

    private IEnumerator GenerateWorldMapCoroutine()
    {
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

            while (mapQueue.Count > 0 && currentMapCount < mapSize && genCount < maxGenCount)
            {
                MapData currentMap = mapQueue.Dequeue();
                GenerateAdjacentMaps(currentMap, mapQueue);


                if (currentMapCount > mapSize * 0.5f && currentMapCount > 5f || mapQueue.Count <= 0)
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
                    yield return ResetGenerateWorldMap();
                    currentMapCount = 1;
                    genCount = 0;
                }

            }
        }
        if (!hasNextStageMap)
        {
            // worldMap을 역순으로 순회
            var reversedWorldMap = worldMap.Reverse();
            int mapIndex = 1;

            foreach (var kvp in reversedWorldMap)
            {
                MapData mapData = kvp.Value;

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
                    worldMap[position] = mapData;

                    hasNextStageMap = true;
                    CheckAndActivatePortals(); 
                    break;
                }

                mapIndex++; // 맵 인덱스 증가
            }

            // 유효성 검사를 통과하는 맵을 찾지 못한 경우
            if (!hasNextStageMap)
            {
                Debug.LogWarning("유효성 검사를 통과하는 맵을 찾지 못했습니다. 맵을 다시 생성합니다.");
                yield return ResetGenerateWorldMap();
            }
        }

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


        worldMap[new Vector2Int(centerX, centerY)] = startMapData;
        usedMapScenes.Add(startMapData.sceneName);

        yield return StartCoroutine(GenerateWorldMapCoroutine());
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

        switch (direction)
        {
            case Direction.Up:
                return previousMap.HasUpPortal && currentMap.HasDownPortal;
            case Direction.Down:
                return previousMap.HasDownPortal && currentMap.HasUpPortal;
            case Direction.Left:
                return previousMap.HasLeftPortal && currentMap.HasRightPortal;
            case Direction.Right:
                return previousMap.HasRightPortal && currentMap.HasLeftPortal;
            default:
                return false;
        }
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
        //TODO:: 작동안함
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
            switch (direction)
            {
                case Direction.Up:
                    return mapData.HasDownPortal;
                case Direction.Down:
                    return mapData.HasUpPortal;
                case Direction.Left:
                    return mapData.HasRightPortal;
                case Direction.Right:
                    return mapData.HasLeftPortal;
                default:
                    return false;
            }
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
            switch (direction)
            {
                case Direction.Up:
                    return mapData.HasDownPortal ? mapData : null;
                case Direction.Down:
                    return mapData.HasUpPortal ? mapData : null;
                case Direction.Left:
                    return mapData.HasRightPortal ? mapData : null;
                case Direction.Right:
                    return mapData.HasLeftPortal ? mapData : null;
                default:
                    return null;
            }
        }

        return null;
    }

    public Vector2Int GetAdjacentPosition(int x, int y, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return new Vector2Int(x, y + 1);
            case Direction.Down:
                return new Vector2Int(x, y - 1);
            case Direction.Left:
                return new Vector2Int(x - 1, y);
            case Direction.Right:
                return new Vector2Int(x + 1, y);
            default:
                return new Vector2Int(x, y);
        }
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
            SceneManager.UnloadSceneAsync(currentMap.sceneName);
            SceneManager.LoadScene("ErrorMap", LoadSceneMode.Additive);
            //TODO:: 해당 맵에 들어오는데 사용한 포탈 제거, 나갈수잇게하기
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
                    SceneManager.LoadScene("ErrorMap",LoadSceneMode.Additive);
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
                    SetPlayerPosition(OppositeDirection(mapToLoad.enteredDirection));
                };
            }
            else
            {
                Debug.LogError($"해당좌표의 맵 데이터를 찾을 수 없습니다.: ({position.x}, {position.y})");
                SceneManager.LoadScene("ErrorMap", LoadSceneMode.Additive);
                //TODO:: 해당 맵에 들어오는데 사용한 포탈 제거, 나갈수잇게하기
            }
        }
        else
        {
            Debug.LogError($"해당좌표에 맵이 없습니다.: ({position.x}, {position.y})");
            SceneManager.UnloadSceneAsync(currentMap.sceneName);
            SceneManager.LoadScene("ErrorMap", LoadSceneMode.Additive);
            //TODO:: 해당 맵에 들어오는데 사용한 포탈 제거, 나갈수잇게하기
        }
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
            enemies = enemyParent.GetComponentsInChildren<EnemyBase_>();
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
            Debug.Log("플레이어 객체가 할당되지 않았습니다.");
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



    }


    /// <summary>
    /// 맵을 불러오기 위해 사용하는 함수
    /// </summary>
    /// <param name="mapData"></param>
    private void LoadMapState(MapData mapData)
    {

        if (mapData.mapItemDatas != null)
        {
            foreach (SaveItemData itemData in mapData.mapItemDatas)
            {
                Factory.Instance.MakeItems(itemData.itemCode, 1, itemData.itemPositions);
            }
        }


    }



    //플레이어 사망시 맵초기화
    private void ClearMapStates()
    {
        foreach (var mapData in worldMap.Values)
        {

            mapData.itemPositions.Clear();
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














