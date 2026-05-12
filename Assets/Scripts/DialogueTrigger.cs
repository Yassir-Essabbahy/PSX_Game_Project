using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public string[] lines;
    public AudioClip[] voiceClips;
    public string speakerName;
    public bool isMama = false;

    [Header("Choice")]
    public bool hasChoice = false;
    public int choiceLineIndex = 2;
    public string choiceA = "Ah";
    public string choiceB = "La";

    [Header("Audio")]
    public AudioSource audioSource;

    private int index = 0;
    private bool isActive = false;
    private bool waitingForChoice = false;
    private bool justStarted = false;

    void Update()
    {
        if (justStarted)
        {
            justStarted = false;
            return;
        }

        if (isActive && !waitingForChoice && Input.GetKeyDown(KeyCode.E))
            NextLine();
    }

    public void Talk()
    {
        if (isActive) return;
        index = 0;
        isActive = true;
        justStarted = true;
        PlayerInteract.isBlocked = true;

        // Unlock cursor when dialogue starts
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (UIManager.instance.playerLookScript)
            UIManager.instance.playerLookScript.enabled = false;

        GetComponent<NPCLookAt>().StartLooking();
        ShowLine(index);
        index++;
    }

    void NextLine()
    {
        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        if (hasChoice && index == choiceLineIndex)
        {
            ShowLine(index);
            index++;
            ShowChoices();
            return;
        }

        ShowLine(index);
        index++;
    }

    void ShowLine(int i)
    {
        UIManager.instance.ShowDialogue(lines[i], speakerName);
        if (voiceClips != null && i < voiceClips.Length && voiceClips[i] != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(voiceClips[i]);
        }
    }

    void ShowChoices()
    {
        waitingForChoice = true;
        // UIManager.ShowChoicePanel already handles cursor + playerLookScript
        UIManager.instance.ShowChoicePanel(new string[] { choiceA, choiceB }, OnChoiceMade);
    }

    void OnChoiceMade(int choiceIndex)
    {
        waitingForChoice = false;
        UIManager.instance.HideChoicePanel();
        NextLine();
    }

    void EndDialogue()
    {
        index = 0;
        isActive = false;
        waitingForChoice = false;
        PlayerInteract.isBlocked = false;

        // Lock cursor back when dialogue ends
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (UIManager.instance.playerLookScript)
            UIManager.instance.playerLookScript.enabled = true;

        GetComponent<NPCLookAt>().StopLooking();
        UIManager.instance.HideDialogue();
        if (isMama)
            GameManager.instance.OnMamaTalked();
    }
}