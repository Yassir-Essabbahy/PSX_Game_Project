using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string[] lines;
    public string speakerName;
    private int index = 0;
    private bool isActive = false;

    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.E))
            Talk();
    }

    public void Talk()
    {
        if (index >= lines.Length)
        {
            index = 0;
            isActive = false;
            PlayerInteract.isBlocked = false;
            UIManager.instance.HideDialogue();
            return;
        }

        isActive = true;
        PlayerInteract.isBlocked = true;
        UIManager.instance.ShowDialogue(lines[index], speakerName);
        index++;
    }
}