using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    public string[] lines;
    public string speakerName;
    public bool isMama = false;

    [Header("Choice")]
    public bool hasChoice = false;
    public int choiceLineIndex = 2;
    public string choiceA = "Ah";
    public string choiceB = "La";

    [Header("Typing Sound")]
    public AudioClip typingTrack;
    public AudioSource audioSource;
    [Range(0f, 1f)] public float typingVolume = 0.8f;

    private int index = 0;
    private bool isActive = false;
    private bool waitingForChoice = false;
    private bool justStarted = false;
    private bool hasSpoken = false;

    void Update()
    {
        if (justStarted)
        {
            justStarted = false;
            return;
        }

        if (isActive && !waitingForChoice && Input.GetKeyDown(KeyCode.E))
        {
            NextLine();
        }
    }

    public void StartVoice()
    {
        if (typingTrack == null || audioSource == null)
            return;

        audioSource.Stop();
        audioSource.clip = typingTrack;
        audioSource.loop = true;
        audioSource.volume = typingVolume;
        audioSource.Play();
    }

    public void StopVoice()
    {
        if (audioSource == null)
            return;

        audioSource.Stop();
    }

    public void Talk()
    {
        if (isActive || hasSpoken)
            return;

        hasSpoken = true;

        index = 0;
        isActive = true;
        justStarted = true;

        PlayerInteract.isBlocked = true;

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
        UIManager.instance.ShowDialogue(lines[i], speakerName, this);
    }

    void ShowChoices()
    {
        waitingForChoice = true;

        StopVoice();

        UIManager.instance.ShowChoicePanel(
            new string[] { choiceA, choiceB },
            OnChoiceMade
        );
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