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
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);      // 현재 재생중인 애니메이션 정보 가져오기

        float currentClipLength = stateInfo.length;         // 애니메이션의 재생 길이를 가져오기


        yield return new WaitForSeconds(currentClipLength);
        Destroy(weaponEffect);
        Debug.Log("이펙트 파괴");
    }
}
