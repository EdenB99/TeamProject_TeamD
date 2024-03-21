using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Tooltip("변수들")]

    public GameObject map; // 첫 맵 사이즈 체크용
    public GameObject line; // 나눠진 공간 체크용
    public GameObject road; //방 사이즈 체크용
    public Vector2Int mapSize; //만들고 싶은 맵의 크기
    public float minimumDevidedRate; // 공간이 나눠지는 최소 비율
    public float maximumDevidedRate; // 최대 비율
    public int maximumDepth; // 트리의 높이, 맵을 2^높이 만큼 나눔
    int mapCount;
    const string BaseSceneName = "Map";
    string[] sceneNames;
    List<Node> rooms = new List<Node>();
    enum SceneLoadState : byte
    {
        Unload = 0, // 로딩이 안된 상태
        Loaded // 로딩이 된 상태

    }
    SceneLoadState[] sceneLoadState;




    //에디터에서 맵 다시그릴때 렌더러 전부 지우고 다시 그리게하기 위한 테스트용 리스트
    List<LineRenderer> lineRenderers = new List<LineRenderer>();
    public LineRenderer lineRenderer;



    void Start()
    {
        //맵 개수는 최대깊이^2
        mapCount = (int)Mathf.Pow(maximumDepth, 2);
        //씬 개수는 맵개수 만큼
        sceneNames = new string[mapCount];
        sceneLoadState = new SceneLoadState[mapCount];

        //만들어 놓은 전체 맵중 맵 개수만큼 랜덤하게 골라서 넣기
        for (int i = 0; i < mapCount; i++)
        {
            int index = i;                      //일단은 중복 허용이지만 맵개수 늘리면 중복안되게 리스트추가해서 막아야함.
            int random = Random.Range(1, 12); // 현재 만들어진 맵중에서 랜덤으로 뽑고

            sceneNames[index] = $"{BaseSceneName}{random}";      //<- 맞는지확인필요
            sceneLoadState[index] = SceneLoadState.Unload;

        }



        Node root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y)); //전체 맵 크기의 루트노드를 만듬
        DrawMap(0, 0);
        Divide(root, 0);
        GenerateLoad(root, 0);
        MapLoad();
    }





    void Divide(Node tree, int n)
    {
        if (n == maximumDepth)
        {
            // 최대 깊이에 도달하면 rooms 리스트에 추가
            rooms.Add(tree);
            return;
        }
        //아니면

        //가로와 세로중 더 긴것을 구하고, 가로가 길면 위쪽의 왼쪽,오른쪽, 세로가 길면 위,아래로 나눠준다.
        int maxLength = Mathf.Max(tree.nodeRect.width, tree.nodeRect.height);
        //최소길이와 최대 길이 사이에서 랜덤으로 값 구하기
        int split = Mathf.RoundToInt(Random.Range(maxLength * minimumDevidedRate, maxLength * maximumDevidedRate));

        //가로가 더 길면 좌 우로 나눔, 세로의 길이는 그대로다.
        if (tree.nodeRect.width >= tree.nodeRect.height)
        {   //왼쪽 노드의 정보, 위치는 좌측 하단 기준이라 변하지않고, 가로 길이는 위에서 구한 랜덤값을 넣어준다.
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, split, tree.nodeRect.height));
            //오른쪽 노드의 정보, 위치는 좌측 하단에서 오른쪽으로 가로 길이만큼 이동한 위치,
            //가로길이는 기존 가로길이 - 새로 구한 가로길이
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x + split, tree.nodeRect.y,
                                                  tree.nodeRect.width - split, tree.nodeRect.height));
            //위 두개의 노드를 나눠준 선을 그리는 함수
            DrawLine(new Vector2(tree.nodeRect.x + split, tree.nodeRect.y),
                     new Vector2(tree.nodeRect.x + split, tree.nodeRect.y + tree.nodeRect.height));
            rooms.Add(tree);
        }

        else //세로가 더 길다면,
        {    //위의 식을 y버전으로 바꿔주기만 하면 된다.
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split,
                                                 tree.nodeRect.width, tree.nodeRect.height - split));
            DrawLine(new Vector2(tree.nodeRect.x, tree.nodeRect.y + split),
                     new Vector2(tree.nodeRect.x + tree.nodeRect.width, tree.nodeRect.y + split));
            rooms.Add(tree);
        }


        tree.leftNode.parNode = tree; // 자식노드들의 부모노드를 나누기전 노드로 설정한다.
        tree.rightNode.parNode = tree;

        Divide(tree.leftNode, n + 1); //왼쪽,오른쪽 자식 노드들도 나눠주기
        Divide(tree.rightNode, n + 1); //n+1해서 최대 높이에 도달시 종료

    }

    void MapLoad()
    {
        List<int> availableScenes = new List<int>();


        for (int i = 0; i < mapCount; i++)
        {
            if (sceneLoadState[i] == SceneLoadState.Unload)
            {
                availableScenes.Add(i);
            }
        }

        int randomMap;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (availableScenes.Count > 0)     //랜덤하게 방넣기
            {
                int randomIndex = Random.Range(0, mapCount + 1);    //<-- 여기도 맵
                randomMap = availableScenes[randomIndex];        // 인덱스 아웃레인지 고치기
                availableScenes.RemoveAt(randomIndex);

                sceneLoadState[randomMap] = SceneLoadState.Loaded;

                AsyncOperation async = SceneManager.LoadSceneAsync(sceneNames[randomMap], LoadSceneMode.Additive);

                async.allowSceneActivation = true;


                async.completed += (_) =>
                {
                    Scene loadedScene = SceneManager.GetSceneByName(sceneNames[randomMap]);
                    if (i < mapCount)
                    {

                        if (i < rooms.Count)
                        {
                            GameObject[] root = loadedScene.GetRootGameObjects();
                            Vector3 position = (Vector2)rooms[i].nodeRect.center;          // vector2int를 vector2로 변환
                            for(int j=0; j < root.Length; j++)
                            {
                            root[j].transform.position =  position; //맵이 나오긴하는데 뒤죽박죽 이상하게 나옴 position을 고쳐야함
                            }
                            SceneManager.SetActiveScene(loadedScene);             // 선이 맵에 딸려나오는 문제, 맵이 가운데에 나오는 문제
                             // 맵이 wrapper managed-to-native 오류나면서 안나오는 경우
                        }
                    }
                    else
                    {
                        
                    }
                };
            }
            else
            {
                break; // 더 이상 사용 가능한 씬이 없으면 종료
            }
        }
    }


    [Tooltip("맵 그리기")]
    private void DrawMap(int x, int y) //x y는 화면의 중앙위치를 뜻한다.
    {
        // -mapsize/2를 하는 이유는 화면의 중앙에서 화면의 크기의 반을 빼줘야 좌측 하단좌표를 구할 수 있기 때문이다.
        lineRenderer = Instantiate(map).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2); //좌측 하단
        lineRenderer.SetPosition(1, new Vector2(x + mapSize.x, y) - mapSize / 2); //우측 하단
        lineRenderer.SetPosition(2, new Vector2(x + mapSize.x, y + mapSize.y) - mapSize / 2);//우측 상단
        lineRenderer.SetPosition(3, new Vector2(x, y + mapSize.y) - mapSize / 2); //좌측 상단

        lineRenderers.Add(lineRenderer);
    }
    private void DrawLine(Vector2 from, Vector2 to)
    {
        lineRenderer = Instantiate(line).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from - mapSize / 2);
        lineRenderer.SetPosition(1, to - mapSize / 2);


        lineRenderers.Add(lineRenderer);
    }


    private void GenerateLoad(Node tree, int n)
    {
        if (n == maximumDepth) // 방금만들어진 방이면 이을 방이 없음.
        {
            return; // 바로 리턴
        }
        Vector2Int leftNodeCenter = tree.leftNode.center;
        Vector2Int rightNodeCenter = tree.rightNode.center;

        //leftnode에 세로기준을 맞춰 가로선으로 연결
        DrawRoad(new Vector2(leftNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, leftNodeCenter.y));
        //rightnode에 가로기준에 맞춰 세로선으로 연결
        DrawRoad(new Vector2(rightNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, rightNodeCenter.y));
        GenerateLoad(tree.leftNode, n + 1); //자식 노드들도 탐색
        GenerateLoad(tree.rightNode, n + 1);
    }

    private void DrawRoad(Vector2 from, Vector2 to) //길 그리기
    {
        lineRenderer = Instantiate(road).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from - mapSize / 2);
        lineRenderer.SetPosition(1, to - mapSize / 2);


        lineRenderers.Add(lineRenderer);
    }

    [Tooltip("유니티 작업용 맵 재생성")]
    public void ReDrow()
    {
        //리스트에 추가해놓은 라인렌더러 다 삭제하기
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            if (lineRenderer != null)
            {
                Destroy(lineRenderer.gameObject);
            }
        }
        lineRenderers.Clear();
        for (int i = 0; i < sceneNames.Length; i++)
        {

        }

        Start();
    }




}