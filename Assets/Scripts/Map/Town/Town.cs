using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{



    private void OnEnable()
    {
        Player player = FindAnyObjectByType<Player>();
        MainCamera mainCamera = FindAnyObjectByType<MainCamera>();

        mainCamera.transform.position = new Vector3(0, 0, mainCamera.transform.position.z);
        player.transform.position = new Vector3(-9.57425f, -5.144983f, 0);

    }

}
