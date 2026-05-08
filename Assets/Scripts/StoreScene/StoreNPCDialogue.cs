using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class DialogueStep
{
    public string speakerName;
    [TextArea(2, 5)]
    public string line;

    public AudioClip voiceClip;
    public string animationTrigger;
    public AudioClip jumpscareSound;
}

public class StoreNPCDialogue : MonoBehaviour
{
    [Header("Dialogue Steps")]
    public DialogueStep[] steps;

    [Header("References")]
    public Animator npcAnimator;
    public AudioSource voiceSource;
    public AudioSource jumpscareSource;

    [Header("Settings")]
    public float silenceBeforeReturn = 3f;

    private int index = 0;
    private bool isActive = false;
    private bool hasTalked = false;

    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.E))
        {
            NextStep();
        }
    }

    public void StartConversation()
    {
        // Prevent talking more than once
        if (isActive || hasTalked)
            return;

        hasTalked = true;

        index = 0;
        isActive = true;

        PlayerInteract.isBlocked = true;

        // Disable interactable
        Interactable interactable = GetComponent<Interactable>();
        if (interactable != null)
        {
            interactable.enabled = false;
        }

        // Start first dialogue
        if (steps.Length > 0)
        {
            PlayStep(steps[index]);
            index++;
        }
    }

    void NextStep()
    {
        if (index >= steps.Length)
        {
            EndConversation();
            return;
        }

        PlayStep(steps[index]);
        index++;
    }

    void PlayStep(DialogueStep step)
    {
        // Show dialogue
        if (!string.IsNullOrEmpty(step.line))
        {
            UIManager.instance.ShowDialogue(step.line, step.speakerName);
        }

        // Play voice
        if (step.voiceClip != null)
        {
            voiceSource.Stop();
            voiceSource.PlayOneShot(step.voiceClip);
        }

        // Play animation
        if (!string.IsNullOrEmpty(step.animationTrigger) && npcAnimator != null)
        {
            npcAnimator.SetTrigger(step.animationTrigger);
        }

        // Play jumpscare sound
        if (step.jumpscareSound != null)
        {
            jumpscareSource.PlayOneShot(step.jumpscareSound);
        }
    }

    void EndConversation()
    {
        isActive = false;

        PlayerInteract.isBlocked = false;

        UIManager.instance.HideDialogue();

        StartCoroutine(ReturnToHouse());
    }

    IEnumerator ReturnToHouse()
    {
        yield return new WaitForSeconds(silenceBeforeReturn);

        SceneManager.LoadScene("HouseScene");
    }
} 