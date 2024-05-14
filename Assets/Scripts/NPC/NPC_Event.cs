using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class NPC_Event : NPC_Base
{
    [Serializable]
    public struct EventDialogues
    {
        [TextArea(3,5)]
        public string[] newDialogues;
        public bool eventToggle;

        public ItemData[] eventItemData;
    }

    [SerializeField]
    public EventDialogues[] DialoguesCategory;

    private int currentCategory = 0;

    protected override void Awake()
    {
        base.Awake();
        if (npcType == NPCType.Event)
        {
            dialogues = DialoguesCategory[0].newDialogues;
            Debug.Log(dialogues[0]);
        }
    }

    public override void EndDialog()
    {
        if (IsInteracting)
        {
            if (DialoguesCategory[currentCategory].eventToggle)
            {
                DropItem(DialoguesCategory[currentCategory].eventItemData);
            }
            dialogBox.gameObject.SetActive(false);
            IsInteracting = false;
            currentDialogIndex = 0;
            if (currentCategory < DialoguesCategory.Length - 1)
            {
                currentCategory++;
                dialogues = DialoguesCategory[currentCategory].newDialogues;
            }
        }
    }
    private void DropItem(ItemData[] itemdatas)
    {
        for (int i = 0; i < itemdatas.Length; i++)
        {
            Factory.Instance.MakeItems(ItemCode.Coin, 1, transform.position);
        }
    }
    
}

