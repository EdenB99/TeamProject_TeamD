using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Store : NPC_Base
{
    InventoryUI inven;
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
    private void Start()
    {
        inven = GameManager.Instance.InventoryUI;
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
        inven.isStore = true;
        dialogBox.gameObject.SetActive(false);
        StoreUI.gameObject.SetActive(true);
    }

    public void DisableStore()
    {
        inven.isStore = false;
        StoreUI.gameObject.SetActive(false);
    }
}
