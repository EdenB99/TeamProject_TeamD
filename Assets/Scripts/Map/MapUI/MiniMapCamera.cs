using UnityEngine;

//TODO:: 싱글톤 오류, 미니맵카메라가 삭제됨.
public class MiniMapCamera : Singleton<MiniMapCamera>
{
    public float cameraSpeed = 5.0f;
    public GameObject boundaryObject; // Boundary 게임 오브젝트 참조

    private Player player;
    private Vector3 cameraPosition;
    private BoxCollider2D boundaryCollider;
    private float cameraHeight;
    private float cameraWidth;

    //TODO 해당오류가 등장하며 미니맵 카메라 삭제되는중, 싱글톤오류?
    /*  DontDestroyOnLoad only works for root GameObjects or components on root GameObjects.
  UnityEngine.StackTraceUtility:ExtractStackTrace()
  Singleton`1<MiniMapCamera>:Awake() (at Assets/Scripts/Core/Singleton.cs:57)
  */



    private void Start()
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

        CameraMove();
    }

    private void FindBoundaryObject()
    {
        // "Boundary" 태그를 가진 GameObject 찾기
        GameObject foundBoundaryObject = GameObject.FindGameObjectWithTag("Boundary");

        if (foundBoundaryObject != null)
        {
            boundaryObject = foundBoundaryObject;
            boundaryCollider = boundaryObject.GetComponent<BoxCollider2D>();
        }
    }

    private void CalculateCameraSize()
    {
        // 카메라의 크기 계산
        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraWidth = cameraHeight * Camera.main.aspect;
    }

    private void CameraMove()
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