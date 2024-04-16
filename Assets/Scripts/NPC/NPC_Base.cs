using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static NPC_Base;

public class NPC_Base : MonoBehaviour
{
    public enum NPCType
    {
        Store,
        Tutorial
        
    }

    public NPCType npcType;

    [TextArea(3, 5)]
    protected string[] dialogues = new string[] { "" };
    protected uint currentDialogIndex = 0;

    public TextMeshProUGUI dialogText;
    public bool IsInteracting;
    protected Transform dialogBox;
    Canvas canvas;
    Transform key;

    protected virtual void Awake()
    {
        canvas = FindAnyObjectByType<Canvas>();
        dialogBox = canvas.transform.GetChild(0);
        dialogText = FindAnyObjectByType<TextMeshProUGUI>();
        key = transform.GetChild(0);

        ShowDialog();
        dialogBox.gameObject.SetActive(false);
    }

    public void StartDialog()
    {
        currentDialogIndex = 0;
        dialogBox.gameObject.SetActive(true);
        ShowDialog();
        IsInteracting = true;
    }

    public virtual void NextDialog()
    {
        if (IsInteracting && currentDialogIndex < dialogues.Length)
        {
            ShowDialog();
        }
    }

    public void ShowDialog()
    {
        if (currentDialogIndex < dialogues.Length)
        {
            dialogText.text = dialogues[currentDialogIndex];
            currentDialogIndex++;
        }
    }

    public void EndDialog()
    {
        if(IsInteracting)
        {
            dialogBox.gameObject.SetActive(false);
            IsInteracting = false;
            currentDialogIndex = 0; 
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
