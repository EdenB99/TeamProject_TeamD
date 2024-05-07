using UnityEngine;

//TODO:: �̱��� ����, �̴ϸ�ī�޶� ������.
public class MiniMapCamera : Singleton<MiniMapCamera>
{
    public float cameraSpeed = 5.0f;
    public GameObject boundaryObject; // Boundary ���� ������Ʈ ����

    private Player player;
    private Vector3 cameraPosition;
    private BoxCollider2D boundaryCollider;
    private float cameraHeight;
    private float cameraWidth;

    //TODO �ش������ �����ϸ� �̴ϸ� ī�޶� �����Ǵ���, �̱������?
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
        // "Boundary" �±׸� ���� GameObject ã��
        GameObject foundBoundaryObject = GameObject.FindGameObjectWithTag("Boundary");

        if (foundBoundaryObject != null)
        {
            boundaryObject = foundBoundaryObject;
            boundaryCollider = boundaryObject.GetComponent<BoxCollider2D>();
        }
    }

    private void CalculateCameraSize()
    {
        // ī�޶��� ũ�� ���
        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraWidth = cameraHeight * Camera.main.aspect;
    }

    private void CameraMove()
    {
        Vector3 playerPosition = player.transform.position;

        // ī�޶� ��ġ�� �÷��̾� ��ġ�� ����
        cameraPosition = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);

        // BoxCollider2D�� bounds ������ ����Ͽ� ī�޶� ��ġ�� ���ѵ� ���� ���� ����
        if (boundaryCollider != null)
        {
            Bounds bounds = boundaryCollider.bounds;
            cameraPosition.x = Mathf.Clamp(cameraPosition.x, bounds.min.x + cameraWidth / 2, bounds.max.x - cameraWidth / 2);
            cameraPosition.y = Mathf.Clamp(cameraPosition.y, bounds.min.y + cameraHeight / 2, bounds.max.y - cameraHeight / 2);
        }

        transform.position = Vector3.Lerp(transform.position, cameraPosition, Time.deltaTime * cameraSpeed);
    }
}