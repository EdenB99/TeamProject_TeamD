using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remover : MonoBehaviour
{
     private static List<GameObject> dontDestoryObj = new List<GameObject>();

    /// <summary>
    /// ������ ������Ʈ ��ŷ ( �迭�� �ֱ� )
    /// </summary>
    /// <param name="obj"></param>
    public static void RemoveObjMark(GameObject obj)
    {
        DontDestroyOnLoad(obj);
        dontDestoryObj.Add(obj);
    }

    /// <summary>
    /// ���� ȭ������ �ٽ� �� ���� �ִٸ� or ���� �ʱ�ȭ�� ����
    /// </summary>
    public static void ClearAllDontDestroyOnLoadObjects()
    {
        foreach (GameObject obj in dontDestoryObj)
        {
            Destroy(obj);
        }
        
        
    }
}
