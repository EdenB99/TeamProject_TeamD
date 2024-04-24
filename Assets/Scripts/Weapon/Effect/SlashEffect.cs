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
    //    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);      // 현재 재생중인 애니메이션 정보 가져오기

    //    float currentClipLength = stateInfo.length;         // 애니메이션의 재생 길이를 가져오기
    //    Debug.Log($"{currentClipLength}");

    //    yield return new WaitForSeconds(currentClipLength);

    //    isDestroyed = true;
    //    Destroy(this.gameObject);
    //}
}
