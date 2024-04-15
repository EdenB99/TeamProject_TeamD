using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeLine : MonoBehaviour, IAttack
{
    public float dir;
    private float ScaleY;
    public uint Damage = 20;
    
    Animator animator;

    Boss_Knight knight;

    public uint AttackPower => Damage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        knight = FindAnyObjectByType<Boss_Knight>();
    }

    private void OnEnable()
    {
        gameObject.layer = LayerMask.NameToLayer("Effect");
        transform.rotation = Quaternion.Euler(0, 0, dir);
        ScaleY = 3.00f;
        StartCoroutine(shotting());
    }


    IEnumerator shotting()
    {
        
        yield return new WaitForSeconds(1.2f);
        ScaleY = 1.50f;
        yield return new WaitForSeconds(0.1f);
        gameObject.layer = LayerMask.NameToLayer("Enemy_Attack");
        animator.SetTrigger("shot");
        yield return new WaitForSeconds(0.2f);
        knight.ReturnToPool(this.gameObject);
    }

    
    void Update()
    {
        if ( ScaleY > 0.2 ) ScaleY -= 0.02f;
        transform.localScale = new Vector3(1.5f, ScaleY, 1);
    }
}
