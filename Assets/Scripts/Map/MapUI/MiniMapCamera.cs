using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    Player player;

    private void Start()
    {
    player = GameObject.FindAnyObjectByType<Player>();

    }

    private void Update()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y,transform.position.z);
    }
}
