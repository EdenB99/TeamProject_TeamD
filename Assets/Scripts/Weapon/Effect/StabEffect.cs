using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabEffect : WeaponEffect
{
    Animator animator;

    BoxCollider2D boxCollider2D;

    public new GameObject weapon;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        // StartCoroutine(DeactivateEffectAfterAnimation(weaponEffect));
    }

    //private IEnumerator DeactivateEffectAfterAnimation(GameObject weaponEffect)
    //{
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);      // ���� ������� �ִϸ��̼� ���� ��������

    //    float currentClipLength = stateInfo.length;         // �ִϸ��̼��� ��� ���̸� ��������


    //    yield return new WaitForSeconds(currentClipLength);
    //    Destroy(weaponEffect);
    //    Debug.Log($"{currentClipLength}");
    //}
}
