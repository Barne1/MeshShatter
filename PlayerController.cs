using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField, Range(0, 100)] float movementSpeed;

    [Header("Camera")]
    [SerializeField, Range(0, 10000)] float cameraSensitivityX;
    [SerializeField, Range(0, 10000)] float cameraSensitivityY;
    [SerializeField, Range(0, 90)] float maxYAngle;
    Quaternion defaultRotation;


    //horizontal = a--d, vertical = w--s
    const string horizontalId = "Horizontal";
    const string verticalId = "Vertical";

    //Camera & mouselook
    const string mouseHorizontal = "Mouse X";
    const string mouseVertical = "Mouse Y";

    [SerializeField] MeshCutter mc;

    private void Awake()
    {
        if(!mc)
        {
            mc = GetComponent<MeshCutter>();
        }
        if(!playerCamera)
        {
            playerCamera = Camera.main.transform;
        }
        defaultRotation = playerCamera.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        DoMovement();
        CameraRotate();
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                mc.Cut(hit.collider.gameObject);
            }
        }
    }

    private void CameraRotate()
    {
        //Y = left,right (mouseX)
        //-X = up, down (mouseY)

        float lookLR = Input.GetAxis(mouseHorizontal) * Time.deltaTime * cameraSensitivityX;
        transform.localRotation *= Quaternion.AngleAxis(lookLR, Vector3.up);

        float lookUD = Input.GetAxis(mouseVertical) * Time.deltaTime * cameraSensitivityY;
        Quaternion nextRotation = playerCamera.localRotation * Quaternion.AngleAxis(lookUD, -Vector3.right);
        if(Quaternion.Angle(nextRotation, defaultRotation) < maxYAngle)
            playerCamera.localRotation = nextRotation;
        
    }

    private void DoMovement()
    {
        Vector3 movement = new Vector3(Input.GetAxis(horizontalId), 0f, Input.GetAxis(verticalId)).normalized;
        movement *= Time.deltaTime * movementSpeed;
        transform.Translate(movement);
    }
}
