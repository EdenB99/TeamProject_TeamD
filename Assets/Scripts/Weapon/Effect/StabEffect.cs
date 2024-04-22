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

    private void OnEnable()
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("StabAttack");
        StartCoroutine(DeactivateEffectAfterAnimation(weaponEffect));
    }

    private IEnumerator DeactivateEffectAfterAnimation(GameObject weaponEffect)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);      // ���� ������� �ִϸ��̼� ���� ��������

        float currentClipLength = stateInfo.length;         // �ִϸ��̼��� ��� ���̸� ��������


        yield return new WaitForSeconds(currentClipLength);

        isDestroyed = true;
        Destroy(this.gameObject);
        Debug.Log("����Ʈ �ı�");
    }
}
