using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationEvent : MonoBehaviour
{
    Animator animator;
    BoxCollider2D boxCollider2D;

    private void Start()
    {
        boxCollider2D = transform.parent.GetComponent<BoxCollider2D>();
    }

    // 애니메이션 이벤트: 슬래시 어택
    public void SlashAttack()
    {
        // 무기를 -150도 회전
        transform.Rotate(Vector3.forward, -150f);
        boxCollider2D.enabled = true;
        boxCollider2D.isTrigger = true;
    }


    public void SlashAttack_Upper()
    {
        transform.Rotate(Vector3.forward, 150f);
        boxCollider2D.enabled = true;
        boxCollider2D.isTrigger = true;
    }

    public void StabAttack_Forward()
    {
        transform.Translate(transform.right * 0.3f);
        boxCollider2D.enabled = true;
        boxCollider2D.isTrigger = true;
    }

    public void StabAttack_Backward()
    {
        transform.Translate(transform.right * -0.3f);
        boxCollider2D.enabled = true;
        boxCollider2D.isTrigger = true;
    }

}
