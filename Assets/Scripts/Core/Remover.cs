using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remover : MonoBehaviour
{
     private static List<GameObject> dontDestoryObj = new List<GameObject>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        /*
        ������ �׽�Ʈ��
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ClearAllDontDestroyOnLoadObjects();
        }
        */
    }

    /// <summary>
    /// ������ ������Ʈ ��ŷ ( �迭�� �ֱ� )
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveObjMark(GameObject obj)
    {
        DontDestroyOnLoad(obj);
        dontDestoryObj.Add(obj);
    }

    /// <summary>
    /// ���� ȭ������ �ٽ� �� ���� �ִٸ� or ���� �ʱ�ȭ�� ����
    /// </summary>
    public void ClearAllDontDestroyOnLoadObjects()
    {
        foreach (GameObject obj in dontDestoryObj)
        {
            Destroy(obj);
        }
        dontDestoryObj.Clear();
    }
}
