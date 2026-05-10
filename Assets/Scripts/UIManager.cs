using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerName;

    [Header("Message UI")]
    public GameObject messagePanel;
    public TextMeshProUGUI messageText;

    [Header("Fade UI")]
    public CanvasGroup fadeCanvasGroup;  // full-screen black Image with CanvasGroup

    [Header("Choice Panel UI")]
    public GameObject choicePanel;       // parent panel that holds buttons
    public Button[] choiceButtons;       // pre-made buttons (create 4-5 in Inspector)
    public TextMeshProUGUI[] choiceButtonTexts; // TMP text on each button

    private Action<int> onChoiceSelected;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (dialoguePanel) dialoguePanel.SetActive(false);
        if (messagePanel) messagePanel.SetActive(false);
        if (choicePanel) choicePanel.SetActive(false);

        // Start fully transparent (visible scene)
        if (fadeCanvasGroup)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.gameObject.SetActive(true);
        }
    }

    // ═══════════════════════════════════════════
    // DIALOGUE
    // ═══════════════════════════════════════════
    public void ShowDialogue(string line, string speaker)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = line;
        if (speakerName != null)
        {
            speakerName.text = speaker;
        }
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    // ═══════════════════════════════════════════
    // MESSAGE (auto-hide)
    // ═══════════════════════════════════════════
    public void ShowMessage(string message, float duration = 2f)
    {
        StopCoroutine(nameof(ShowMessageRoutine));
        StartCoroutine(ShowMessageRoutine(message, duration));
    }

    private IEnumerator ShowMessageRoutine(string message, float duration)
    {
        messagePanel.SetActive(true);
        messageText.text = message;
        yield return new WaitForSeconds(duration);
        messagePanel.SetActive(false);
    }

    // ═══════════════════════════════════════════
    // FADE (black screen)
    // ═══════════════════════════════════════════
    public Coroutine FadeOut(float duration)
    {
        return StartCoroutine(FadeRoutine(0f, 1f, duration));
    }

    public Coroutine FadeIn(float duration)
    {
        return StartCoroutine(FadeRoutine(1f, 0f, duration));
    }

    private IEnumerator FadeRoutine(float from, float to, float duration)
    {
        fadeCanvasGroup.blocksRaycasts = true;
        float elapsed = 0f;
        fadeCanvasGroup.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        fadeCanvasGroup.alpha = to;

        // Only block raycasts when fully black
        fadeCanvasGroup.blocksRaycasts = (to >= 1f);
    }

    public void ShowChoicePanel(string[] options, Action<int> callback)
    {
        onChoiceSelected = callback;
        choicePanel.SetActive(true);

        // Show/hide buttons based on how many options
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < options.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtonTexts[i].text = options[i];

                // Wire button click
                int index = i; // capture for closure
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => OnChoiceButtonClicked(index));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }

        // Unlock cursor for clicking buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void HideChoicePanel()
    {
        choicePanel.SetActive(false);

        // Re-lock cursor for FPS gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnChoiceButtonClicked(int index)
    {
        onChoiceSelected?.Invoke(index);
    } 
}
