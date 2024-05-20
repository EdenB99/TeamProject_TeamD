using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererEnemy : EnemyBase_
{
    public GameObject ThunderPrefab;
    readonly int isAttack_Hash = Animator.StringToHash("isAttack");
    Animator animator;

    bool isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    protected override void FixedUpdate()
    {
        spriteDirection();

        if (playerDetected && IsLive) // 플레이어 발견시 행동
        {
            // 플레이어의 위치를 받는다.
            targetPos = player.transform.position;
            if (!IsMove)
            {
                // 플레이어의 위치에 따라 CheckLR 을 변경한다.

                if (targetPos.x < rb.position.x) CheckLR = -1;
                else CheckLR = 1;

            }
            attackAction();
        }

        else if (IsLive) // 플레이어 미발견시 행동
        {

            playerCheck();
            idleAction();
        }

        if (!IsLive) // 죽을시
        {
            fade += Time.deltaTime * 0.5f;
            sprite.material.SetFloat(FadeID, 1 - fade);

            if (fade > 1)
            {
                Destroy(this.gameObject); // 1초후 삭제
            }
        }
    }

    protected override void attackAction()
    {
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        if(playerCheck() && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger(isAttack_Hash);


            yield return new WaitForSeconds(1f);
            Vector3 spawnPosition = new Vector3(targetPos.x, targetPos.y + 4f, targetPos.z);
            GameObject thunderInstance = Instantiate(ThunderPrefab, spawnPosition, Quaternion.identity);
            Destroy(thunderInstance, 2f);

            yield return new WaitForSeconds(3f);
            isAttacking = false;
        }
        
    }
}
