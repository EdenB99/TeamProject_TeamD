using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{

    public GameObject map; // ù �� ������ üũ��
    public GameObject line; // ������ ���� üũ��
    public GameObject roomLine; //�� ������ üũ��
    public Vector2Int mapSize; //����� ���� ���� ũ��
    public float minimumDevidedRate; // ������ �������� �ּ� ����
    public float maximumDevidedRate; // �ִ� ����
    public int maximumDepth; // Ʈ���� ����, ���� 2^���� ��ŭ ����



    public LineRenderer lineRenderer;
    //�����Ϳ��� �� �ٽñ׸��� ������ ���� ����� �ٽ� �׸����ϱ� ���� ����Ʈ
    private List<LineRenderer> lineRenderers = new List<LineRenderer>(); 


    void Start()
    {


        Node root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y)); //��ü �� ũ���� ��Ʈ��带 ����
        DrawMap(0, 0);
        Divide(root, 0);
        GenerateRoom(root, 0);
    }




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


    void Divide(Node tree, int n)
    {
        if (n == maximumDepth) return; // �ִ���̿� �����ϸ� �� ������ �ʴ´�.
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
        }

        else //���ΰ� �� ��ٸ�,
        {    //���� ���� y�������� �ٲ��ֱ⸸ �ϸ� �ȴ�.
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split,
                                                 tree.nodeRect.width, tree.nodeRect.height - split));
            DrawLine(new Vector2(tree.nodeRect.x, tree.nodeRect.y + split),
                     new Vector2(tree.nodeRect.x + tree.nodeRect.width, tree.nodeRect.y + split));
        }
        tree.leftNode.parNode = tree; // �ڽĳ����� �θ��带 �������� ���� �����Ѵ�.
        tree.rightNode.parNode = tree;

        Divide(tree.leftNode, n + 1); //����,������ �ڽ� ���鵵 �����ֱ�
        Divide(tree.rightNode, n + 1); //n+1�ؼ� �ִ� ���̿� ���޽� ����


    }

    private void DrawLine(Vector2 from, Vector2 to)
    {
        lineRenderer = Instantiate(line).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from - mapSize / 2);
        lineRenderer.SetPosition(1, to - mapSize / 2);


        lineRenderers.Add(lineRenderer);
    }

    private RectInt GenerateRoom(Node tree, int n)
    {
        RectInt rect;

        if (n == maximumDepth) // �������� �����̸� �����
        {
            rect = tree.nodeRect; 
            int width = Random.Range(rect.width / 2 ,rect.width - 1);
            int height = Random.Range(1, rect.width - width);
            int x = rect.x + Random.Range(1, rect.width - width);
            int y = rect.y + Random.Range(1, rect.height - height);
            rect = new RectInt(x, y, width, height);
            DrawRectangle(rect);
        }
        else
        {
            tree.leftNode.roomRect = GenerateRoom(tree.leftNode, n + 1);
            tree.rightNode.roomRect = GenerateRoom(tree.rightNode, n + 1);
            rect = tree.leftNode.roomRect;
        }
        return rect;

    }

    private void DrawRectangle(RectInt rect)
    {
        LineRenderer lineRenderer = Instantiate(roomLine).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(rect.x, rect.y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(1, new Vector2(rect.x + rect.width, rect.y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(2, new Vector2(rect.x + rect.width, rect.y + rect.height) - mapSize / 2);//���� ���
        lineRenderer.SetPosition(3, new Vector2(rect.x, rect.y + rect.height) - mapSize / 2); //���� ���

        lineRenderers.Add(lineRenderer);
    }


    public void ReDrow()
    {
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            if (lineRenderer != null)
            {
                Destroy(lineRenderer.gameObject);
            }
        }
        lineRenderers.Clear();
        Start();
    }




}