using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : EnemyBase_
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
        if (playerDetected)
        {
            targetPos = player.transform.position;
            if (IsMove)
            {
                if (targetPos.x < rb.position.x) CheckLR = -1;
                else CheckLR = 1;
            }
        }
        attackAction();
    }

    bool playerCheck()
    {
        // 범위 내에
        Collider2D colliders = Physics2D.OverlapCircle(transform.position, sightRange, LayerMask.GetMask("Player"));


        // 플레이어가 있다면
        if (colliders != null)
        {
            if (!playerDetected)
            {
                playerDetected = true;
                firstAction();
            }

            return true;
        }
        return false;
    }

    protected override void attackAction()
    {
        //  플레이어가 감지되었는지 확인
        if (playerCheck() && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());


        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        animator.SetTrigger(isAttack_Hash);


        yield return new WaitForSeconds(1f);
        Vector3 spawnPosition = new Vector3(targetPos.x, targetPos.y + 5f, targetPos.z);
        GameObject thunderInstance = Instantiate(ThunderPrefab, spawnPosition, Quaternion.identity);
        Destroy(thunderInstance, 2f);

        yield return new WaitForSeconds(3f);
        isAttacking = false;
    }
}
