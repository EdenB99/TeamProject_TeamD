using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPanel : MonoBehaviour
{
    private int gold;
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            goldChange?.Invoke(gold);
        }
    }
    Action<int> goldChange;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
