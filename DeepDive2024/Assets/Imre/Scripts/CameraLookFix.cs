using UnityEngine;

public class CameraLookFix : MonoBehaviour
{
    [Header("Look Variables")]
    [SerializeField] private Transform hand;
    [SerializeField] private Transform _camera;
    [SerializeField] private float camSens = 200f;
    [SerializeField] private float camAcc = 5f;
    private float rotationX;
    private float rotationY;
    public float rotXClamp;
    public float rotYClamp;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
    }

    private void HandleMouseLook()
    {
        rotationX += Input.GetAxis("Mouse Y") * camSens * Time.deltaTime;
        rotationY += Input.GetAxis("Mouse X") * camSens * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -rotXClamp, rotXClamp);
        rotationY = Mathf.Clamp(rotationY, -rotYClamp, rotYClamp);

        hand.localRotation = Quaternion.Euler(-rotationX, rotationY, 0);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, rotationY, 0), camAcc * Time.deltaTime);
        _camera.localRotation = Quaternion.Lerp(_camera.localRotation, Quaternion.Euler(-rotationX, 0, 0), camAcc * Time.deltaTime);
    }
}
