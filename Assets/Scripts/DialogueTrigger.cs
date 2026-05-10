using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string[] lines;
    public string speakerName;
    public bool isMama = false; // check this on Mama's object in Inspector
    private int index = 0;
    private bool isActive = false;

    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.E))
            NextLine();
    }

    public void Talk()
    {
        if (isActive) return;
        index = 0;
        isActive = true;
        PlayerInteract.isBlocked = true;
        GetComponent<NPCLookAt>().StartLooking();
        UIManager.instance.ShowDialogue(lines[index], speakerName);
        index++;
    }

    void NextLine()
    {
        if (index >= lines.Length)
        {
            index = 0;
            isActive = false;
            PlayerInteract.isBlocked = false;
            GetComponent<NPCLookAt>().StartLooking();
            UIManager.instance.HideDialogue();

            if (isMama)
                GameManager.instance.OnMamaTalked();

            return;
        }

        UIManager.instance.ShowDialogue(lines[index], speakerName);
        index++;
    }
}