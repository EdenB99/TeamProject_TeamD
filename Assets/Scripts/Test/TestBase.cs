using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestBase : MonoBehaviour
{
    TestInputAction action;

    private void Awake()
    {
        action = new TestInputAction();
    }


    private void Start()
    {
        
    }

    private void OnEnable()
    {
        action.Test.Enable();

        action.Test.Test1.performed += OnTest1;
        action.Test.Test2.performed += OnTest2;
        action.Test.Test3.performed += OnTest3;
    }

    private void OnDisable()
    {
        action.Test.Test3.performed += OnTest3;
        action.Test.Test2.performed += OnTest2;
        action.Test.Test1.performed += OnTest1;


        action.Test.Disable();
    }

    /// <summary>
    /// 7번 입력
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void OnTest1(InputAction.CallbackContext context)
    {
        
    }

    /// <summary>
    /// 8번 입력
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void OnTest2(InputAction.CallbackContext context)
    {
        
    }

    /// <summary>
    /// 9번 입력
    /// </summary>
    /// <param name="context"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected virtual void OnTest3(InputAction.CallbackContext context)
    {
        
    }



}
