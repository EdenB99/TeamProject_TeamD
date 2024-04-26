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


        StartCoroutine(AwakeAction());                      // 개전 시작 

        GenerateObj();

        mapCoroutine = StartCoroutine(Map_Pattern());
    }
    

    // 상태에 따라 할일 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    protected override void State_Wait()
    {
        animator.SetInteger(move_temp, 0);
    }

    protected override void Update_Wait()
    {
        // 방향
        if (Mathf.Abs(transform.position.y - playerPos.y) < 3.0 && Mathf.Abs(transform.position.x - playerPos.x) > 0.2 )
        {
            State = BossState.Chase;
        }

        // 패턴 시간 증가 ( 이시간이 증가하면 상태 변경 )
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
        // 거리가 일정거리 이하라면 이동x
        if ( Mathf.Abs( transform.position.x - playerPos.x ) > 0.2)
        {
            // 방향으로 플레이어에게 다가감
            moveDir = new Vector2(CheckLR, 0);
            transform.Translate(Time.fixedDeltaTime * speed * -moveDir);

        } // 플레이어가 공격할 수 없는 위치라면 대기
        else if ( (Mathf.Abs(transform.position.y - playerPos.y) > 3.0 ) )
        {
            State = BossState.Wait;
        }

        // 플레이어와 일정거리 근접하면 패턴
        if ( playerCheck() )
        {
            State = BossState.SpAttack;
            TimeElapsed = 0;
        }    

        // 패턴 시간 증가 ( 이시간이 증가하면 상태 변경 )
        if (TimeElapsed < PatternTime )
        {
            TimeElapsed += Time.deltaTime;
        }
        else // 패턴 시간 되면 패턴
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

    // 패턴 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    protected override void InitializePatterns()
    {
        patternActions = new Dictionary<uint, Func<IEnumerator>>()
    {
            { 1, BossPattern_1 },
            { 2, BossPattern_2 },
            { 3, BossPattern_3 },
            { 4, BossPattern_4 }

        // 다른 패턴들도 이와 같이 초기화
    };
    }

    /// <summary>
    /// 패턴 1 : 땅에서 올려 베기
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator BossPattern_1()
    {
        animator.SetTrigger("Attack_1");
        yield return new WaitForSeconds(1.0f);
        Vector2 pos = transform.position;
        pos.y -= 1;

        for (int i = 0; i < 8; i++)
        {

            MakeBlade(pos, i * 5 * -CheckLR);
        }

        yield return new WaitForSeconds(3.5f);
        State = BossState.Chase;
    }

    /// <summary>
    /// 패턴 2 : 내려찍기
    /// </summary>
    /// <returns></returns>
    IEnumerator BossPattern_2()
    {
        animator.SetTrigger("Attack_2");
        float temp = Random.Range(75, 105);
        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Attack_3");
        yield return new WaitForSeconds(0.1f);


        // 임시코드 , 팩토리 쓸지 고민중
        for ( int i = 0; i < 7 ; i++ )
        {
            MakeBlade(new Vector2(transform.position.x + i * 1f, 0), temp);
        }


        
        yield return new WaitForSeconds(2.0f);
        State = BossState.Chase;
    }

    /// <summary>
    /// 패턴 3 : 외곽 내려찍기
    /// </summary>
    /// <returns></returns>
    IEnumerator BossPattern_3()
    {
        animator.SetTrigger("Attack_2");
        float temp = Random.Range(75, 105);
        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Attack_3");
        yield return new WaitForSeconds(0.1f);


        // 임시코드 , 팩토리 쓸지 고민중
        for (int i = 0; i < 8; i++)
        {
            MakeBlade(new Vector2(transform.position.x + i * 1f + 3.0f, 0.8f), temp);
            MakeBlade(new Vector2(transform.position.x + i * -1f - 3.0f, 0.8f), temp);
        }
        yield return new WaitForSeconds(2.0f);
        State = BossState.Chase;
    }

    /// <summary>
    /// 패턴 4 : 분신 베기
    /// </summary>
    /// <returns></returns>
    IEnumerator BossPattern_4()
    {
        StopCoroutine(mapCoroutine);
        animator.SetTrigger("Attack_2");
        yield return new WaitForSeconds(1.5f);


        // 임시코드 , 팩토리 쓸지 고민중
        for (int i = 0; i < 7; i++)
        {
            Instantiate(shadow);
            yield return new WaitForSeconds(0.8f);
        }
        yield return new WaitForSeconds(0.6f);
        animator.SetTrigger("Attack_3");
        MakeBlade(new Vector2(player.transform.position.x, player.transform.position.y + 0.8f), 45);
        MakeBlade(new Vector2(player.transform.position.x, player.transform.position.y + 0.8f), 135);

        yield return new WaitForSeconds(2.0f);
        State = BossState.Chase;
        mapCoroutine = StartCoroutine(Map_Pattern());
    }

    // 칼날 미니 팩토리

    public GameObject bladeObj;
    private Queue<GameObject> pool = new Queue<GameObject>();
    public int poolSize = 32;

    public void GenerateObj()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bladeObj, transform.position, Quaternion.identity);
            
            obj.name = $"{bladeObj.name}_{i}";    // 이름 변경
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject MakeBlade(Vector2 pos, float direction)
    {
        if (pool.Count == 0)
        {
            // 풀에 오브젝트가 없다면 그냥 생성하지 않음.
            return null;
        }
 

        GameObject obj = pool.Dequeue();
        obj.transform.position = pos;

        BladeLine bladeScript = obj.GetComponent<BladeLine>();
        bladeScript.dir = direction;
       

        obj.SetActive(true);
        return obj;
    }

    

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    void OnDestroy()
    {
        // A 오브젝트가 파괴될 때 풀 정리
        while (pool.Count > 0)
        {
            Destroy(pool.Dequeue());
        }
    }



    // 개전 행동 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ

    protected override IEnumerator AwakeAction()
    {
        // 여러 모션을 취하거나 , 행동을 취한뒤에

        yield return new WaitForSeconds(0);

        yield return new WaitForSeconds(0);

        yield return new WaitForSeconds(0);
        State = BossState.Chase;

    }








    // 쉐도우 크리에이트 ㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡㅡ









    IEnumerator Map_Pattern()
    {
        while ( true)
        {
            yield return new WaitForSeconds(7.0f);
            Instantiate(shadow);
        }
    }

}
