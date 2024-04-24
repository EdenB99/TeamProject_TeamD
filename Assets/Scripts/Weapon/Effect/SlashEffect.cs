using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SlashEffect : WeaponEffect
{
    BoxCollider2D slashCollider;
    Animator animator;

    protected override void Awake()
    {
        base.Awake();
        slashCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    //protected IEnumerator DeactivateEffectAfterAnimation()
    //{
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);      // ���� ������� �ִϸ��̼� ���� ��������

    //    float currentClipLength = stateInfo.length;         // �ִϸ��̼��� ��� ���̸� ��������
    //    Debug.Log($"{currentClipLength}");

    //    yield return new WaitForSeconds(currentClipLength);

    //    isDestroyed = true;
    //    Destroy(this.gameObject);
    //}
}
