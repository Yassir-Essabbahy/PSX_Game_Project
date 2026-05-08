using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public float range = 2f;
    public GameObject promptUI;
    public Camera playerCamera;
    public TextMeshProUGUI promptText;
    public static bool isBlocked = false;
    private Interactable current;

    void Update()
    {
        if (isBlocked) return;

        Ray ray = playerCamera != null
            ? new Ray(playerCamera.transform.position, playerCamera.transform.forward)
            : new Ray(transform.position, transform.forward);

        // debug always visible
        Debug.DrawRay(ray.origin, ray.direction * range, Color.red);

        RaycastHit[] hits = Physics.RaycastAll(ray, range); // hits ALL colliders not just first

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
            promptUI.SetActive(true);
            promptText.text = current.promptMessage;

            if (Input.GetKeyDown(KeyCode.E))
            {
                current.Interact();
                promptUI.SetActive(false);
            }
        }
        else
        {
            promptUI.SetActive(false);
            current = null;
        }
    }
}