using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    /*동기식 맵생성
    처음 맵 진입 시 위 아래 왼쪽 오른쪽 포탈로 진입 시 리스트에 들어있는 맵중 하나 랜덤으로 불러와서 맵에 넣기
    넣어진 맵은 지도에 들어가서 네모박스로 표시하기
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
   

    //TODO :: 포탈 함수 합칠지 생각, 맵제한 넘으면 맵종단마다 방이 더 없는 맵만 생성, 들어간 포탈이름에 따라
    // 방향 결정(left,right,Up,Down), 이벤트 방 몇개 들어가야함.



    //TODO:: 포탈 위 아래 왼쪽 오른쪽 진입시 해당하는 맵 불러와서 넣기
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
