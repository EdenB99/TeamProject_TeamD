using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;



public class MapManager : MonoBehaviour
{
    //TODO::맵개수가 1~3개정도 먹히는중, 끝쪽으로 갈 수록 포탈 연결이 빈약해지는중,SetActive관련한거 손봐야할듯
    //TODO Low:: 에디터로 한글화 추가하기
    //TODO:: 다음스테이지맵 구현: 맵생성사이에 하나만 끼워넣기
    [Header("변수")]

    private Player player;
    [SerializeField] private int mapSize = 20;
    [Header("월드맵크기(MapSize*MapSize)")]
    [SerializeField] private int worldMapSize = 7;
    public int WorldMapSize => worldMapSize;
    [SerializeField] private int currentMapCount = 0;
    [SerializeField] private GameObject[] mapPrefabs;
    [SerializeField] private MapData[] mapScenes;
    private Dictionary<Vector2Int, MapData> worldMap = new Dictionary<Vector2Int, MapData>();
    private HashSet<string> usedMapScenes = new HashSet<string>();
    private int centerX;
    private int centerY;
    //TODO:: 쓸일있으면 쓰기 MapUIManager mapUIManager;


    [Header("시작맵 리스트풀")]
    [SerializeField] private GameObject[] startMapPrefabs;
    [SerializeField] private MapData[] startMapScenes;

    private MapData currentMap;
    public MapData CurrentMap
    {
        get { return currentMap; }
        private set { currentMap = value; }
    }
    MapUI mapUI;

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
        MapCheck(new Vector2Int(centerX, centerY));
    }





    private void Start()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        mapUI = GameObject.FindAnyObjectByType<MapUI>();
        StartCoroutine(GenerateWorldMapCoroutine());
    }


    private void AutoFillMapData()
    {
        // StartMapScenes 처리
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

            mapScenes[i] = mapData;
        }
    }

    private IEnumerator GenerateWorldMapCoroutine()
    {
        while (currentMapCount < mapSize)
        {
            currentMapCount = 1;
            Queue<MapData> mapQueue = new Queue<MapData>();
            mapQueue.Enqueue(worldMap[new Vector2Int(centerX, centerY)]);

            usedMapScenes = new HashSet<string>();
            usedMapScenes.Add(worldMap[new Vector2Int(centerX, centerY)].sceneName);

            int maxGenCount = 1000;
            int genCount = 0;

            while (mapQueue.Count > 0 && currentMapCount < mapSize && genCount < maxGenCount)
            {
                MapData currentMap = mapQueue.Dequeue();
                GenerateAdjacentMaps(currentMap, mapQueue);

                CheckWorldMap();
                RemoveInvalidMaps();
                GenerateAdditionalMaps();

                genCount++;

                if (genCount >= maxGenCount)
                {
                    Debug.LogError("맵 생성이 중단되었습니다.");
                    break;
                }
            }

            if (currentMapCount < mapSize)
            {
                Debug.LogWarning($"맵 생성에 실패했습니다. 현재 맵 개수: {currentMapCount}. 1초 후 다시 시도합니다.");
                yield return new WaitForSeconds(1f);
            }
        }

        foreach (var kvp in worldMap)
        {
            MapData mapData = kvp.Value;
            CheckAndDisablePortal(mapData);
        }

        CheckAndActivatePortals();
    }




    //실험중----------------------------------------------------------------

    //TODO::맵끝쪽에서 작동안함
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







    //실험중---------------------------------------------------------------

    //맵 생성을 시작하는 함수
private void GenerateAdjacentMaps(MapData currentMap, Queue<MapData> mapQueue)
{
    List<Direction> availableDirections = GetAvailableDirections(currentMap);

    foreach (Direction direction in availableDirections)
    {
        Vector2Int newPosition = GetAdjacentPosition(currentMap.mapX, currentMap.mapY, direction);

        if (!worldMap.ContainsKey(newPosition))
        {
            string randomMapScene = SelectRandomMapScene(direction);
            MapData selectedMap = FindMapWithPortal(randomMapScene, direction);

            if (selectedMap != null && IsConnectedToPreviousMap(currentMap, selectedMap, direction))
            {
                MapData newMap = CreateNewMap(newPosition, selectedMap);
                if (newMap != null)
                {
                    worldMap[newPosition] = newMap;
                    currentMapCount++;
                    Debug.Log($"GenerateAdjacentMaps에서 맵을 생성: {currentMapCount}");
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
                }
            }
        }

        foreach (Vector2Int position in invalidPositions)
        {
            worldMap.Remove(position);
            currentMapCount--;
            Debug.Log($"RemoveInvalidMaps 에서 맵을 삭제: {currentMapCount}");
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
                            Debug.Log($"GenerateAdditionalMaps에서 맵을 재생성: {currentMapCount}");
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
        MapData mapData = mapScenes.FirstOrDefault(m => m.sceneName == sceneName);

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
    //TODO:: 이쪽에서 맵 생성시 일반맵,보스맵(1개만),이벤트맵(2개만)나오게 하되
    //보스맵과 이벤트맵의 개수는 무조건 채워져야함.
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
        }
    }

    private IEnumerator UnloadAndLoadMap(Vector2Int position, Direction direction)
    {
        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentMap.sceneName);
        while (!unloadOperation.isDone)
        {
            yield return null;
        }

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
                    return;
                }

                asyncLoad.completed += _ =>
                {
                    Scene loadedScene = SceneManager.GetSceneByName(mapToLoad.sceneName);
                    SceneManager.SetActiveScene(loadedScene);
                    currentMap = mapToLoad;
                    currentMap.isVisited = true;
                    if(mapUI != null)
                    {
                    mapUI.UpdateMapUI();
                    }
                    SetPlayerPosition(OppositeDirection(mapToLoad.enteredDirection));
                };
            }
            else
            {
                Debug.LogError($"해당좌표의 맵 데이터를 찾을 수 없습니다.: ({position.x}, {position.y})");
            }
        }
        else
        {
            Debug.LogError($"해당좌표에 맵이 없습니다.: ({position.x}, {position.y})");
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
            Debug.LogError("플레이어 객체가 할당되지 않았습니다.");
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

            Debug.LogWarning($"현재맵에 {direction}방향의 포탈을 찾지 못했습니다.");
            player.transform.position = new Vector3(0, 0, player.transform.position.z);
        }
    }

    public void EnterPortal(Direction direction)
    {
        Vector2Int MapPosition = GetAdjacentPosition(currentMap.mapX, currentMap.mapY, direction);
        if (IsValidPosition(MapPosition))
        {
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

    //메모리 누수 발생으로 인한 오류 해결못해서 Claude에게 질문했음, 종료시 모든 객체 파괴 및 정리
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

                // 2. 사용한 에셋 및 리소스 언로드
                //Resources.UnloadUnusedAssets();

                // 3. 코루틴 정리
                StopAllCoroutines();

                // 4. 이벤트 리스너 및 콜백 제거
                // 필요한 경우 이벤트 리스너나 콜백 함수를 제거합니다.

                // 5. 정적 변수 및 싱글톤 객체 정리
                currentMap = null;
                player = null;
                mapUI = null;
                // 필요한 경우 다른 정적 변수나 싱글톤 객체도 정리합니다.

                // 6. 캐시 데이터 정리
                usedMapScenes.Clear();
                // 필요한 경우 다른 캐시 데이터나 임시 데이터도 정리합니다.


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














