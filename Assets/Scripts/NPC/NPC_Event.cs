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
    [Range(1, 10)]
    public int DialogueCount;
    
    [SerializeField]
    public EventDialogues[] DialoguesCategory;

    protected override void Awake()
    {
        base.Awake();
        if (npcType == NPCType.Event)
        {
            dialogues = (string[])DialoguesCategory.GetValue(0);
        }
    }

    int currentindex = 0;

    private void OnValidate()
    {
        
    }
}

