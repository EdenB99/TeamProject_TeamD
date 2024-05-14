using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Event : NPC_Base
{
    [Serializable]
    public struct EventDialogues
    {
        [TextArea(3,5)]
        public string[] newDialogues;
    }

    [SerializeField]
    public EventDialogues[] DialoguesCategory;

    private int currentCategory = 0;

    [SerializeField]
    public ItemData[] ItemdataCategory;

    private int SendItemCategory;


    protected override void Awake()
    {
        base.Awake();
        if (npcType == NPCType.Event)
        {
            dialogues = DialoguesCategory[0].newDialogues;
            Debug.Log(dialogues[0]);
        }
    }

    int currentindex = 0;
    public override void EndDialog()
    {
        if (IsInteracting)
        {
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
    private void DropItem()
    {
        //프리팹 생성후 아이템을 드랍하는 기능 구현
       /*if (itemObjectPrefab != null)
        {
            GameObject newItemObject = Instantiate(itemObjectPrefab, transform.position, Quaternion.identity);
            newItemObject.AddComponent<ItemObject>();
            ItemObject obj = newItemObject.GetComponent<ItemObject>();
            obj.ItemData = ItemdataCategory[0];
        }*/
    }
    
}

