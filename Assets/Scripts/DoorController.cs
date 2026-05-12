using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float smooth = 2f;
    public bool isLocked;

    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, 0, openAngle);
    }

    void Update()
    {
        if (isOpen)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, openRotation, Time.deltaTime * smooth);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, closedRotation, Time.deltaTime * smooth);
        }
    }

    public void ToggleDoor()
    {
    if (isLocked)
    {
        UIManager.instance.ShowMessage("Baba makay5linich nd5l hna");
        return;
    }
    isOpen = !isOpen;
    }
}