using System.Collections;
using System.Collections.Generic;
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

    void Update()
    {

    }

    private IEnumerator DeactivateEffectAfterAnimation(GameObject weaponEffect)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);      // ���� ������� �ִϸ��̼� ���� ��������

        float currentClipLength = stateInfo.length;         // �ִϸ��̼��� ��� ���̸� ��������


        yield return new WaitForSeconds(currentClipLength);
        Destroy(weaponEffect);
        Debug.Log("����Ʈ �ı�");
    }
}
