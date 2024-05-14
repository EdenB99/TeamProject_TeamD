using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawner : MonoBehaviour
{
    public GameObject playerPrefab; // �÷��̾� ������
    public Transform respawnPoint; // �÷��̾ ��Ȱ�� ��ġ
    private GameObject currentPlayer;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Town")
        {
            if (respawnPoint != null && playerPrefab != null)
            {
                Debug.Log("Respawn point position: " + respawnPoint.position);

                // ��Ȱ �������� ���ο� �÷��̾� �ν��Ͻ� ����
                GameObject myInstance = Instantiate(playerPrefab, respawnPoint.position, respawnPoint.rotation);

                // ���ο� �÷��̾��� ���� �ʱ�ȭ
                PlayerStats newPlayerStats = myInstance.GetComponent<PlayerStats>();
                if (newPlayerStats != null)
                {
                    newPlayerStats.SetInitialState();
                }

                // ���� �÷��̾� ��ü ����
                if (currentPlayer != null)
                {
                    Destroy(currentPlayer);
                }

                // ���� �÷��̾ ���ο� �ν��Ͻ��� ����
                currentPlayer = myInstance;
            }
          
        }
    }

    public void Respawn()
    {
        Debug.Log("�÷��̾� ��Ȱ");
        SceneManager.LoadScene("Town", LoadSceneMode.Single);
    }
}
