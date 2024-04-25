using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


//TODO:: ���� �����ʳѾ�� ��Ż ����, �����ʿ� ���� -> ������Ż -> ������
public class MapManager : MonoBehaviour
{
    //TODO :: ������ �÷��� �����
    //TODO Low:: �����ͷ� �ѱ�ȭ �߰��ϱ�
    [Header("����")]

private Player player;
    [SerializeField] private int mapSize = 20;
    [Header("�����ũ��(MapSize*MapSize)")]
    [SerializeField] private int worldMapSize = 7;
    public int WorldMapSize => worldMapSize;
    [SerializeField] private int currentMapCount = 0;
    [SerializeField] private GameObject[] mapPrefabs;
    [SerializeField] private MapData[] mapScenes;
    private Dictionary<Vector2Int, MapData> worldMap = new Dictionary<Vector2Int, MapData>();
    private int centerX;
    private int centerY;
    //TODO:: ���������� ���� MapUIManager mapUIManager;


    [Header("���۸� ����ƮǮ")]
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

    //���۸� �������� �ϳ� ���ϱ�
    private void LoadStartMap()
    {
        MapData randomStartMap = startMapScenes[UnityEngine.Random.Range(0, startMapScenes.Length)];

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

        worldMap[new Vector2Int(centerX, centerY)] = startMap;
        MapCheck(new Vector2Int(centerX, centerY));
    }





    private void Start()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        mapUI = GameObject.FindAnyObjectByType<MapUI>();
        GenerateWorldMap();
    }


    private void AutoFillMapData()
    {
        // StartMapScenes ó��
        startMapScenes = new MapData[startMapPrefabs.Length];
        for (int i = 0; i < startMapPrefabs.Length; i++)
        {
            GameObject mapPrefab = startMapPrefabs[i];
            if (mapPrefab == null)
            {
                Debug.LogWarning($"StartMapScenes {i}�� ����ֽ��ϴ�.");
                continue;
            }

            MapData mapData = new MapData();
            mapData.sceneName = mapPrefab.name;

            // ��Ż ���� ����
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

            // �ٸ� ��Ż ������ �����ϰ� ����

            startMapScenes[i] = mapData;
        }


        mapScenes = new MapData[mapPrefabs.Length];

        for (int i = 0; i < mapPrefabs.Length; i++)
        {
            GameObject mapPrefab = mapPrefabs[i];

            if (mapPrefab == null)
            {
                Debug.LogWarning($"MapScenes {i}�� ����ֽ��ϴ�.");
                continue;
            }

            MapData mapData = new MapData();
            mapData.sceneName = mapPrefab.name;

            // ��Ż ���� ����
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

    //TODO:: ������ �ʻ����ȵǴ� ���� ������,GenerateAdditionalMaps�ΰŰ���
    private void GenerateWorldMap()
    {
        currentMapCount = 1;
        Queue<MapData> mapQueue = new Queue<MapData>();
        mapQueue.Enqueue(worldMap[new Vector2Int(centerX, centerY)]);

        int maxGenCount = 100; // �ִ� �ݺ� Ƚ�� ����
        int genCount = 0;

        while (mapQueue.Count > 0 && currentMapCount < mapSize && genCount < maxGenCount)
        {
            MapData currentMap = mapQueue.Dequeue();
            GenerateAdjacentMaps(currentMap, mapQueue);

            // �� ���� �� �� üũ �� ����
            CheckWorldMap();
            RemoveInvalidMaps();
            GenerateAdditionalMaps();

            genCount++;
        }

        if (genCount >= maxGenCount)
        {
            Debug.LogError("�� ������ �����߽��ϴ�. ���� ���� �߻�.");
            return;
        }

        // ��Ż ��Ȱ��ȭ
        foreach (var kvp in worldMap)
        {
            MapData mapData = kvp.Value;
            CheckAndDisablePortal(mapData);
        }
    }

    //�� ������ �����ϴ� �Լ�
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
                worldMap[newPosition] = newMap;
                currentMapCount++;
                mapQueue.Enqueue(newMap);
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

    //��� ������ ���� üũ
    private List<Direction> GetAvailableDirections(MapData mapData)
    {
        List<Direction> availableDirections = new List<Direction>();

        if (mapData.hasUpPortal)
            availableDirections.Add(Direction.Up);
        if (mapData.hasDownPortal)
            availableDirections.Add(Direction.Down);
        if (mapData.hasLeftPortal)
            availableDirections.Add(Direction.Left);
        if (mapData.hasRightPortal)
            availableDirections.Add(Direction.Right);

        return availableDirections;
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



    //--------�� üũ�Լ�
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

    //�������� ��Ż���� Ȯ��
    private bool HasValidPortalConnections(MapData mapData)
    {
        if (mapData.hasUpPortal && !IsValidPortalConnection(mapData, Direction.Up))
            return false;
        if (mapData.hasDownPortal && !IsValidPortalConnection(mapData, Direction.Down))
            return false;
        if (mapData.hasLeftPortal && !IsValidPortalConnection(mapData, Direction.Left))
            return false;
        if (mapData.hasRightPortal && !IsValidPortalConnection(mapData, Direction.Right))
            return false;

        return true;
    }

    //�ش���Ż�� ������ ���� üũ
    private bool IsValidPortalConnection(MapData mapData, Direction direction)
    {
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
                        return adjacentMapData.hasDownPortal;
                    case Direction.Down:
                        return adjacentMapData.hasUpPortal;
                    case Direction.Left:
                        return adjacentMapData.hasRightPortal;
                    case Direction.Right:
                        return adjacentMapData.hasLeftPortal;
                }
            }
        }

        return false;
    }




    //=========��üũ(����)

    private void RemoveInvalidMaps()
    {
        List<Vector2Int> invalidPositions = new List<Vector2Int>();

        foreach (var kvp in worldMap)
        {
            Vector2Int position = kvp.Key;
            MapData mapData = kvp.Value;

            if (!HasValidPortalConnections(mapData))
            {
                bool canRemove = true;
                foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                {
                    Vector2Int adjacentPosition = GetAdjacentPosition(position.x, position.y, dir);
                    if (worldMap.ContainsKey(adjacentPosition) && HasValidPortalConnections(worldMap[adjacentPosition]))
                    {
                        canRemove = false;
                        break;
                    }
                }

                if (canRemove)
                {
                    invalidPositions.Add(position);
                    currentMapCount--;
                }
            }
        }

        foreach (Vector2Int position in invalidPositions)
        {
            worldMap.Remove(position);
        }
    }



    //-----------��üũ(�����)
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
                        newMapPositions.Add(newPosition);
                        currentMapCount++;
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



    //----------��Ż ����
    //TODO::��Ż ���������� ��¦ ��ȭ���ʿ䰡����
private void CheckAndDisablePortal(MapData mapData)
{
    // ���� ���� ��Ż ���⿡ ���� �ݴ� ������ ��Ż üũ
    if (mapData.hasUpPortal)
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

    if (mapData.hasDownPortal)
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

    if (mapData.hasLeftPortal)
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

    if (mapData.hasRightPortal)
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



    //-------�� ���� �Լ�
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
        List<MapData> availableMaps = mapScenes.Where(mapData =>
        {
            switch (direction)
            {
                case Direction.Up:
                    return mapData.hasDownPortal;
                case Direction.Down:
                    return mapData.hasUpPortal;
                case Direction.Left:
                    return mapData.hasRightPortal;
                case Direction.Right:
                    return mapData.hasLeftPortal;
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
                    return mapData.hasDownPortal ? mapData : null;
                case Direction.Down:
                    return mapData.hasUpPortal ? mapData : null;
                case Direction.Left:
                    return mapData.hasRightPortal ? mapData : null;
                case Direction.Right:
                    return mapData.hasLeftPortal ? mapData : null;
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



    //---------�� �ε� �Լ�
    //TODO:: ���ʿ��� �� ������ �Ϲݸ�,������(1����),�̺�Ʈ��(2����)������ �ϵ�
    //�����ʰ� �̺�Ʈ���� ������ ������ ä��������.
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
            Debug.LogError($"��ȿ���� ���� �� ��ġ�Դϴ�. ��ġ: ({position.x}, {position.y})");
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
                    Debug.LogError($"�ҷ����µ� ������ ��: {mapToLoad.sceneName}");
                    return;
                }

                asyncLoad.completed += _ =>
                {
                    Scene loadedScene = SceneManager.GetSceneByName(mapToLoad.sceneName);
                    SceneManager.SetActiveScene(loadedScene);
                    currentMap = mapToLoad;
                    currentMap.isVisited = true;
                    mapUI.UpdateMapUI();
                    SetPlayerPosition(OppositeDirection(mapToLoad.enteredDirection));
                };
            }
            else
            {
                Debug.LogError($"�ش���ǥ�� �� �����͸� ã�� �� �����ϴ�.: ({position.x}, {position.y})");
            }
        }
        else
        {
            Debug.LogError($"�ش���ǥ�� ���� �����ϴ�.: ({position.x}, {position.y})");
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
                Debug.LogWarning($"{direction}������ ��Ż���� �÷��̾ ������ ������ ã�� ���߽��ϴ�.");
            }
        }
        else
        {

            Debug.LogWarning($"����ʿ� {direction}������ ��Ż�� ã�� ���߽��ϴ�.");
            
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

    private void OnDisable()
    {
        foreach (var kvp in worldMap)
        {
            MapData mapData = kvp.Value;
            ResetPortalObjects(mapData, true);
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














