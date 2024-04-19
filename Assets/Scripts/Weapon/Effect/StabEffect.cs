using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabEffect : WeaponEffect
{
    Animator animator;
    BoxCollider2D stabCollider;

    protected override void Awake()
    {
        base.Awake();
        stabCollider = GetComponent<BoxCollider2D>();
    }
    protected override void Start()
    {
        base.Start();
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
