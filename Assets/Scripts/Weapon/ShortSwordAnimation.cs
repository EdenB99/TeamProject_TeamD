using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortSwordAnimation : StateMachineBehaviour
{
    Animator animator;
    public WeaponCode currentWeaponCode;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
    }

    // 웨폰 코드를 설정하고 애니메이션을 변경
    public void SetWeaponCode(WeaponCode weaponCode)
    {
        currentWeaponCode = weaponCode;
        UpdateAnimator();
    }

    // 애니메이터에 웨폰 코드에 해당하는 매개변수 값을 전달
    private void UpdateAnimator()
    {
        if (animator != null)
        {
            animator.SetInteger("WeaponCode", (int)currentWeaponCode);
        }
    }
}
