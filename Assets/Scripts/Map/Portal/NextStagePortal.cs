
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStagePortal : MonoBehaviour
{
    [SerializeField] private GameObject nextStageMap;
    private GameObject text;
    public bool isInsideTrigger;
    Player player;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        text = child.gameObject;
        text.SetActive(false);
        SceneManager.sceneLoaded += OnSceneLoaded;
        player = GameManager.Instance.Player;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(true); // 텍스트 메시 활성화
            isInsideTrigger = true; // 트리거 영역 내부로 설정
            player.interactingPortal = this; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(false); // 텍스트 메시 비활성화
            isInsideTrigger = false; // 트리거 영역 외부로 설정
            player.interactingPortal = null; 
        }
    }

    public void LoadNextStage()
    {
        SceneManager.LoadScene(nextStageMap.name);

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새로운 씬에서 "PlayerLocation" 태그를 가진 오브젝트 찾아, 있으면 그곳으로 이동.
        GameObject playerLocation = GameObject.FindWithTag("PlayerLocation");
        if (playerLocation != null && player != null)
        {
            player.transform.position = playerLocation.transform.position;
        }

    }
}





