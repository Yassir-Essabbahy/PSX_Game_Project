using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LightsTrigger : MonoBehaviour
{
    [Header("Lights")]
    public Light[] lightsToTurnOff;
    public Light[] secondLights;

    [Header("Scene")]
    public string nextScene;

    [Header("Timing")]
    public float hideMessageDuration = 3f;
    public float timerDuration = 30f;
    public float breathingDuration = 3f;
    public float fadeDuration = 1f;
    public float knockingDelay = 2f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource knockingAudioSource;
    public AudioClip lightsOffSound;
    public AudioClip breathingSound;
    public AudioClip doorKnockingSound;

    public static LightsTrigger instance;
    private bool triggered = false;
    private bool sequenceStarted = false;
    private Coroutine timerCoroutine;

    void Awake()
    {
        instance = this;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (triggered) return;

        triggered = true;
        StartCoroutine(LightsOffSequence());
    }

    private IEnumerator LightsOffSequence()
    {
        if (audioSource != null && lightsOffSound != null)
            audioSource.PlayOneShot(lightsOffSound);

        foreach (Light l in lightsToTurnOff)
            if (l != null) l.enabled = false;

        foreach (Light l in secondLights)
            if (l != null) l.enabled = true;

        UIManager.instance.ShowMessage("Hide!", hideMessageDuration);

        yield return new WaitForSeconds(knockingDelay);
        if (knockingAudioSource != null && doorKnockingSound != null)
        {
            knockingAudioSource.clip = doorKnockingSound;
            knockingAudioSource.loop = true;
            knockingAudioSource.Play();
        }

        timerCoroutine = StartCoroutine(CountdownTimer());
        yield return timerCoroutine;

        yield return StartCoroutine(EndingSequence());
    }

    private IEnumerator CountdownTimer()
    {
        float remaining = timerDuration;
        while (remaining > 0f)
        {
            remaining -= Time.deltaTime;
            UIManager.instance.ShowMessage("Hide! " + Mathf.CeilToInt(remaining) + "s", 0.2f);
            yield return null;
        }
    }

    public void TriggerEndingEarly()
    {
        if (sequenceStarted) return;
        if (timerCoroutine != null) StopCoroutine(timerCoroutine);
        StartCoroutine(EndingSequence());
    }

    public IEnumerator EndingSequence()
    {
        sequenceStarted = true;

        // Stop radio
        if (Radio.instance != null)
            Radio.instance.audioSource.Stop();

        // Stop knocking
        if (knockingAudioSource != null)
            knockingAudioSource.Stop();

        if (audioSource != null && breathingSound != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(breathingSound);
        }

        yield return new WaitForSeconds(breathingDuration);

        if (UIManager.instance != null && UIManager.instance.fadeCanvasGroup != null)
            yield return UIManager.instance.FadeOut(fadeDuration);

        SceneManager.LoadScene(nextScene);
    }
}