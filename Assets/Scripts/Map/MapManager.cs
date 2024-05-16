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
    //TODO L:: �� �ε������ 
    //TODO:: ������ �����
    //TODO:: �����̵�����
    //TODO:: ���� �����ؾ���.

    //ã�� ���� :: ����ǻ�Ϳ��� ������ ������ �����̴� ��찡����
    //�÷��̾ ��ð� ���� ���̾ �ٽ� ���ƿԴµ� �뽬�� ���������� ��� ���� �΋H���� ���� �ָ� �� �� ����.
    //���� ������ �����ϸ� ���� ����Ʈ���� null �߻��ϴ� ��찡 ����(�ַ� ���̷����� ���� �� �߻�)
    //���� �÷��̾�ؿ��ְų� �Ķ��ذ��� �÷��̾�� ���� ��� ���� ��������
    //�������� ���� ������ ���ƿö����� �ٸ��������� �Ǿ�����(�Ծ��� �� �����������̿���.)
    
    //��� ������ ������ �̻��Ѱ����� �̵��Ǹ� �������� � �Ծ����� 
    //indexOutOfRangeException: Index was outside the bounds of the array., Assets/Scripts/Core/Factory.cs:76 
    //Assets/Scripts/Map/MapManager.cs:1251)  (at Assets/Scripts/Map/MapManager.cs:1026)
    //������ ����



    [Header("����")]

    private Player player;
    [Header("��ü �� ����(+-1~3)")]
    [SerializeField] private int mapSize = 20;
    [Header("�����ũ��(MapSize*MapSize)")]
    [SerializeField] private int worldMapSize = 7;
    public int WorldMapSize => worldMapSize;
    [SerializeField] private int currentMapCount = 0;
    [Header("���������� ������")]
    [SerializeField] private GameObject[] mapPrefabs;
    private MapData[] mapScenes;
    private Dictionary<Vector2Int, MapData> worldMap = new Dictionary<Vector2Int, MapData>();
    private HashSet<string> usedMapScenes = new HashSet<string>();
    private int centerX;
    private int centerY;
    Vector2Int currentPosition;


    //Ư�� �� ����
    private bool hasNextStageMap = false;
    MapData startMapData;


    [Header("���۸� ������")]
    [SerializeField] private GameObject[] startMapPrefabs;
    private MapData[] startMapScenes;
    [Header("�������������� ������")]
    [SerializeField] private GameObject[] nextStageMapPrefabs;
    private MapData[] nextStageMapScenes;

    private MapData currentMap;
    public MapData CurrentMap
    {
        get { return currentMap; }
        private set { currentMap = value; }
    }
    private MapUI mapUI;
    //������
    private GameObject enemyParent;
    EnemyBase_[] enemies;
    protected bool isEnemyDead = false;

    //��Ż ����
    private Portal[] portals;
    private Dictionary<Portal, SpriteRenderer[]> portalSpriteRenderers = new Dictionary<Portal, SpriteRenderer[]>();

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

    //�������� ������� �ڵ����� MapData[] ������ ä���ִ� �Լ�
    private void AutoFillMapData()
    {
        // startMapScenes ó��
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

            // �ٸ� ��Ż ������ �����ϰ� ����

            startMapScenes[i] = mapData;
        }

        //mapScenes ó��
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

        //nextStageMapScenes ó��
        nextStageMapScenes = new MapData[nextStageMapPrefabs.Length];

        for (int i = 0; i < nextStageMapPrefabs.Length; i++)
        {
            GameObject mapPrefab = nextStageMapPrefabs[i];

            if (mapPrefab == null)
            {
                Debug.LogWarning($"MapScenes {i}�� ����ֽ��ϴ�.");
                continue;
            }

            MapData mapData = new MapData();
            mapData.sceneName = mapPrefab.name;

            // ��Ż ���� ����
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
        Debug.Log("�ʻ����� ����");

        //���۸� ������ �ʱ�ȭ�ϰ� �ֱ�
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
                    Debug.LogError("�� ������ �ߴܵǾ����ϴ�.");
                    break;
                }
            }

            if (currentMapCount < mapSize)
            {
                if (currentMapCount < mapSize || mapQueue.Count <= 0)
                {
                    //�� ���� ����� ������ �� currentMapCount�� 14, mapSize�� 14�� QueCount�� ��������ϰ������� CheckAndActivatePortals�� �ϰ� ����� ���ƿ�,currentMapCount�� 1�̵�.
                    //�ϴ� �����س���, ������ �ٽ� �ذ��� ��
                    Debug.LogWarning("���� ������ �����ϰų� ��ť�� ����־� ��� ���� �����ϰ� �ٽ� �����մϴ�.");
                    yield return ResetGenerateWorldMap();
                    currentMapCount = 1;
                    genCount = 0;
                }

            }
        }
        if (!hasNextStageMap)
        {
            // worldMap�� �������� ��ȸ
            var reversedWorldMap = worldMap.Reverse();
            int mapIndex = 1;

            foreach (var kvp in reversedWorldMap)
            {
                MapData mapData = kvp.Value;

                if (HasValidPortalConnections(mapData))
                {
                    // ��ȿ�� �˻縦 ����� ù ��° �ʿ� NextStageMap ��ġ
                    Vector2Int position = new Vector2Int(mapData.mapX, mapData.mapY);
                    Debug.Log($"�ڿ��� {mapIndex}��° ��,  {mapData.sceneName}���� NextStageMap �Ҵ�."); // �� �ε��� ���
                    Debug.Log($"NextMap�� ��ǥ:{position}");
                    mapData = SelectRandomMapFromNextStageMapScenes();
                    mapData.mapX = position.x;
                    mapData.mapY = position.y;
                    mapData.isNextStageRoom = true;
                    worldMap[position] = mapData;

                    hasNextStageMap = true;
                    CheckAndActivatePortals(); 
                    break;
                }

                mapIndex++; // �� �ε��� ����
            }

            // ��ȿ�� �˻縦 ����ϴ� ���� ã�� ���� ���
            if (!hasNextStageMap)
            {
                Debug.LogWarning("��ȿ�� �˻縦 ����ϴ� ���� ã�� ���߽��ϴ�. ���� �ٽ� �����մϴ�.");
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

    //����
    private IEnumerator ResetGenerateWorldMap()
    {
        // ��� ��Ż ��ü Ȱ��ȭ
        foreach (var kvp in worldMap)
        {
            MapData mapData = kvp.Value;
            ResetPortalObjects(mapData, true);
        }

        // worldMap�� usedMapScenes �ʱ�ȭ
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






    //�� ������ �����ϴ� �Լ�
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

    //��� ������ ���� üũ
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





    //�������������� ���� �� �ҷ����� �Լ�
    private MapData SelectRandomMapFromNextStageMapScenes()
    {
        if (nextStageMapScenes.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, nextStageMapScenes.Length);
            return nextStageMapScenes[randomIndex];
        }

        return null;
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
        if (mapData == null)
        {
            return false;
        }

        //���������� ���� ���� ��Ż�� �������
        //TODO:: �۵�����
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

    //�ش���Ż�� ������ ���� üũ
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




    //=========��üũ(����)

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

                    // nextStageMapScenes���� ���õ� ���� �������� usedMapScenes���� nextStageMapScenes�� ��� �� sceneName ����
                    if (nextStageMapScenes.Any(m => m.sceneName == mapData.sceneName))
                    {
                        Debug.Log($"�������������� �̵����� RemoveInvalidMaps���� �����Ǿ����ϴ�.");
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



    //----------��Ż ����
    //TODO::��Ż ���������� ��¦ ��ȭ���ʿ䰡����
    private void CheckAndDisablePortal(MapData mapData)
    {
        if (mapData == null)
        {
            return;
        }

        // ���� ���� ��Ż ���⿡ ���� �ݴ� ������ ��Ż üũ
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



    //-------�� ���� �Լ�
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
        //TODO:: null ����
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



    //---------�� �ε� �Լ�
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
            SceneManager.UnloadSceneAsync(currentMap.sceneName);
            SceneManager.LoadScene("ErrorMap", LoadSceneMode.Additive);
            //TODO:: �ش� �ʿ� �����µ� ����� ��Ż ����, �������հ��ϱ�
        }
    }

    private IEnumerator UnloadAndLoadMap(Vector2Int position, Direction direction)
    {
        AsyncOperation unloadOperation = null;

        // ���� �� ��ε�
        if (currentMap != null)
        {
            unloadOperation = SceneManager.UnloadSceneAsync(currentMap.sceneName);

            // unloadOperation�� null�� �ƴ� ��쿡�� ����
            if (unloadOperation != null)
            {
                while (!unloadOperation.isDone)
                {
                    yield return null;
                }
            }
            else
            {
                Debug.LogError($"�� ��ε忡 ����: {currentMap.sceneName}");
            }
        }

        // �� �� �ε�
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
                    SceneManager.LoadScene("ErrorMap",LoadSceneMode.Additive);
                    //TODO:: �ش� �ʿ� �����µ� ����� ��Ż ����, �������հ��ϱ�
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
                Debug.LogError($"�ش���ǥ�� �� �����͸� ã�� �� �����ϴ�.: ({position.x}, {position.y})");
                SceneManager.LoadScene("ErrorMap", LoadSceneMode.Additive);
                //TODO:: �ش� �ʿ� �����µ� ����� ��Ż ����, �������հ��ϱ�
            }
        }
        else
        {
            Debug.LogError($"�ش���ǥ�� ���� �����ϴ�.: ({position.x}, {position.y})");
            SceneManager.UnloadSceneAsync(currentMap.sceneName);
            SceneManager.LoadScene("ErrorMap", LoadSceneMode.Additive);
            //TODO:: �ش� �ʿ� �����µ� ����� ��Ż ����, �������հ��ϱ�
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
    //�� üũ �Լ�
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
    //��Ż �� ���� �Լ�
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
            Debug.Log("�÷��̾� ��ü�� �Ҵ���� �ʾҽ��ϴ�.");
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
                Debug.LogWarning($"{direction}������ ��Ż���� �÷��̾ ������ ������ ã�� ���߽��ϴ�.");
            }
        }
        else
        {

            GameObject.FindAnyObjectByType<ReturnZone>().transform.parent.Find($"{direction}Portal").gameObject.SetActive(true);
            portalObject = GameObject.FindAnyObjectByType<ReturnZone>().transform.parent.Find($"{direction}Portal").gameObject;
            if (portalObject != null)
            {
                Debug.LogWarning($"����ʿ� {direction}������ ��Ż�� ������ �����־����ϴ�.");
                UpdatePortalState(currentMap.hasEnemies);
                Transform playerSpawnPoint = portalObject.transform.GetChild(0); 
                if (playerSpawnPoint != null)
                {
                    player.transform.position = playerSpawnPoint.position;
                }
            }
            else
            {
                Debug.LogWarning($"����ʿ� {direction}������ ��Ż�� �����ϴ�.");
                player.transform.position = new Vector3(0, 0, player.transform.position.z);
            }
        }
    }

    //��Ż ���� ��
    public void EnterPortal(Direction direction)
    {
        currentPosition = new Vector2Int(currentMap.mapX, currentMap.mapY);
        Vector2Int MapPosition = GetAdjacentPosition(currentMap.mapX, currentMap.mapY, direction);
        if (IsValidPosition(MapPosition))
        {
            SaveMapState(currentPosition); // ���� ���� ���¸� ����
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

    //�� ����,�ε�==============================================


    /// <summary>
    /// ���� �����ϱ� ���� ����ϴ� �Լ�
    /// </summary>
    /// <param name="position"></param>
    private void SaveMapState(Vector2Int position)
    {
        // �������� ����Ǵ� ��
        MapData map = GetMapData(position.x, position.y);

        List<SaveItemData> saveItemDatas = new List<SaveItemData>();

        // �ʿ��� ������ ��������
        ItemObject[] itemObjects = FindObjectsOfType<ItemObject>();

        // ���������� ����Ʈ �ۼ�
        for (int i = 0; i < itemObjects.Length; i++)
        {
            saveItemDatas.Add(new SaveItemData
            {
                itemCode = itemObjects[i].ItemData.code,
                itemPositions = (Vector2)itemObjects[i].transform.position
            });

            // �� �� ��Ȱ��ȭ
            GameObject obj = itemObjects[i].gameObject;
            obj.SetActive(false);
        }

        // �ʿ� �ۼ��� ����Ʈ �ֱ�
        if (map != null)
        {
        map.mapItemDatas = saveItemDatas;

        }



    }


    /// <summary>
    /// ���� �ҷ����� ���� ����ϴ� �Լ�
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



    //�÷��̾� ����� ���ʱ�ȭ
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

        // 1. �� ������ ����
        foreach (var kvp in worldMap)
        {
            MapData mapData = kvp.Value;
            ResetPortalObjects(mapData, true);
            if (mapData != null)
            {
                // �� ������ ���� ���� ������Ʈ �� ������Ʈ ����
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














