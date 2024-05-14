using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRespawner : MonoBehaviour
{
    public GameObject playerPrefab; // 플레이어 프리팹
    public Transform respawnPoint; // 플레이어가 부활할 위치
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

                // 부활 지점에서 새로운 플레이어 인스턴스 생성
                GameObject myInstance = Instantiate(playerPrefab, respawnPoint.position, respawnPoint.rotation);

                // 새로운 플레이어의 상태 초기화
                PlayerStats newPlayerStats = myInstance.GetComponent<PlayerStats>();
                if (newPlayerStats != null)
                {
                    newPlayerStats.SetInitialState();
                }

                // 기존 플레이어 객체 제거
                if (currentPlayer != null)
                {
                    Destroy(currentPlayer);
                }

                // 현재 플레이어를 새로운 인스턴스로 설정
                currentPlayer = myInstance;
            }
          
        }
    }

    public void Respawn()
    {
        Debug.Log("플레이어 부활");
        SceneManager.LoadScene("Town", LoadSceneMode.Single);
    }
}
