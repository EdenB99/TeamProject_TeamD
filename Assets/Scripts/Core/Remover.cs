using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remover : MonoBehaviour
{
     private static List<GameObject> dontDestoryObj = new List<GameObject>();

    /// <summary>
    /// 제거할 오브젝트 마킹 ( 배열에 넣기 )
    /// </summary>
    /// <param name="obj"></param>
    public static void RemoveObjMark(GameObject obj)
    {
        DontDestroyOnLoad(obj);
        dontDestoryObj.Add(obj);
    }

    /// <summary>
    /// 메인 화면으로 다시 올 일이 있다면 or 완전 초기화시 실행
    /// </summary>
    public static void ClearAllDontDestroyOnLoadObjects()
    {
        foreach (GameObject obj in dontDestoryObj)
        {
            Destroy(obj);
        }
        
        
    }
}
