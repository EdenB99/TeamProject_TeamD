using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    /*����� �ʻ���
    ó�� �� ���� �� �� �Ʒ� ���� ������ ��Ż�� ���� �� ����Ʈ�� ����ִ� ���� �ϳ� �������� �ҷ��ͼ� �ʿ� �ֱ�
    �־��� ���� ������ ���� �׸�ڽ��� ǥ���ϱ�
    */





    //direction limit
    private int DL = 3;
    private int left;
    private int right;
    private int up;
    private int down;
    private int eventMap;
    private int mapLimit = 20;
    


    private bool isGenEnd;

    //Left,Right,Up,Down Scene Name
     string LSN = "LeftMap10";
     string RSN = "RightMap10";
     string USN = "UpMap10";
     string DSN = "DownMap10";
   

    //TODO :: ��Ż �Լ� ��ĥ�� ����, ������ ������ �����ܸ��� ���� �� ���� �ʸ� ����, �� ��Ż�̸��� ����
    // ���� ����(left,right,Up,Down), �̺�Ʈ �� � ������.



    //TODO:: ��Ż �� �Ʒ� ���� ������ ���Խ� �ش��ϴ� �� �ҷ��ͼ� �ֱ�
    void LeftPortal()
    {
        if (!isGenEnd)
        {

            if (left > DL)
            {
                int random = Random.Range(0, 5);
                SceneManager.LoadScene(LSN + random);
            }
            else
            {

            }
        }

    }

    void RightPortal()
    {

    }

    void UpPortal()
    {

    }

    void DownPortal()
    {

    }



}
