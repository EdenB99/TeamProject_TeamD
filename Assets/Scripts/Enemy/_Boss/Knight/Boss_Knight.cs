using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss_Knight : PatternEnemyBase
{
    readonly int move_temp = Animator.StringToHash("Move");

    private float TimeElapsed = 0;

    public float PatternTime = 0;

    public float speed = 5.0f;

    Vector2 moveDir;

    public GameObject obj;

    public GameObject shadow;

    Coroutine mapCoroutine;

    protected override void Start()
    {
        base.Start();
        State = BossState.Chase;


        mapCoroutine = StartCoroutine(Map_Pattern());
    }


    // ���¿� ���� ���� �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    protected override void State_Wait()
    {
        animator.SetInteger(move_temp, 0);
    }

    protected override void Update_Wait()
    {
        // ����
        if (Mathf.Abs(transform.position.y - playerPos.y) < 3.0 && Mathf.Abs(transform.position.x - playerPos.x) > 0.2 )
        {
            State = BossState.Chase;
        }

        // ���� �ð� ���� ( �̽ð��� �����ϸ� ���� ���� )
        if (TimeElapsed < PatternTime)
        {
            TimeElapsed += Time.deltaTime;
        }
        else
        {
            State = BossState.SpAttack;
            animator.SetInteger(move_temp, 0);
            TimeElapsed = 0;
        }
    }


    protected override void State_Chase()
    {
        animator.SetInteger(move_temp, 1);
    }

    protected override void Update_Chase()
    {
        // �Ÿ��� �����Ÿ� ���϶�� �̵�x
        if ( Mathf.Abs( transform.position.x - playerPos.x ) > 0.2)
        {
            // �������� �÷��̾�� �ٰ���
            moveDir = new Vector2(CheckLR, 0);
            transform.Translate(Time.fixedDeltaTime * speed * -moveDir);

        } // �÷��̾ ������ �� ���� ��ġ��� ���
        else if ( (Mathf.Abs(transform.position.y - playerPos.y) > 3.0 ) )
        {
            State = BossState.Wait;
        }

        // �÷��̾�� �����Ÿ� �����ϸ� ����
        if ( playerCheck() )
        {
            State = BossState.SpAttack;
            TimeElapsed = 0;
        }    

        // ���� �ð� ���� ( �̽ð��� �����ϸ� ���� ���� )
        if (TimeElapsed < PatternTime )
        {
            TimeElapsed += Time.deltaTime;
        }
        else // ���� �ð� �Ǹ� ����
        {
            State = BossState.SpAttack;
            
            TimeElapsed = 0;
        }
        

    }

    protected override void State_SpAttack()
    {
        animator.SetInteger(move_temp, 0);
        StartCoroutine(bossActionSelect());
    }

    // ���� �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�

    protected override void InitializePatterns()
    {
        patternActions = new Dictionary<uint, Func<IEnumerator>>()
    {
            { 1, BossPattern_1 },
            { 2, BossPattern_2 },
            { 3, BossPattern_3 },
            { 4, BossPattern_4 }

        // �ٸ� ���ϵ鵵 �̿� ���� �ʱ�ȭ
    };
    }

    /// <summary>
    /// ���� 1 : ������ �÷� ����
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator BossPattern_1()
    {
        animator.SetTrigger("Attack_1");
        yield return new WaitForSeconds(1.0f);
        Vector2 pos = transform.position;
        pos.y -= 1;

        // �ӽ��ڵ� , ���丮 ���� ������
        for (int i = 0; i < 8; i++)
        {
            GameObject line = Instantiate(obj, pos, Quaternion.identity) ;
            BladeLine lineScript = line.GetComponent<BladeLine>();
            lineScript.dir = i * 5 * -CheckLR;
        }

        yield return new WaitForSeconds(3.5f);
        State = BossState.Chase;
    }

    /// <summary>
    /// ���� 2 : �������
    /// </summary>
    /// <returns></returns>
    IEnumerator BossPattern_2()
    {
        animator.SetTrigger("Attack_2");
        float temp = Random.Range(75, 105);
        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Attack_3");
        yield return new WaitForSeconds(0.1f);


        // �ӽ��ڵ� , ���丮 ���� ������
        for ( int i = 0; i < 7 ; i++ )
        {
            GameObject line = Instantiate(obj, new Vector2(transform.position.x + i * 1f, 0), Quaternion.identity);
            BladeLine lineScript = line.GetComponent<BladeLine>();
            GameObject line2 = Instantiate(obj, new Vector2(transform.position.x + i * -1f, 0), Quaternion.identity);
            BladeLine lineScript2 = line2.GetComponent<BladeLine>();
            lineScript.dir = temp;
            lineScript2.dir = temp;
        }


        
        yield return new WaitForSeconds(2.0f);
        State = BossState.Chase;
    }

    /// <summary>
    /// ���� 3 : �ܰ� �������
    /// </summary>
    /// <returns></returns>
    IEnumerator BossPattern_3()
    {
        animator.SetTrigger("Attack_2");
        float temp = Random.Range(75, 105);
        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Attack_3");
        yield return new WaitForSeconds(0.1f);


        // �ӽ��ڵ� , ���丮 ���� ������
        for (int i = 0; i < 8; i++)
        {
            GameObject line = Instantiate(obj, new Vector2(transform.position.x + i * 1f + 3.0f, 0.8f), Quaternion.identity);
            BladeLine lineScript = line.GetComponent<BladeLine>();
            GameObject line2 = Instantiate(obj, new Vector2(transform.position.x + i * -1f - 3.0f, 0.8f), Quaternion.identity);
            BladeLine lineScript2 = line2.GetComponent<BladeLine>();
            lineScript.dir = temp;
            lineScript2.dir = temp;
        }



        yield return new WaitForSeconds(2.0f);
        State = BossState.Chase;
    }

    /// <summary>
    /// ���� 4 : �н� ����
    /// </summary>
    /// <returns></returns>
    IEnumerator BossPattern_4()
    {
        StopCoroutine(mapCoroutine);
        animator.SetTrigger("Attack_2");
        yield return new WaitForSeconds(1.5f);


        // �ӽ��ڵ� , ���丮 ���� ������
        for (int i = 0; i < 7; i++)
        {
            Instantiate(shadow);
            yield return new WaitForSeconds(0.8f);
        }
        yield return new WaitForSeconds(0.6f);
        animator.SetTrigger("Attack_3");
        GameObject line = Instantiate(obj, new Vector2(player.transform.position.x, player.transform.position.y+0.8f), Quaternion.identity);
        BladeLine lineScript = line.GetComponent<BladeLine>();
        lineScript.dir = 45;
        GameObject line2 = Instantiate(obj, new Vector2(player.transform.position.x, player.transform.position.y + 0.8f), Quaternion.identity);
        BladeLine lineScript2 = line2.GetComponent<BladeLine>();
        lineScript2.dir = 135;


        yield return new WaitForSeconds(2.0f);
        State = BossState.Chase;
        mapCoroutine = StartCoroutine(Map_Pattern());
    }

    // ������ ũ������Ʈ �ѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤѤ�



    IEnumerator Map_Pattern()
    {
        while ( true)
        {
            Debug.Log("����");
            yield return new WaitForSeconds(7.0f);
            Instantiate(shadow);
        }
    }

}