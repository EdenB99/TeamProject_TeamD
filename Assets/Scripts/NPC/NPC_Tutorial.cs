using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Tutorial : NPC_Base
{
    [TextArea(3, 10)]
    public string[] tutorialDialogues = new string[] {  };
    protected override void Awake()
    {
        base.Awake();
        if (npcType == NPCType.Tutorial)
        {
            dialogues = tutorialDialogues;
        }
    }
}
