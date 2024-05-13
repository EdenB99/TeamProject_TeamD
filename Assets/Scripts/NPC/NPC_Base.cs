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
        Tutorial,
        Event
        
    }

    public NPCType npcType;

    [TextArea(3, 5)]
    protected string[] dialogues = new string[] { "" };
    protected uint currentDialogIndex = 0;

    public Transform dialogBox;
    public TextMeshProUGUI dialogText;

    public bool IsInteracting;
    Transform key;

    protected virtual void Awake()
    {
        key = transform.GetChild(0);

        ShowDialog();
        dialogBox.gameObject.SetActive(false);
    }

    public virtual void StartDialog()
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
        } else if (IsInteracting&& currentDialogIndex == dialogues.Length)
        {
            EndDialog();
        }
        
    }

    public virtual void ShowDialog()
    {
        if (currentDialogIndex < dialogues.Length)
        {
            dialogText.text = dialogues[currentDialogIndex];
            currentDialogIndex++;
        }
    }

    public virtual void EndDialog()
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
        if (IsInteracting)
        {
            dialogBox.gameObject.SetActive(false);
            currentDialogIndex = 0;
            IsInteracting = false;
        }
    }
}
