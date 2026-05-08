using UnityEngine;

public class LimitedCameraLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    // Total = 160°
    public float maxLookAngle = 80f;

    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // FPS vertical look
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        // Horizontal limited look
        yRotation += mouseX;
        yRotation = Mathf.Clamp(yRotation, -maxLookAngle, maxLookAngle);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        
    }
}