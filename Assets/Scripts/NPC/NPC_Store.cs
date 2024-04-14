using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Store : NPC_Base
{
    [TextArea(3,5)]
    public string[] dialogues = new string[]
        { "어서 오세요! 무엇을 도와드릴까요?",
        "나나나나" };
    int currentDialogIndex = 0;
    public TextMeshProUGUI dialogText;
    Canvas canvas;
    Transform dialogBox;
    Transform key;
    Player player;
    public bool IsInteracting;

    private void Awake()
    {
        canvas = FindAnyObjectByType<Canvas>();
        dialogBox = canvas.transform.GetChild(0); 
        dialogText = FindAnyObjectByType<TextMeshProUGUI>();
        player = GameManager.Instance.Player;
        key = transform.GetChild(0);

        ShowDialog();
        dialogBox.gameObject.SetActive(false);
    }

    public void ShowDialog()
    {
        if(npcType == NPCType.Store)
        {
            if(currentDialogIndex < dialogues.Length)
            {
                dialogText.text = dialogues[currentDialogIndex];
                currentDialogIndex++;
            }
        }
    }

    public void StartDialogue()
    {
        if (npcType == NPCType.Store)
        {
            currentDialogIndex = 0;
            dialogBox.gameObject.SetActive(true);
            ShowDialog();
            IsInteracting = true;
        }
    }

    // 다음 대화로 넘기는 메서드
    public void NextDialogue()
    {
        if (IsInteracting && currentDialogIndex < dialogues.Length)
        {
            ShowDialog();
        }
        else
        {
            dialogBox.gameObject.SetActive(false);
            IsInteracting = false;
            currentDialogIndex = 0; // 대화 인덱스 초기화
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            key.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            key.gameObject.SetActive(false);
        }
    }
}
