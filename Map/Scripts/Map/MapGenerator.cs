using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [Tooltip("������")]

    public GameObject map; // ù �� ������ üũ��
    public GameObject line; // ������ ���� üũ��
    public GameObject road; //�� ������ üũ��
    public Vector2Int mapSize; //����� ���� ���� ũ��
    public float minimumDevidedRate; // ������ �������� �ּ� ����
    public float maximumDevidedRate; // �ִ� ����
    public int maximumDepth; // Ʈ���� ����, ���� 2^���� ��ŭ ����
    int mapCount;
    const string BaseSceneName = "Map";
    string[] sceneNames;
    List<Node> rooms = new List<Node>();
    enum SceneLoadState : byte
    {
        Unload = 0, // �ε��� �ȵ� ����
        Loaded // �ε��� �� ����

    }
    SceneLoadState[] sceneLoadState;




    //�����Ϳ��� �� �ٽñ׸��� ������ ���� ����� �ٽ� �׸����ϱ� ���� �׽�Ʈ�� ����Ʈ
    List<LineRenderer> lineRenderers = new List<LineRenderer>();
    public LineRenderer lineRenderer;



    void Start()
    {
        //�� ������ �ִ����^2
        mapCount = (int)Mathf.Pow(maximumDepth, 2);
        //�� ������ �ʰ��� ��ŭ
        sceneNames = new string[mapCount];
        sceneLoadState = new SceneLoadState[mapCount];

        //����� ���� ��ü ���� �� ������ŭ �����ϰ� ��� �ֱ�
        for (int i = 0; i < mapCount; i++)
        {
            int index = i;                      //�ϴ��� �ߺ� ��������� �ʰ��� �ø��� �ߺ��ȵǰ� ����Ʈ�߰��ؼ� ���ƾ���.
            int random = Random.Range(1, 12); // ���� ������� ���߿��� �������� �̰�

            sceneNames[index] = $"{BaseSceneName}{random}";      //<- �´���Ȯ���ʿ�
            sceneLoadState[index] = SceneLoadState.Unload;

        }



        Node root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y)); //��ü �� ũ���� ��Ʈ��带 ����
        DrawMap(0, 0);
        Divide(root, 0);
        GenerateLoad(root, 0);
        MapLoad();
    }





    void Divide(Node tree, int n)
    {
        if (n == maximumDepth)
        {
            // �ִ� ���̿� �����ϸ� rooms ����Ʈ�� �߰�
            rooms.Add(tree);
            return;
        }
        //�ƴϸ�

        //���ο� ������ �� ����� ���ϰ�, ���ΰ� ��� ������ ����,������, ���ΰ� ��� ��,�Ʒ��� �����ش�.
        int maxLength = Mathf.Max(tree.nodeRect.width, tree.nodeRect.height);
        //�ּұ��̿� �ִ� ���� ���̿��� �������� �� ���ϱ�
        int split = Mathf.RoundToInt(Random.Range(maxLength * minimumDevidedRate, maxLength * maximumDevidedRate));

        //���ΰ� �� ��� �� ��� ����, ������ ���̴� �״�δ�.
        if (tree.nodeRect.width >= tree.nodeRect.height)
        {   //���� ����� ����, ��ġ�� ���� �ϴ� �����̶� �������ʰ�, ���� ���̴� ������ ���� �������� �־��ش�.
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, split, tree.nodeRect.height));
            //������ ����� ����, ��ġ�� ���� �ϴܿ��� ���������� ���� ���̸�ŭ �̵��� ��ġ,
            //���α��̴� ���� ���α��� - ���� ���� ���α���
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x + split, tree.nodeRect.y,
                                                  tree.nodeRect.width - split, tree.nodeRect.height));
            //�� �ΰ��� ��带 ������ ���� �׸��� �Լ�
            DrawLine(new Vector2(tree.nodeRect.x + split, tree.nodeRect.y),
                     new Vector2(tree.nodeRect.x + split, tree.nodeRect.y + tree.nodeRect.height));
            rooms.Add(tree);
        }

        else //���ΰ� �� ��ٸ�,
        {    //���� ���� y�������� �ٲ��ֱ⸸ �ϸ� �ȴ�.
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split,
                                                 tree.nodeRect.width, tree.nodeRect.height - split));
            DrawLine(new Vector2(tree.nodeRect.x, tree.nodeRect.y + split),
                     new Vector2(tree.nodeRect.x + tree.nodeRect.width, tree.nodeRect.y + split));
            rooms.Add(tree);
        }


        tree.leftNode.parNode = tree; // �ڽĳ����� �θ��带 �������� ���� �����Ѵ�.
        tree.rightNode.parNode = tree;

        Divide(tree.leftNode, n + 1); //����,������ �ڽ� ���鵵 �����ֱ�
        Divide(tree.rightNode, n + 1); //n+1�ؼ� �ִ� ���̿� ���޽� ����

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
            if (availableScenes.Count > 0)     //�����ϰ� ��ֱ�
            {
                int randomIndex = Random.Range(0, mapCount + 1);    //<-- ���⵵ ��
                randomMap = availableScenes[randomIndex];        // �ε��� �ƿ������� ��ġ��
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
                            Vector3 position = (Vector2)rooms[i].nodeRect.center;          // vector2int�� vector2�� ��ȯ
                            for(int j=0; j < root.Length; j++)
                            {
                            root[j].transform.position =  position; //���� �������ϴµ� ���׹��� �̻��ϰ� ���� position�� ���ľ���
                            }
                            SceneManager.SetActiveScene(loadedScene);             // ���� �ʿ� ���������� ����, ���� ����� ������ ����
                             // ���� wrapper managed-to-native �������鼭 �ȳ����� ���
                        }
                    }
                    else
                    {
                        
                    }
                };
            }
            else
            {
                break; // �� �̻� ��� ������ ���� ������ ����
            }
        }
    }


    [Tooltip("�� �׸���")]
    private void DrawMap(int x, int y) //x y�� ȭ���� �߾���ġ�� ���Ѵ�.
    {
        // -mapsize/2�� �ϴ� ������ ȭ���� �߾ӿ��� ȭ���� ũ���� ���� ����� ���� �ϴ���ǥ�� ���� �� �ֱ� �����̴�.
        lineRenderer = Instantiate(map).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(1, new Vector2(x + mapSize.x, y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(2, new Vector2(x + mapSize.x, y + mapSize.y) - mapSize / 2);//���� ���
        lineRenderer.SetPosition(3, new Vector2(x, y + mapSize.y) - mapSize / 2); //���� ���

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
        if (n == maximumDepth) // ��ݸ������ ���̸� ���� ���� ����.
        {
            return; // �ٷ� ����
        }
        Vector2Int leftNodeCenter = tree.leftNode.center;
        Vector2Int rightNodeCenter = tree.rightNode.center;

        //leftnode�� ���α����� ���� ���μ����� ����
        DrawRoad(new Vector2(leftNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, leftNodeCenter.y));
        //rightnode�� ���α��ؿ� ���� ���μ����� ����
        DrawRoad(new Vector2(rightNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, rightNodeCenter.y));
        GenerateLoad(tree.leftNode, n + 1); //�ڽ� ���鵵 Ž��
        GenerateLoad(tree.rightNode, n + 1);
    }

    private void DrawRoad(Vector2 from, Vector2 to) //�� �׸���
    {
        lineRenderer = Instantiate(road).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from - mapSize / 2);
        lineRenderer.SetPosition(1, to - mapSize / 2);


        lineRenderers.Add(lineRenderer);
    }

    [Tooltip("����Ƽ �۾��� �� �����")]
    public void ReDrow()
    {
        //����Ʈ�� �߰��س��� ���η����� �� �����ϱ�
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