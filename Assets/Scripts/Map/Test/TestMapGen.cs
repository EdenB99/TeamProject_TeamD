using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class TestMapGen : MonoBehaviour
{

    TestInputAction action;
    EnemyBase[] enemies;
    GameObject enemyParent;
    [Header("7번 누를 시 적 전부 처치")]
    public GameObject seven;

    private void Awake()
    {
        action = new TestInputAction();
    }

    private void OnEnable()
    {
        action.Test.Enable();
        action.Test.Test1.performed += OnTest1;
        action.Test.Test2.performed += OnTest2;

    }

    private void OnTest2(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("EndScene");
    }

    private void OnTest1(InputAction.CallbackContext context)
    {
        if (enemyParent == null)
        {
            enemyParent = GameObject.Find("Enemy");
        }
        if (enemyParent != null)
        {
            enemies = enemyParent.GetComponentsInChildren<EnemyBase>();

            for (int i = 0; i < enemies.Length; i++)
            {
                Destroy(enemies[i].gameObject);
            }

        }
    }
    
}
