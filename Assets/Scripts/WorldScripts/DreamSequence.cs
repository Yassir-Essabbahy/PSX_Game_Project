using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DreamSequence : MonoBehaviour
{
    [Header("NPCs in order")]
    public DialogueTrigger[] npcsInOrder;

    [Header("Ending")]
    public CanvasGroup fadeOverlay;
    public float waitBeforeFade = 1f;
    public float fadeDuration = 2f;
    public string mainMenuScene = "MainMenu";

    private int currentIndex = 0;
    private bool endingTriggered = false;

    void Start()
{
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}

    public void StartSequence()
    {
        currentIndex = 0;
        TalkNext();
    }

    void TalkNext()
    {
        if (currentIndex >= npcsInOrder.Length) return;
        npcsInOrder[currentIndex].Talk();
    }

    public void OnDialogueEnd()
    {
        currentIndex++;
        if (currentIndex < npcsInOrder.Length)
        {
            TalkNext();
        }
        else
        {
            if (!endingTriggered)
            {
                endingTriggered = true;
                StartCoroutine(FadeToMenu());
            }
        }
    }

    IEnumerator FadeToMenu()
    {
        yield return new WaitForSeconds(waitBeforeFade);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            if (fadeOverlay != null)
                fadeOverlay.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        SceneManager.LoadScene(mainMenuScene);
    }
}