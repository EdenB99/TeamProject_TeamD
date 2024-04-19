using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


//TODO:: 병합 다음맵넘어가는 포탈 제작, 다음맵에 상점 -> 보스포탈 -> 보스방
public class MapManager : MonoBehaviour
{
    //TODO :: 집가서 플랫폼 만들기
    //TODO Low:: 에디터로 한글화 추가하기
    [Header("변수")]

    Player player;
    [SerializeField] private int mapSize = 20;
    [Header("월드맵크기(MapSize*MapSize")]
    [SerializeField] private int worldMapSize = 7;
    public int WorldMapSize
    {
        get { return worldMapSize; }
    }

    //TODO:: 나중에 현재맵 serializefield지우기
    [SerializeField] private int currentMapCount = 0;
    [SerializeField] private GameObject[] mapPrefabs;
    [SerializeField] private MapData[] mapScenes;
    //오류:: mapScenesList를 아예 mapScenes와 교체해보려했지만 오류가 발생함.
    private List<MapData> mapScenesList = new List<MapData>();
    int centerX;
    int centerY;

    //TODO:: 쓸일있으면 쓰기 MapUIManager mapUIManager;


    [Header("시작맵 리스트풀")]
    [SerializeField] private GameObject[] startMapPrefabs;
    [SerializeField] private MapData[] startMapScenes;


    private MapData[,] worldMap;
    private MapData currentMap;

    private void Awake()
    {
        centerX = worldMapSize / 2;
        centerY = worldMapSize / 2;
        worldMap = new MapData[worldMapSize, worldMapSize];
        AutoFillMapData();
        LoadStartMap();
    }
    private void LoadStartMap()
    {


        // startMapScenes에서 랜덤으로 하나의 시작 맵 MapData를 선택
        MapData randomStartMap = startMapScenes[Random.Range(0, startMapScenes.Length)];

        MapData startMap = new MapData
        {
            mapX = centerX,
            mapY = centerY,
            sceneName = randomStartMap.sceneName,
            hasUpPortal = randomStartMap.hasUpPortal,
            hasDownPortal = randomStartMap.hasDownPortal,
            hasLeftPortal = randomStartMap.hasLeftPortal,
            hasRightPortal = randomStartMap.hasRightPortal,
            upPortalObject = randomStartMap.upPortalObject,
            downPortalObject = randomStartMap.downPortalObject,
            leftPortalObject = randomStartMap.leftPortalObject,
            rightPortalObject = randomStartMap.rightPortalObject
        };

        worldMap[centerX, centerY] = startMap;
        MapCheck(new Vector2Int(centerX, centerY));
    }




    private void Start()
    {
        mapScenesList = new List<MapData>(mapScenes);
        player = GameObject.FindAnyObjectByType<Player>();
        //mapUIManager = GameObject.FindAnyObjectByType<MapUIManager>();


        GenerateWorldMap();

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
            mapData.hasUpPortal = upPortalTransform != null;
            mapData.upPortalObject = upPortalTransform?.gameObject;

            Transform downPortalTransform = mapPrefab.transform.Find("DownPortal");
            mapData.hasDownPortal = downPortalTransform != null;
            mapData.downPortalObject = downPortalTransform?.gameObject;

            Transform leftPortalTransform = mapPrefab.transform.Find("LeftPortal");
            mapData.hasLeftPortal = leftPortalTransform != null;
            mapData.leftPortalObject = leftPortalTransform?.gameObject;

            Transform rightPortalTransform = mapPrefab.transform.Find("RightPortal");
            mapData.hasRightPortal = rightPortalTransform != null;
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
            mapData.hasUpPortal = upPortalTransform != null;
            mapData.upPortalObject = upPortalTransform?.gameObject;

            Transform downPortalTransform = mapPrefab.transform.Find("DownPortal");
            mapData.hasDownPortal = downPortalTransform != null;
            mapData.downPortalObject = downPortalTransform?.gameObject;

            Transform leftPortalTransform = mapPrefab.transform.Find("LeftPortal");
            mapData.hasLeftPortal = leftPortalTransform != null;
            mapData.leftPortalObject = leftPortalTransform?.gameObject;

            Transform rightPortalTransform = mapPrefab.transform.Find("RightPortal");
            mapData.hasRightPortal = rightPortalTransform != null;
            mapData.rightPortalObject = rightPortalTransform?.gameObject;

            mapScenes[i] = mapData;
        }
    }

    //TODO::
    //1.시작맵과 연결된 포탈은 여전히 이상함(포탈이없는오류,없는맵이동오류)
    //2.월드맵 끝의 포탈은 여전히 사라지지않음

    private void GenerateWorldMap()
    {
        currentMapCount = 1;
        Queue<MapData> mapQueue = new Queue<MapData>();
        mapQueue.Enqueue(worldMap[centerX, centerY]);

        while (mapQueue.Count > 0 && currentMapCount < mapSize)
        {
            MapData currentMap = mapQueue.Dequeue();
            List<Direction> availableDirections = new List<Direction>(System.Enum.GetValues(typeof(Direction)).Cast<Direction>());

            while (availableDirections.Count > 0)
            {
                int randomIndex = Random.Range(0, availableDirections.Count);
                Direction direction = availableDirections[randomIndex];
                availableDirections.RemoveAt(randomIndex);

                Vector2Int newPosition = GetAdjacentPosition(currentMap.mapX, currentMap.mapY, direction);

                if (!IsValidPosition(newPosition) || worldMap[newPosition.x, newPosition.y] != null)
                    continue;

                string randomMapScene = SelectRandomMapScene(direction);
                MapData selectedMap = FindMapWithPortal(randomMapScene, direction);

                if (selectedMap == null || !IsConnectedToPreviousMap(currentMap, selectedMap, direction))
                    continue;

                MapData newMap = CreateNewMap(newPosition, selectedMap);
                worldMap[newPosition.x, newPosition.y] = newMap;
                currentMapCount++;
                mapQueue.Enqueue(newMap);
                mapScenesList.Remove(selectedMap);
            }
        }
    
    // 맵 생성 후 맵 체크 및 수정
    int checkCount = 0;
        int maxCheckCount = 100;

        while (!CheckWorldMap() && checkCount < maxCheckCount)
        {
            CheckWorldMap();
            RemoveInvalidMaps();
            GenerateAdditionalMaps();
            checkCount++;
        }
        if (!CheckWorldMap()) Debug.LogError("맵이 제대로 만들어지지않았습니다.");

        // 포탈  비활성화
        for (int x = 0; x < worldMapSize; x++)
        {
            for (int y = 0; y < worldMapSize; y++)
            {
                MapData mapData = worldMap[x, y];
                if (mapData != null)
                {
                    CheckAndDisablePortal(mapData);

                }
            }
        }
    }


    private bool IsConnectedToPreviousMap(MapData previousMap, MapData currentMap, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                return previousMap.hasUpPortal && currentMap.hasDownPortal;
            case Direction.Down:
                return previousMap.hasDownPortal && currentMap.hasUpPortal;
            case Direction.Left:
                return previousMap.hasLeftPortal && currentMap.hasRightPortal;
            case Direction.Right:
                return previousMap.hasRightPortal && currentMap.hasLeftPortal;
            default:
                return false;
        }
    }



    //-----맵 체크함수
    private bool CheckWorldMap()
    {
        for (int x = 0; x < worldMapSize; x++)
        {
            for (int y = 0; y < worldMapSize; y++)
            {
                MapData mapData = worldMap[x, y];
                if (mapData != null)
                {
                    if (!HasAdjacentPortal(mapData))
                    {
                        return false;
                    }

                    if (!IsPortalsConnected(mapData))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    private bool HasAdjacentPortal(MapData mapData)
    {
        Vector2Int[] adjacentPositions = new Vector2Int[]
        {
        GetAdjacentPosition(mapData.mapX, mapData.mapY, Direction.Up),
        GetAdjacentPosition(mapData.mapX, mapData.mapY, Direction.Down),
        GetAdjacentPosition(mapData.mapX, mapData.mapY, Direction.Left),
        GetAdjacentPosition(mapData.mapX, mapData.mapY, Direction.Right)
        };

        foreach (Vector2Int position in adjacentPositions)
        {
            if (IsValidPosition(position))
            {
                MapData adjacentMapData = worldMap[position.x, position.y];
                if (adjacentMapData != null)
                {
                    if (IsPortalConnected(mapData, adjacentMapData))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private bool IsPortalsConnected(MapData mapData)
    {
        if (mapData.hasUpPortal && !IsPortalConnected(mapData, Direction.Up))
            return false;
        if (mapData.hasDownPortal && !IsPortalConnected(mapData, Direction.Down))
            return false;
        if (mapData.hasLeftPortal && !IsPortalConnected(mapData, Direction.Left))
            return false;
        if (mapData.hasRightPortal && !IsPortalConnected(mapData, Direction.Right))
            return false;
        return true;
    }

    private bool IsPortalConnected(MapData mapData, Direction direction)
    {
        HashSet<Vector2Int> visitedPositions = new HashSet<Vector2Int>();
        Vector2Int currentPosition = new Vector2Int(mapData.mapX, mapData.mapY);
        Vector2Int adjacentPosition = GetAdjacentPosition(currentPosition.x, currentPosition.y, direction);

        while (IsValidPosition(adjacentPosition))
        {
            if (visitedPositions.Contains(adjacentPosition))
            {
                break;
            }

            visitedPositions.Add(adjacentPosition);

            MapData adjacentMapData = worldMap[adjacentPosition.x, adjacentPosition.y];
            if (adjacentMapData != null)
            {
                if (IsPortalConnected(mapData, adjacentMapData))
                {
                    return true;
                }
            }

            currentPosition = adjacentPosition;
            adjacentPosition = GetAdjacentPosition(currentPosition.x, currentPosition.y, direction);
        }

        return false;
    }

    private bool IsPortalConnected(MapData mapData1, MapData mapData2)
    {
        if (mapData1.hasUpPortal && mapData2.hasDownPortal)
            return true;
        if (mapData1.hasDownPortal && mapData2.hasUpPortal)
            return true;
        if (mapData1.hasLeftPortal && mapData2.hasRightPortal)
            return true;
        if (mapData1.hasRightPortal && mapData2.hasLeftPortal)
            return true;
        return false;
    }

    private List<Vector2Int> removedMapPositions = new List<Vector2Int>();

   //=========맵체크(삭제)
private void RemoveInvalidMaps()
{
    removedMapPositions.Clear();

    for (int x = 0; x < worldMapSize; x++)
    {
        for (int y = 0; y < worldMapSize; y++)
        {
            MapData mapData = worldMap[x, y];
            if (mapData != null)
            {
                if (!HasValidPortalConnection(mapData))
                {
                    worldMap[x, y] = null;
                    currentMapCount--;
                    removedMapPositions.Add(new Vector2Int(x, y));
                }
            }
        }
    }
}

    private bool HasValidPortalConnection(MapData mapData)
    {
        if (mapData.hasUpPortal && HasOppositePortal(mapData, Direction.Up))
            return true;

        if (mapData.hasDownPortal && HasOppositePortal(mapData, Direction.Down))
            return true;

        if (mapData.hasLeftPortal && HasOppositePortal(mapData, Direction.Left))
            return true;

        if (mapData.hasRightPortal && HasOppositePortal(mapData, Direction.Right))
            return true;

        return false;
    }

    //-----------맵체크(재생성)
    private void GenerateAdditionalMaps()
    {
        int addedMapCount = 0;

        foreach (Vector2Int position in removedMapPositions)
        {
            MapData currentMap = GetAdjacentPosition(position);
            if (currentMap != null)
            {
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    Vector2Int newPosition = GetAdjacentPosition(position.x,position.y, direction);

                    if (IsValidPosition(newPosition) && worldMap[newPosition.x, newPosition.y] == null)
                    {
                        string randomMapScene = SelectRandomMapScene(direction);
                        MapData selectedMap = FindMapWithPortal(randomMapScene, direction);

                        if (selectedMap != null && IsConnectedToPreviousMap(currentMap, selectedMap, direction))
                        {
                            MapData newMap = CreateNewMap(newPosition, selectedMap);
                            worldMap[newPosition.x, newPosition.y] = newMap;
                            currentMapCount++;
                            addedMapCount++;

                            if (addedMapCount >= removedMapPositions.Count)
                                return;
                        }
                    }
                }
            }
        }
    }

    //----------포탈 끄기
    //TODO::현재 맵이 제대로 안꺼짐(월드맵의 밖과 이어진 포탈이 안꺼지는오류,맵이없는곳과 포탈이 이어져있는 버그
    private void CheckAndDisablePortal(MapData mapData)
    {
        // 현재 맵의 포탈 방향에 따라 반대 방향의 포탈 체크
        if (mapData.hasUpPortal && !HasOppositePortal(mapData, Direction.Up))
        {
            DisablePortal(mapData, Direction.Up);
        }

        if (mapData.hasDownPortal && !HasOppositePortal(mapData, Direction.Down))
        {
            DisablePortal(mapData, Direction.Down);
        }

        if (mapData.hasLeftPortal && !HasOppositePortal(mapData, Direction.Left))
        {
            DisablePortal(mapData, Direction.Left);
        }

        if (mapData.hasRightPortal && !HasOppositePortal(mapData, Direction.Right))
        {
            DisablePortal(mapData, Direction.Right);
        }
    }

    private bool HasOppositePortal(MapData mapData, Direction direction)
    {
        Vector2Int oppositePosition = GetAdjacentPosition(mapData.mapX, mapData.mapY, direction);

        if (IsValidPosition(oppositePosition))
        {
            MapData oppositeMapData = worldMap[oppositePosition.x, oppositePosition.y];
            if (oppositeMapData != null)
            {
                switch (direction)
                {
                    case Direction.Up:
                        return oppositeMapData.hasDownPortal;
                    case Direction.Down:
                        return oppositeMapData.hasUpPortal;
                    case Direction.Left:
                        return oppositeMapData.hasRightPortal;
                    case Direction.Right:
                        return oppositeMapData.hasLeftPortal;
                }
            }
        }

        return false;
    }

    private void DisablePortal(MapData mapData, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                if (mapData.upPortalObject != null)
                {
                    mapData.upPortalObject.SetActive(false);
                    mapData.hasUpPortal = false;
                }
                break;
            case Direction.Down:
                if (mapData.downPortalObject != null)
                {
                    mapData.downPortalObject.SetActive(false);
                    mapData.hasDownPortal = false;
                }
                break;
            case Direction.Left:
                if (mapData.leftPortalObject != null)
                {
                    mapData.leftPortalObject.SetActive(false);
                    mapData.hasLeftPortal = false;
                }
                break;
            case Direction.Right:
                if (mapData.rightPortalObject != null)
                {
                    mapData.rightPortalObject.SetActive(false);
                    mapData.hasRightPortal = false;
                }
                break;
        }
    }



    //-------맵 생성 함수
    private MapData CreateNewMap(Vector2Int position, MapData selectedMap)
    {
        return new MapData
        {
            mapX = position.x,
            mapY = position.y,
            sceneName = selectedMap.sceneName,
            hasUpPortal = selectedMap.hasUpPortal,
            hasDownPortal = selectedMap.hasDownPortal,
            hasLeftPortal = selectedMap.hasLeftPortal,
            hasRightPortal = selectedMap.hasRightPortal,
            upPortalObject = selectedMap.upPortalObject,
            downPortalObject = selectedMap.downPortalObject,
            leftPortalObject = selectedMap.leftPortalObject,
            rightPortalObject = selectedMap.rightPortalObject
        };
    }

    private string SelectRandomMapScene(Direction direction)
    {
    if (mapScenesList.Count == 0)
    {
        Debug.LogError("사용 가능한 맵 씬이 없습니다.");
        return string.Empty;
    }

            int randomIndex = Random.Range(0, mapScenesList.Count);
            return mapScenesList[randomIndex].sceneName;
        
    }


    private MapData FindMapWithPortal(string sceneName, Direction direction)
    {
        MapData selectedMap = mapScenesList.Find(mapData => mapData.sceneName == sceneName);

        if (selectedMap != null)
        {
            bool hasPortal = direction switch
            {
                Direction.Up => selectedMap.hasDownPortal,
                Direction.Down => selectedMap.hasUpPortal,
                Direction.Left => selectedMap.hasRightPortal,
                Direction.Right => selectedMap.hasLeftPortal,
                _ => false,
            };

            if (!hasPortal)
            {
                selectedMap = null;
            }
        }

        return selectedMap;
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
    public MapData GetAdjacentPosition(Vector2Int position)
    {
        Vector2Int[] directions = new Vector2Int[]
        {
        new Vector2Int(0, 1),  // Up
        new Vector2Int(0, -1), // Down
        new Vector2Int(-1, 0), // Left
        new Vector2Int(1, 0)   // Right
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int adjacentPosition = new Vector2Int(position.x + dir.x, position.y + dir.y);
            if (IsValidPosition(adjacentPosition))
            {
                MapData adjacentMap = worldMap[adjacentPosition.x, adjacentPosition.y];
                if (adjacentMap != null)
                    return adjacentMap;
            }
        }

        return null;
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

    //
    private void LoadMap(Vector2Int position, Direction direction)
    {
        MapData mapToLoad = worldMap[position.x, position.y];

        if (mapToLoad != null)
        {
            mapToLoad.enteredDirection = direction;
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mapToLoad.sceneName, LoadSceneMode.Additive);
            mapToLoad.IsLoaded = true;

            if (asyncLoad == null)
            {
                Debug.LogError($"로드 실패한 맵 이름: {mapToLoad.sceneName}");
                return;
            }

            //TODO:: 맵이 완료 됬을 때 체크하고 비활성화 하기, 맵생성은 안전하게해야함, 확신할수 있을 때
            //다음번에 만약 만들면 CHECK::체크하는 것과 맵생성은 따로하는게 좋음, 아예 클래스 하나를 만들어 거기에 포탈정보 넣고
            //그거에 따라 맵만들고, 씬로드
            //맵생성 한 후에 맵 체크-> 맵이 이상하면 다시 삭제후 다시 나머지 생성 -> 체크-> 반복해보기
            asyncLoad.completed += _ =>
            {
                Scene loadedScene = SceneManager.GetSceneByName(mapToLoad.sceneName);
                SceneManager.SetActiveScene(loadedScene);
                currentMap = mapToLoad;
                SetPlayerPosition(OppositeDirection(mapToLoad.enteredDirection));
            };
        }
        else
        {
            Debug.LogError($"로딩될 맵이 없습니다. 위치: ({position.x}, {position.y})");
            currentMap = null;
            MapCheck(new Vector2Int(centerX,centerY), direction);
            return;
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
        if (!IsValidPosition(new Vector2Int(x, y)))
        {
            return null;
        }
        return worldMap[x, y];
    }

    private void OnDisable()
    {
        for (int x = 0; x < worldMapSize; x++)
        {
            for (int y = 0; y < worldMapSize; y++)
            {
                MapData mapData = worldMap[x, y];
                if (mapData != null)
                {
                    ResetPortalObjects(mapData, true); // true 전달하여 활성화 모드로 실행
                }
            }
        }
    }

    private void ResetPortalObjects(MapData mapData, bool activateMode)
    {
        if (mapData.upPortalObject != null)
        {
            mapData.upPortalObject.SetActive(activateMode);
            mapData.hasUpPortal = activateMode;
        }

        if (mapData.downPortalObject != null)
        {
            mapData.downPortalObject.SetActive(activateMode);
            mapData.hasDownPortal = activateMode;
        }

        if (mapData.leftPortalObject != null)
        {
            mapData.leftPortalObject.SetActive(activateMode);
            mapData.hasLeftPortal = activateMode;
        }

        if (mapData.rightPortalObject != null)
        {
            mapData.rightPortalObject.SetActive(activateMode);
            mapData.hasRightPortal = activateMode;
        }
    }



}














