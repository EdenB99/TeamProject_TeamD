using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.transform.position = new Vector3(0, 0,transform.position.z);
        } 
    }
    
}
