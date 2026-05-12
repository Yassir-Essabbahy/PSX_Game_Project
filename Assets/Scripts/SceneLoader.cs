using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene")]
    public string sceneName;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip soundClip;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    [Header("Trigger Settings")]
    public bool useTrigger = false;

    void OnTriggerEnter(Collider other)
    {
        if (!useTrigger) return;
        if (!other.CompareTag("Player")) return;
        StartCoroutine(FadeAndLoad());
    }

    public void LoadScene()
    {
        StartCoroutine(FadeAndLoad());
    }

    private IEnumerator FadeAndLoad()
    {
        if (audioSource != null && soundClip != null)
            audioSource.PlayOneShot(soundClip);

        if (UIManager.instance != null && UIManager.instance.fadeCanvasGroup != null)
            yield return UIManager.instance.FadeOut(fadeDuration);

        SceneManager.LoadScene(sceneName);
    }
}