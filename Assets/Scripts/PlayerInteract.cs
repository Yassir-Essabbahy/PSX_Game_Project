using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public float range = 2f;
    public Camera playerCamera;
    public Image crosshairFill; // circular UI image, type = Filled
    public TextMeshProUGUI promptText;
    public static bool isBlocked = false;
    private Interactable current;

    void Update()
    {
        if (isBlocked)
        {
            crosshairFill.fillAmount = 0f;
            return;
        }

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(ray, range);
        current = null;

        foreach (RaycastHit hit in hits)
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            if (interactable == null)
                interactable = hit.collider.GetComponentInParent<Interactable>();

            if (interactable != null)
            {
                current = interactable;
                break;
            }
        }

        if (current != null)
{
    crosshairFill.fillAmount = 1f;
    promptText.text = current.promptMessage;

    if (Input.GetKeyDown(KeyCode.E))
        current.Interact();
}
else
{
    crosshairFill.fillAmount = 0f;
    promptText.text = "";
}
    }
}