using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TownPortal : MonoBehaviour
{
    private GameObject text;
    public bool isInsideTrigger;
    Player player;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        text = child.gameObject;
        text.SetActive(false);
        player = GameManager.Instance.Player;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(true); // �ؽ�Ʈ �޽� Ȱ��ȭ
            isInsideTrigger = true; // Ʈ���� ���� ���η� ����
            player.townptal = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(false); // �ؽ�Ʈ �޽� ��Ȱ��ȭ
            isInsideTrigger = false; // Ʈ���� ���� �ܺη� ����
            player.townptal = null;
        }
    }

    public void LodaingNext()
    {
        SceneManager.LoadScene("AsyncLoadScene");

    }

    /*private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���ο� ������ "PlayerLocation" �±׸� ���� ������Ʈ ã��, ������ �װ����� �̵�.
        GameObject playerLocation = GameObject.FindWithTag("PlayerLocation");
        if (playerLocation != null && player != null)
        {
            player.transform.position = playerLocation.transform.position;
        }

    }*/
}