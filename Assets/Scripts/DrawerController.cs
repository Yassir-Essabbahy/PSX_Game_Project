using UnityEngine;

public class DrawerController : MonoBehaviour
{
    [Header("Drawer Settings")]
    public float slideDistance = 0.5f;
    public float smooth = 2f;
    public bool isLocked = false;
    public string lockedMessage = "الدرج مسدود";

    [Header("Sound")]
    public AudioClip openCloseSound;

    private AudioSource audioSource;

    private bool isOpen = false;

    private Vector3 closedPosition;
    private Vector3 openPosition;

    void Start()
    {
        closedPosition = transform.localPosition;

        // Slides forward
        openPosition = closedPosition + new Vector3(0, 0, -slideDistance);

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        Vector3 target = isOpen ? openPosition : closedPosition;

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            target,
            Time.deltaTime * smooth
        );
    }

    public void ToggleDrawer()
    {
        if (isLocked)
        {
            UIManager.instance.ShowMessage(lockedMessage);
            return;
        }

        isOpen = !isOpen;

        // Play sound only if available
        if (openCloseSound != null)
        {
            audioSource.PlayOneShot(openCloseSound);
        }
    }
}