using UnityEngine;

public class WindowController : MonoBehaviour
{
    [Header("Window Settings")]
    public float slideDistance = 1f;
    public float smooth = 2f;
    public bool isLocked = false;
    public string lockedMessage = "النافذة مسدودة";

    private bool isOpen = false;
    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        closedPosition = transform.localPosition;
        openPosition = closedPosition + new Vector3(-slideDistance, 0, 0);
        // negative = left, if it goes wrong way change to positive
    }

    void Update()
    {
        Vector3 target = isOpen ? openPosition : closedPosition;
        transform.localPosition = Vector3.Lerp(
            transform.localPosition, target, Time.deltaTime * smooth);
    }

    public void ToggleWindow()
    {
        if (isLocked)
        {
            UIManager.instance.ShowMessage(lockedMessage);
            return;
        }
        isOpen = !isOpen;
    }
}