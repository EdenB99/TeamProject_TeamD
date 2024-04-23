using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStageScripts : MonoBehaviour
{
    CapsuleCollider2D collider;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        collider = child.GetComponent<CapsuleCollider2D>();
    
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collider.CompareTag("Player"))
        {
            Debug.Log("ºÎ‹HÈû");
        }
    }

}

