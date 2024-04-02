using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour where T : RecycleObject
{
    /// <summary>
    /// 풀에서 관리할 오브젝트 프리팹
    /// </summary>
    public GameObject originalPrefab;

    /// <summary>
    /// 풀의 크기
    /// </summary>
    public int poolSize = 64;

    /// <summary>
    /// T로 지정된 모든 오브젝트가 있는 배열
    /// </summary>
    T[] pool;

    /// <summary>
    /// 비활성화 상태인 오브젝트의 큐 
    /// </summary>
    Queue<T> readyQueue;

    public void Initialize()
    {
        if( pool == null )  // 풀이 아직 만들어지지 않은 경우
        {
            pool = new T[poolSize];                 // 배열의 크기만큼 new
            readyQueue = new Queue<T>(poolSize);    // 레디큐를 만들고 capacity를 poolSize로 지정

            GenerateObjects(0, poolSize, pool);
        }
        else
        {
            // 풀이 이미 만들어져 있는 경우(ex:씬이 추가로 로딩 or 씬이 다시 시작)
            foreach( T obj in pool )    // foreach : 특정 컬랙션 안에 있는 모든 요소를 한번씩 처리해야 할 일이 있을 때 사용
            {
                obj.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 풀에서 사용하지 않는 오브젝트를 하나 꺼낸 후 리턴 하는 함수
    /// </summary>
    /// <param name="position">배치될 위치(월드좌표)</param>
    /// <param name="eulerAngle">배치될 때의 각도</param>
    /// <returns>풀에서 꺼낸 오브젝트(활성화됨)</returns>
    public T GetObject(Vector3? position = null, Vector3? eulerAngle = null)
    {
        if (readyQueue.Count > 0)          // 레디큐에 오브젝트가 남아있는지 확인
        {
            T comp = readyQueue.Dequeue();  // 남아있으면 하나 꺼내고
            comp.transform.position = position.GetValueOrDefault(); // 지정된 위치로 이동
            comp.transform.rotation = Quaternion.Euler(eulerAngle.GetValueOrDefault());  // 지정된 각도로 회전
            comp.gameObject.SetActive(true);// 활성화 시키고
            OnGetObject(comp);              // 오브젝트별 추가 처리
            return comp;                    // 리턴
        }
        else
        {
            // 레디큐가 비어있다 == 남아있는 오브젝트가 없다
            ExpandPool();                           // 풀을 두배로 확장한다.
            return GetObject(position, eulerAngle); // 새로 하나 꺼낸다.
        }
    }

    /// <summary>
    /// 특정 오브젝트에 해야할 일 
    /// </summary>
    protected virtual void OnGetObject(T component)
    {
    }

    /// <summary>
    /// 풀을 두배로 확장시키는 함수
    /// </summary>
    void ExpandPool()
    {
        // 경고 표시
        Debug.LogWarning($"{gameObject.name} 풀 사이즈 증가. {poolSize} -> {poolSize * 2}");

        int newSize = poolSize * 2;         // 풀 크기를 2배로 늘림
        T[] newPool = new T[newSize];       // 새롭게 풀을 만듬 
        for(int i = 0; i<poolSize; i++)     // 풀 > 새로운 풀 
        {
            newPool[i] = pool[i];
        }
        GenerateObjects(poolSize, newSize, newPool);    // 새 풀의 남은 부분에 오브젝트 생성해서 추가
        
        pool = newPool;         // 새 풀 사이즈 설정
        poolSize = newSize;     // 새 풀을 풀로 설정
    }


    /// <summary>
    /// 풀에서 사용할 오브젝트를 생성하는 함수
    /// </summary>
    /// <param name="start">새로 생성 시작할 인덱스</param>
    /// <param name="end">새로 생성이 끝나는 인덱스+1</param>
    /// <param name="results">생성된 오브젝트가 들어갈 배열</param>
    void GenerateObjects(int start, int end, T[] results)
    {
        for (int i = start; i < end; i++)
        {
            GameObject obj = Instantiate(originalPrefab, transform);    // 프리팹 생성
            obj.name = $"{originalPrefab.name}_{i}";    // 이름 변경

            T comp = obj.GetComponent<T>();
            comp.onDisable += () => readyQueue.Enqueue(comp);   // 재활용 오브젝트가 비활성화 되면 레디큐로 되돌려라

            OnGenerateObject(comp);

            results[i] = comp;      // 배열에 저장
            obj.SetActive(false);   // 비활성화 
        }
    }

    /// <summary>
    /// 각 T타입별로 생성 직후에 필요한 추가 작업을 처리하는 함수
    /// </summary>
    /// <param name="comp">T타입의 컴포넌트</param>
    protected virtual void OnGenerateObject(T comp)
    {
    }
}
