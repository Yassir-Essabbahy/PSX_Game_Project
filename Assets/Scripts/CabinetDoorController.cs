using UnityEngine;

public class CabinetDoorController : MonoBehaviour
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float smooth = 2f;
    public bool isLocked = false;
    public string lockedMessage = "الخزانة مسدودة";

    [Header("Sound")]
    public AudioClip openCloseSound;

    private AudioSource audioSource;

    private bool isOpen = false;

    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);

        // Get or create AudioSource
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        Quaternion target = isOpen ? openRotation : closedRotation;

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            target,
            Time.deltaTime * smooth
        );
    }

    public void ToggleDoor()
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