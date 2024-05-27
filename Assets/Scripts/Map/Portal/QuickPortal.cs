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
            text.SetActive(true); // 텍스트 메시 활성화
            isInsideTrigger = true; // 트리거 영역 내부로 설정
            player.interactingQuickPortal = this;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(false); // 텍스트 메시 비활성화
            isInsideTrigger = false; // 트리거 영역 외부로 설정
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
