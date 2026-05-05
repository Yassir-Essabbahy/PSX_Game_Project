using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public string promptMessage = "اضغط E";

    // drag any function here in Inspector
    public UnityEvent onInteract;

    public void Interact()
    {
        onInteract.Invoke();
    }
}