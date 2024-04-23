using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SlashEffect : WeaponEffect
{
    BoxCollider2D slashCollider;
    Animator animator;
    GameObject weaponEffect;

    protected override void Awake()
    {
        base.Awake();
        slashCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("SlashAttack");
        Debug.Log("Ʈ���� �ߵ�");
        StartCoroutine(DeactivateEffectAfterAnimation(weaponEffect));
        Debug.Log("�ڷ�ƾ ����");
    }

    protected override IEnumerator DeactivateEffectAfterAnimation(GameObject weaponEffect)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);      // ���� ������� �ִϸ��̼� ���� ��������

        float currentClipLength = stateInfo.length;         // �ִϸ��̼��� ��� ���̸� ��������

        yield return new WaitForSeconds(currentClipLength);

        isDestroyed = true;
        Destroy(this.gameObject);
        Debug.Log("����Ʈ �ı�");
    }
}
