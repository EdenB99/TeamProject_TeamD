using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWeapon : MonoBehaviour, IWeapon
{
    /// <summary>
    /// °ø°Ý·Â
    /// </summary>
    public float attackDamage = 10.0f;


    private void Start()
    {
        Transform hinge = GameObject.Find("Player/hinge").transform;


        transform.SetParent(hinge);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void WeaponAttack()
    {
        
    }
}
