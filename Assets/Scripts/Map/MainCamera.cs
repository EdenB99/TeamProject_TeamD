using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float cameraSpeed = 5.0f;
    private Player player;
    Vector3 cameraPosition;
    public LayerMask cameraWallMask; // "CameraWall" 레이어를 포함하는 레이어 마스크

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        cameraWallMask = LayerMask.GetMask("CameraWall");
    }

    private void LateUpdate()
    {
        CameraMove();

    }

    private void CameraMove()
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 direction = (playerPosition - transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, Vector3.Distance(transform.position, playerPosition), cameraWallMask))
        {
            // 장애물이 있으면 카메라를 장애물 위치로 이동
            cameraPosition = hit.point;
        }
        else
        {
            // 장애물이 없으면 카메라를 원래 위치로 이동
            cameraPosition = Vector3.Lerp(transform.position, playerPosition, Time.deltaTime * cameraSpeed);
        }

        transform.position = new Vector3(cameraPosition.x, cameraPosition.y, transform.position.z);
    }
}