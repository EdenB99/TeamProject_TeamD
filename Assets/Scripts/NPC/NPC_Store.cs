using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Store : NPC_Base
{
    [TextArea(3, 10)]
    public string[] storeDialogues = new string[] { "안녕하세요, 상점입니다.", "무엇을 도와드릴까요?" };

    public Transform StoreUI;
    protected override void Awake()
    {
        base.Awake();

        if (npcType == NPCType.Store)
        {
            dialogues = storeDialogues;
        }
    }

    public override void NextDialog()
    {
        if (IsInteracting && currentDialogIndex < dialogues.Length)
        {
            ShowDialog();
        }
        else
        {
            ShowStore();
        }
    }

    void ShowStore()
    {
        dialogBox.gameObject.SetActive(false);
        StoreUI.gameObject.SetActive(true);
    }

    public void DisableStore()
    {
        StoreUI.gameObject.SetActive(false);
    }
}
