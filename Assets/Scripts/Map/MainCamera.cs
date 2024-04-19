using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float cameraSpeed = 5.0f;
    private Player player;
    Vector3 cameraPosition;
    public LayerMask cameraWallMask; // "CameraWall" ���̾ �����ϴ� ���̾� ����ũ

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
            // ��ֹ��� ������ ī�޶� ��ֹ� ��ġ�� �̵�
            cameraPosition = hit.point;
        }
        else
        {
            // ��ֹ��� ������ ī�޶� ���� ��ġ�� �̵�
            cameraPosition = Vector3.Lerp(transform.position, playerPosition, Time.deltaTime * cameraSpeed);
        }

        transform.position = new Vector3(cameraPosition.x, cameraPosition.y, transform.position.z);
    }
}