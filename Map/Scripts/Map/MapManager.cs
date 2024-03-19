using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{


    /// <summary>
    /// �� �̸�(+����)
    /// </summary>
    const string BaseSceneName = "Map";
    /// <summary>
    /// �� ����� �迭
    /// </summary>
    string[] sceneNames;

    /// <summary>
    /// ���� �ε����¸� ��Ÿ���� ���� enum
    /// </summary>
    enum SceneLoadState : byte
    {
        Unload =0, // �ε��� �ȵ� ����
        Loaded // �ε��� �� ����

    }
    /// <summary>
    /// ������ �ε� ����
    /// </summary>
    SceneLoadState[] sceneLoadState;

    [Tooltip("�� �ε�,��ε���� ����Ʈ")]
    List<int> loadedScene = new();  
    List<int> unloadedScene = new();


    MapGenerator Generater;


    public void Start() 
    {
        Generater = GetComponent<MapGenerator>();
        int mapCount = (int)Mathf.Pow(Generater.maximumDepth, 2);
        
        sceneNames = new string[mapCount];
        sceneLoadState = new SceneLoadState[mapCount];
        for(int i = 0; i < mapCount; i++)
        {
            int index = i;                      //�ϴ��� �ߺ� ��������� �ʰ��� �ø��� �ߺ��ȵǰ� ����Ʈ�߰��ؼ� ���ƾ���.
            int number = Random.Range(1, 5);          // <<--�� ��ü���� �˾Ƽ� ���ϴ� �ĵ� ã�ƺ�����
            sceneNames[index] = $"{BaseSceneName}{number}";      //<- �´���Ȯ���ʿ�
            sceneLoadState[index] = SceneLoadState.Unload;
        }
    }

}
