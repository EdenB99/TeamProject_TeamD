
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
            text.gameObject.SetActive(true); // �ؽ�Ʈ �޽� Ȱ��ȭ
            isInsideTrigger = true; // Ʈ���� ���� ���η� ����
            player = other.gameObject; // �÷��̾� ������Ʈ ����
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.gameObject.SetActive(false); // �ؽ�Ʈ �޽� ��Ȱ��ȭ
            isInsideTrigger = false; // Ʈ���� ���� �ܺη� ����
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
        // ���ο� ������ "PlayerLocation" �±׸� ���� ������Ʈ ã��
        GameObject playerLocation = GameObject.FindWithTag("PlayerLocation");
        if (playerLocation != null && player != null)
        {
            player.transform.position = playerLocation.transform.position;
        }
        else
        {
            Debug.LogWarning("'PlayerLocation' �±׸� ���� ���� ������Ʈ�� �÷��̾� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }
}





