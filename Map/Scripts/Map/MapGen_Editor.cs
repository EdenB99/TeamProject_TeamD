using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

//[CustomEditor(typeof(MapGenerator))]
public class MapGen_Editor : Editor
{
    //public MapGenerator selected;

    //// Editor���� OnEnable�� ���� �����Ϳ��� ������Ʈ�� �������� Ȱ��ȭ��
    //private void OnEnable()
    //{
    //    // target�� Editor�� �ִ� ������ ������ ������Ʈ�� �޾ƿ�.
    //    if (AssetDatabase.Contains(target))
    //    {
    //        selected = null;
    //    }
    //    else
    //    {
    //        // target�� Object���̹Ƿ� Enemy�� ����ȯ
    //        selected = (MapGenerator)target;
    //    }
    //}

    //// ����Ƽ�� �ν����͸� GUI�� �׷��ִ��Լ�
    //public override void OnInspectorGUI()
    //{
    //    if (selected == null)
    //        return;

    //    EditorGUILayout.Space();
    //    GUI.color = Color.yellow;
    //    EditorGUILayout.LabelField("���η������ִ°�", EditorStyles.boldLabel);
    //    EditorGUILayout.Space();

    //    GUI.color = Color.white;

    //    selected.map = (GameObject)EditorGUILayout.ObjectField("ù �� ������ üũ��(rectangle)",
    //                                                           selected.map, typeof(GameObject), true);
    //    selected.line = (GameObject)EditorGUILayout.ObjectField("������ ���� üũ��(line)",
    //                                                           selected.line, typeof(GameObject), true);

    //    selected.road = (GameObject)EditorGUILayout.ObjectField("�̾��� ��üũ��(road)",
    //                                                           selected.road, typeof(GameObject), true);

    //    EditorGUILayout.Space();
    //    GUI.color = Color.yellow;
    //    EditorGUILayout.LabelField("�� ����", EditorStyles.boldLabel);
    //    EditorGUILayout.Space();

    //    GUI.color = Color.white;
    //    selected.mapSize = EditorGUILayout.Vector2IntField("                      �� ũ��(X,Y)", selected.mapSize);
    //    selected.minimumDevidedRate = EditorGUILayout.FloatField("������   �ּ� ����", selected.minimumDevidedRate);
    //    selected.maximumDevidedRate = EditorGUILayout.FloatField("��������   �ִ� ����", selected.maximumDevidedRate);
    //    selected.maximumDepth = EditorGUILayout.IntField("Ʈ���� ����(2^n)", selected.maximumDepth);


    //    // �ٽ� �׸���
    //    if (GUILayout.Button("�ٽñ׸���"))
    //    {
    //        selected.ReDrow();
    //    }

    //}
}
