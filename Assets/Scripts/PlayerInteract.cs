using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public float range = 2f;
    public GameObject promptUI;
    public Camera playerCamera; // drag your Camera here in Inspector
    private Interactable current;
    public TextMeshProUGUI promptText;

    public static bool isBlocked = false;

    void Update()
    {
        if (isBlocked) return;


        // shoot from camera center, not player feet
        Ray ray = playerCamera != null
            ? new Ray(playerCamera.transform.position, playerCamera.transform.forward)
            : new Ray(transform.position, transform.forward);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            current = hit.collider.GetComponent<Interactable>();

            // also check parent in case collider is on a child object
            if (current == null)
                current = hit.collider.GetComponentInParent<Interactable>();

            if (current != null)
            {
                
                promptUI.SetActive(true);
                promptText.text = current.promptMessage;

                if (Input.GetKeyDown(KeyCode.E))
                {

                    current.Interact();

                promptUI.SetActive(false);
                }
                return;

            }
        }

        promptUI.SetActive(false);
        current = null;
    }
}