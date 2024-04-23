
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStagePortal : MonoBehaviour
{
    [SerializeField] private GameObject nextStageMap;

    
    
    private GameObject text; 
    private bool isInsideTrigger = false;
    private GameObject player; 

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        text = child.gameObject;
        text.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(true); // 텍스트 메시 활성화
            isInsideTrigger = true; // 트리거 영역 내부로 설정
            player = other.gameObject; // 플레이어 오브젝트 저장
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(false); // 텍스트 메시 비활성화
            isInsideTrigger = false; // 트리거 영역 외부로 설정
        }
    }

    private void Update()
    {
        if (isInsideTrigger && Input.GetKeyDown(KeyCode.F))
        {
            LoadNextStage();
        }
    }

    private void LoadNextStage()
    {
        SceneManager.LoadScene(nextStageMap.name);

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새로운 씬에서 "PlayerLocation" 태그를 가진 오브젝트 찾기
        GameObject playerLocation = GameObject.FindWithTag("PlayerLocation");
        if (playerLocation != null && player != null)
        {
            player.transform.position = playerLocation.transform.position;
        }
        else
        {
            Debug.LogWarning("'PlayerLocation' 태그를 가진 게임 오브젝트나 플레이어 오브젝트를 찾을 수 없습니다.");
        }
    }
}





