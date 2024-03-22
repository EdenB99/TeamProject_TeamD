using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

//[CustomEditor(typeof(MapGenerator))]
public class MapGen_Editor : Editor
{
    //public MapGenerator selected;

    //// Editor에서 OnEnable은 실제 에디터에서 오브젝트를 눌렀을때 활성화됨
    //private void OnEnable()
    //{
    //    // target은 Editor에 있는 변수로 선택한 오브젝트를 받아옴.
    //    if (AssetDatabase.Contains(target))
    //    {
    //        selected = null;
    //    }
    //    else
    //    {
    //        // target은 Object형이므로 Enemy로 형변환
    //        selected = (MapGenerator)target;
    //    }
    //}

    //// 유니티가 인스펙터를 GUI로 그려주는함수
    //public override void OnInspectorGUI()
    //{
    //    if (selected == null)
    //        return;

    //    EditorGUILayout.Space();
    //    GUI.color = Color.yellow;
    //    EditorGUILayout.LabelField("라인렌더러넣는곳", EditorStyles.boldLabel);
    //    EditorGUILayout.Space();

    //    GUI.color = Color.white;

    //    selected.map = (GameObject)EditorGUILayout.ObjectField("첫 맵 사이즈 체크용(rectangle)",
    //                                                           selected.map, typeof(GameObject), true);
    //    selected.line = (GameObject)EditorGUILayout.ObjectField("나눠진 공간 체크용(line)",
    //                                                           selected.line, typeof(GameObject), true);

    //    selected.road = (GameObject)EditorGUILayout.ObjectField("이어진 길체크용(road)",
    //                                                           selected.road, typeof(GameObject), true);

    //    EditorGUILayout.Space();
    //    GUI.color = Color.yellow;
    //    EditorGUILayout.LabelField("맵 정보", EditorStyles.boldLabel);
    //    EditorGUILayout.Space();

    //    GUI.color = Color.white;
    //    selected.mapSize = EditorGUILayout.Vector2IntField("                      맵 크기(X,Y)", selected.mapSize);
    //    selected.minimumDevidedRate = EditorGUILayout.FloatField("공간이   최소 비율", selected.minimumDevidedRate);
    //    selected.maximumDevidedRate = EditorGUILayout.FloatField("나눠지는   최대 비율", selected.maximumDevidedRate);
    //    selected.maximumDepth = EditorGUILayout.IntField("트리의 높이(2^n)", selected.maximumDepth);


    //    // 다시 그리기
    //    if (GUILayout.Button("다시그리기"))
    //    {
    //        selected.ReDrow();
    //    }

    //}
}
