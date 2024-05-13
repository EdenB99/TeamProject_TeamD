using UnityEngine;

public class MainCamera : Singleton<MainCamera>
{


    public float cameraSpeed = 5.0f;
    public GameObject boundaryObject; // Boundary 게임 오브젝트 참조

    private Player player;
    private Vector3 cameraPosition;
    private BoxCollider2D boundaryCollider;
    private float cameraHeight;
    private float cameraWidth;


    protected override void OnPreInitialize()
    {
        base.OnPreInitialize();

    }

    protected override void OnInitialize()
    {
        player = GameObject.FindObjectOfType<Player>();
        FindBoundaryObject();
        CalculateCameraSize();
    }


    private void LateUpdate()
    {
        if (boundaryCollider == null)
        {
            FindBoundaryObject();
        }
        if(player == null)
        {
            player = GameObject.FindObjectOfType<Player>();
        }

        CameraMove();
    }



    //경계 관련 코드============================================

    //경계찾기(없으면)
    private void FindBoundaryObject()
    {

        GameObject foundBoundaryObject = GameObject.FindGameObjectWithTag("Boundary");

        if (foundBoundaryObject != null)
        {
            boundaryObject = foundBoundaryObject;
            boundaryCollider = boundaryObject.GetComponent<BoxCollider2D>();
        }
    }
    //경계 교체(public)
    public void UpdateBoundaryObject(GameObject newBoundaryObject)
    {
        boundaryObject = newBoundaryObject;
        boundaryCollider = boundaryObject.GetComponent<BoxCollider2D>();
    }

    //카메라 관련 코드============================================

    //카메라크기계산
    private void CalculateCameraSize()
    {

        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraWidth = cameraHeight * Camera.main.aspect;
    }

    //카메라이동
    private void CameraMove()
    {
        if(player != null)
        {

        Vector3 playerPosition = player.transform.position;

        // 카메라 위치를 플레이어 위치로 설정
        cameraPosition = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);

        // BoxCollider2D의 bounds 정보를 사용하여 카메라 위치를 제한된 범위 내로 조정
        if (boundaryCollider != null)
        {
            Bounds bounds = boundaryCollider.bounds;
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, bounds.min.x + cameraWidth / 2, bounds.max.x - cameraWidth / 2);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, bounds.min.y + cameraHeight / 2, bounds.max.y - cameraHeight / 2);
        }

        transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * cameraSpeed);
    }

        }
}