using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTrap : MonoBehaviour
{


    [SerializeField] private float fireDelay = 0f;
    [SerializeField] private float fireInterval = 2f;

    public BulletCode bullet;



    [SerializeField] private Direction fireDir = Direction.Down;
    private Vector2 fireVector = Vector2.zero;


    private void OnEnable()
    {
        SelectDir();
        StartCoroutine(Fire());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    private void SelectDir()
    {
        switch (fireDir)
        {
            case Direction.Up:
                fireVector = Vector2.up;
                break;
            case Direction.Down:
                fireVector = Vector2.down;
                break;
            case Direction.Left:
                fireVector = Vector2.left;
                break;
            case Direction.Right:
                fireVector = Vector2.right;
                break;
            default:
                fireVector = Vector2.down;
                break;
        }
    }

    
    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(fireDelay);
        while (true)
        {
            Factory.Instance.MakeBullet(transform.position, fireVector, bullet);
            yield return new WaitForSeconds(fireInterval);
        }
    }
}