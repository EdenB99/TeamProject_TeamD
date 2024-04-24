using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class SlashEffect : WeaponEffect
{
    Animator animator;
    BoxCollider2D slashCollider;

    protected override void Awake()
    {
        base.Awake();
        slashCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
