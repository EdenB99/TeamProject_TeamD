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
    public string dialogue = "어서 오세요! 무엇을 도와드릴까요?";
    public TextMeshProUGUI dialogText;
    Image dialogBox;
    Transform key;

    private void Awake()
    {
        dialogBox = FindAnyObjectByType<Image>();
        dialogText = FindAnyObjectByType<TextMeshProUGUI>();
        key = transform.GetChild(0);
    }

    private void Start()
    {
        ShowDialog();
        dialogBox.gameObject.SetActive(false);
    }

    private void ShowDialog()
    {
        if(npcType == NPCType.Store)
        {
            dialogText.text = dialogue;
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
