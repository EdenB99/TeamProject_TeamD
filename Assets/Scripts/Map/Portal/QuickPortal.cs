using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickPortal : MonoBehaviour
{
    GameObject text;
    public bool isInsideTrigger = false;
    Player player;
    MapUI mapUI;

    private void Awake()
    {
        text = transform.GetChild(1).gameObject;
        player = GameManager.Instance.Player;
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(true); // �ؽ�Ʈ �޽� Ȱ��ȭ
            isInsideTrigger = true; // Ʈ���� ���� ���η� ����
            player.interactingQuickPortal = this;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(false); // �ؽ�Ʈ �޽� ��Ȱ��ȭ
            isInsideTrigger = false; // Ʈ���� ���� �ܺη� ����
            player.interactingQuickPortal = null;
            mapUI.HideMap();
        }
    }

    public void OnQuickTrevel()
    {
        if(mapUI == null)
        {
            mapUI = FindAnyObjectByType<MapUI>();
        }
        mapUI.QuickTrevel();

    }


}
