using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Town : MonoBehaviour
{



    private void OnEnable()
    {
        Player player = FindAnyObjectByType<Player>();
        MainCamera mainCamera = FindAnyObjectByType<MainCamera>();
        BackgroundFollow background = FindAnyObjectByType<BackgroundFollow>();
        player.transform.position = new Vector3(-9.57425f, -5.144983f, 0);
        mainCamera.transform.position = new Vector3(player.transform.position.x, 1, mainCamera.transform.position.z);
        background.transform.position = new Vector3(player.transform.position.x, 1, background.transform.position.z);

    }

}
