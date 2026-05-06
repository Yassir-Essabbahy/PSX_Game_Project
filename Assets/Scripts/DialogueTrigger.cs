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
            NextLine();
    }

    public void Talk() // called by Interactable — starts dialogue only
    {
        if (isActive) return; // already open, do nothing
        index = 0;
        isActive = true;
        PlayerInteract.isBlocked = true;
        UIManager.instance.ShowDialogue(lines[index], speakerName);
        index++;
    }

    void NextLine() // called by Update — advances dialogue
    {
        if (index >= lines.Length)
        {
            index = 0;
            isActive = false;
            PlayerInteract.isBlocked = false;
            UIManager.instance.HideDialogue();
            return;
        }

        UIManager.instance.ShowDialogue(lines[index], speakerName);
        index++;
    }
}