using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstSceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //TODO:: �ȵǸ� Town�� ��ũ��Ʈ�� �־� ������ ��ġ�ʱ�ȭ�ѹ��غ���.
        BackgroundFollow background = FindAnyObjectByType<BackgroundFollow>();
        MainCamera mainCamera = GetComponent<MainCamera>();

        mainCamera.transform.position = new Vector3(0, 0, mainCamera.transform.position.z);
        background.transform.position = new Vector3(0, 0, background.transform.position.z);
        SceneManager.LoadScene("Town");
    }


}
