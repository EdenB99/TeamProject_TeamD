using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    TextMeshProUGUI[] menuText;
    public Image selecter;
    private int menuIndex = 0;
    Vector3 selectPos = new Vector3(200, 0, 0);


    void Start()
    {
        menuText = GetComponentsInChildren<TextMeshProUGUI>();
        UpdateMenu();
    }

    void Update()
    {
        selecter.transform.position = menuText[menuIndex].transform.position - selectPos;

        if (Input.GetKeyDown(KeyCode.W))
        {
            menuIndex--;
            if (menuIndex < 0) menuIndex = menuText.Length - 1; // 0�̸� ���������� �̵�
            UpdateMenu();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            menuIndex++;
            if (menuIndex >= menuText.Length) menuIndex = 0;    // �ִ�� 0���� �̵�
            UpdateMenu();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SelectMenuItem();
        }
    }

    /// <summary>
    /// Ű �Է½� ����� �޼���
    /// </summary>
    void UpdateMenu()
    {

        

        for (int i = 0; i < menuText.Length; i++)
        {
            if (i == menuIndex)
            {
                menuText[i].color = Color.yellow; // ���õ� �׸��� ���� ����
            }
            else
            {
                menuText[i].color = Color.white; // �ٸ� �׸��� ���� ����
            }
        }
    }

    /// <summary>
    /// ���ý� ����� �޼���
    /// </summary>
    void SelectMenuItem()
    {
        switch (menuIndex)
        {
            case 0:
                SceneManager.LoadScene("FirstLoadScene");
                break;
            case 1:
                // Ʃ�丮�� ������ �̵�

                break;
            case 2:
                // ���� â �ѱ�

                break;
            case 3:
                // ����
                Application.Quit();
                break;
        }
    }
}
