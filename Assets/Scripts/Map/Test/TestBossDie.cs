using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class TestBossDie : MonoBehaviour
{

    TestInputAction action;
    public Boss_Knight Boss;
    private void Awake()
    {
        action = new TestInputAction();
    }

    private void OnEnable()
    {
        action.Test.Enable();
        action.Test.Test1.performed += OnTest1;

    }

    private void OnTest1(InputAction.CallbackContext context)
    {
        Boss = FindAnyObjectByType<Boss_Knight>();
        Boss.HP -= 100;
    }
}
