using UnityEngine;
using TMPro;
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
        if (messagePanel) dialoguePanel.SetActive(false);

    }

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

    private IEnumerator ShowMessageRoutine(string message, float duration)
    {
        messagePanel.SetActive(true);
        messageText.text = message;
        
        yield return new WaitForSeconds(duration);
        messagePanel.SetActive(false);
    }

    public void ShowMessage(string message, float duration = 2f)
{
    StopAllCoroutines();
    StartCoroutine(ShowMessageRoutine(message, duration));
}
}
