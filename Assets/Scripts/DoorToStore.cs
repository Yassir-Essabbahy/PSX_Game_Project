using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorToStore : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip successClip;
    public AudioClip failClip;

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.instance.phase == 1 && GameManager.instance.hasMoney)
        {
            if (audioSource != null && successClip != null)
                audioSource.PlayOneShot(successClip);

            StartCoroutine(FadeAndLoad());
        }
        else if (!GameManager.instance.hasMoney)
        {
            if (audioSource != null && failClip != null)
                audioSource.PlayOneShot(failClip);

            UIManager.instance.ShowMessage("No Money", 2f);
        }
    }

    private IEnumerator FadeAndLoad()
    {
        if (UIManager.instance != null && UIManager.instance.fadeCanvasGroup != null)
            yield return UIManager.instance.FadeOut(fadeDuration);

        SceneManager.LoadScene("StoreWay");
    }
}