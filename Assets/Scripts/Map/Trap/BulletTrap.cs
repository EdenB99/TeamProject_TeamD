using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTrap : MonoBehaviour
{


    [SerializeField] private float fireDelay = 0f;
    [SerializeField] private float fireInterval = 2f;

    public BulletCode bullet;

    [SerializeField] private float fireDir = 0; 


    private void OnEnable()
    {
        StartCoroutine(Fire());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    
    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireDelay);
        while (true)
        {
            Factory.Instance.MakeBullet(transform.position, fireDir, bullet);


            yield return new WaitForSeconds(fireInterval);
        }
    }
}