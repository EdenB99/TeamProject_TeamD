using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabEffect : WeaponEffect
{
    Animator animator;
    BoxCollider2D stabCollider;
    GameObject weaponEffect;

    protected override void Awake()
    {
        base.Awake();
        stabCollider = GetComponent<BoxCollider2D>();
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    //protected IEnumerator DeactivateEffectAfterAnimation(GameObject weaponEffect)
    //{
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);      // ���� ������� �ִϸ��̼� ���� ��������

    //    float currentClipLength = stateInfo.length;         // �ִϸ��̼��� ��� ���̸� ��������
    //    Debug.Log($"{currentClipLength}");

    //    yield return new WaitForSeconds(currentClipLength);

    //    isDestroyed = true;
    //    Destroy(this.gameObject);
    //}
}
