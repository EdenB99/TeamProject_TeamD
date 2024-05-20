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
    public Image Fade;

    /// <summary>
    /// ���������� Ȯ���ϴ� �ڵ�
    /// </summary>
    bool fadeEffect;

    /// <summary>
    /// ���� ���� ��
    /// </summary>
    private float fadeFloat = 0.0f;

    private int menuIndex = 0;
    Vector3 selectPos = new Vector3(200, 0, 0);


    void Start()
    {
        menuText = GetComponentsInChildren<TextMeshProUGUI>();
        Remover remover = FindAnyObjectByType<Remover>();
        remover.ClearAllDontDestroyOnLoadObjects();
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
            if (!fadeEffect)
            {
                fadeEffect = true;
                StartCoroutine(SelectMenuItem());
            }
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

    IEnumerator SelectMenuItem()
    {
        while (fadeEffect)
        {
            Debug.Log(fadeFloat);
            fadeFloat += Time.deltaTime * 0.5f;
            Fade.color = new Color(0, 0, 0, fadeFloat);
            if (fadeFloat > 1) fadeEffect = false;
            yield return new WaitForSeconds(0.0f);
        }

        yield return new WaitForSeconds(0.5f);

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
