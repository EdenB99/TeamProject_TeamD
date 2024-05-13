using UnityEngine;

public class MainCamera : Singleton<MainCamera>
{


    public float cameraSpeed = 5.0f;
    public GameObject boundaryObject; // Boundary ���� ������Ʈ ����

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



    //��� ���� �ڵ�============================================

    //���ã��(������)
    private void FindBoundaryObject()
    {

        GameObject foundBoundaryObject = GameObject.FindGameObjectWithTag("Boundary");

        if (foundBoundaryObject != null)
        {
            boundaryObject = foundBoundaryObject;
            boundaryCollider = boundaryObject.GetComponent<BoxCollider2D>();
        }
    }
    //��� ��ü(public)
    public void UpdateBoundaryObject(GameObject newBoundaryObject)
    {
        boundaryObject = newBoundaryObject;
        boundaryCollider = boundaryObject.GetComponent<BoxCollider2D>();
    }

    //ī�޶� ���� �ڵ�============================================

    //ī�޶�ũ����
    private void CalculateCameraSize()
    {

        cameraHeight = Camera.main.orthographicSize * 2f;
        cameraWidth = cameraHeight * Camera.main.aspect;
    }

    //ī�޶��̵�
    private void CameraMove()
    {
        if(player != null)
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
}