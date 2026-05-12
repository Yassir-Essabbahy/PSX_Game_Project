using UnityEngine;

public class MessageTrigger : MonoBehaviour
{
    [Header("Message Settings")]
    public string message = "Hello!";
    public float duration = 2f;

    public void SendMessage()
    {
        UIManager.instance.ShowMessage(message, duration);
    }
}