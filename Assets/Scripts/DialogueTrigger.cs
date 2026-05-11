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
    public GameObject choicePanel;
    public Button choiceButtonA;
    public Button choiceButtonB;
    public TextMeshProUGUI choiceTextA;
    public TextMeshProUGUI choiceTextB;
    public string choiceA = "ايه ماما";
    public string choiceB = "نعم ماما";

    [Header("Audio")]
    public AudioSource audioSource;

    private int index = 0;
    private bool isActive = false;
    private bool waitingForChoice = false;
    private bool justStarted = false; // ← fix for line skip bug

    void Start()
    {
        choicePanel.SetActive(false);
        choiceButtonA.onClick.AddListener(() => OnChoiceMade());
        choiceButtonB.onClick.AddListener(() => OnChoiceMade());
        choiceTextA.text = choiceA;
        choiceTextB.text = choiceB;
    }

    void Update()
    {
        // Skip the same frame Talk() was called so E doesn't instantly advance
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
        justStarted = true; // ← block E this frame
        PlayerInteract.isBlocked = true;
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
        choicePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnChoiceMade()
    {
        waitingForChoice = false;
        choicePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        NextLine();
    }

    void EndDialogue()
    {
        index = 0;
        isActive = false;
        waitingForChoice = false;
        PlayerInteract.isBlocked = false;
        GetComponent<NPCLookAt>().StopLooking();
        UIManager.instance.HideDialogue();
        if (isMama)
            GameManager.instance.OnMamaTalked();
    }
}   