using UnityEngine;
using System.Collections;

public class DoorToHome : MonoBehaviour
{
    public CharacterController characterController;

    public Transform player;
    public Transform spawnPoint;
    public CanvasGroup fadeScreen;
    public float fadeDuration = 1f;
    public AudioClip doorSound;
    public AudioSource audioSource;

    void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
        StartCoroutine(Teleport());
}

IEnumerator Teleport()
{
    yield return StartCoroutine(Fade(0, 1));

    if (doorSound != null && audioSource != null)
        audioSource.PlayOneShot(doorSound);

    characterController.enabled = false;
    player.position = spawnPoint.position;
    characterController.enabled = true;

    yield return StartCoroutine(Fade(1, 0));
}

    IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        fadeScreen.gameObject.SetActive(true);

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            fadeScreen.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        fadeScreen.alpha = to;
        if (to == 0) fadeScreen.gameObject.SetActive(false);
    }
}