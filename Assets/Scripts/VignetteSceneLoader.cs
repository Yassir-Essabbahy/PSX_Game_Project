using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class VignetteSceneLoader : MonoBehaviour
{
    [Header("Scene")]
    public string sceneName;

[Header("Timing")]
public float jumpscareHoldTime = 1.5f;
public float returnToFPSSpeed = 3f;
public float waitAfterReturn = 10f;  // ← جديد
public float heartbeatDuration = 2f;
public float breathingDuration = 2f;
public float vignetteDuration = 2f;
public float fadeDuration = 1f;

[Header("Jumpscare")]
public Transform fpsCamera;
public Transform jumpscareTarget;
public float jumpscareSpeed = 10f;
public GameObject jumpscareNPC;
public MonoBehaviour fpsController;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip jumpscareSound;
    public AudioClip heartbeatSound;
    public AudioClip breathingSound;

    [Header("Vignette")]
    public Volume volume;

    private Vignette vignette;
    private bool triggered = false;

    void Start()
    {
        if (volume.profile.TryGet(out vignette))
            vignette.intensity.value = 0f;

        if (jumpscareNPC != null)
            jumpscareNPC.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(Sequence());
    }

private IEnumerator Sequence()
{
    // اظهر الـNPC
    if (jumpscareNPC != null)
        jumpscareNPC.SetActive(true);

    // ⬇ disable الـFPS controller
    if (fpsController != null)
        fpsController.enabled = false;

    // jumpscare sound
    if (jumpscareSound != null)
        audioSource.PlayOneShot(jumpscareSound);

    // دور الكاميرا لوجهه
    float t = 0f;
    Quaternion startRot = fpsCamera.rotation;
    Quaternion targetRot = Quaternion.LookRotation(
        jumpscareTarget.position - fpsCamera.position
    );

    while (t < 1f)
    {
        t += Time.deltaTime * jumpscareSpeed;
        fpsCamera.rotation = Quaternion.Slerp(startRot, targetRot, t);
        yield return null;
    }

    // ثبت على الوجه
    yield return new WaitForSeconds(jumpscareHoldTime);

    // ارجع للـFPS rotation
    t = 0f;
    Quaternion jumpRot = fpsCamera.rotation;
    while (t < 1f)
    {
        t += Time.deltaTime * returnToFPSSpeed;
        fpsCamera.rotation = Quaternion.Slerp(jumpRot, startRot, t);
        yield return null;
    }

// ⬆ enable الـFPS controller
if (fpsController != null)
    fpsController.enabled = true;

// ← انتظر 10 ثواني
yield return new WaitForSeconds(waitAfterReturn);

// heartbeat
if (heartbeatSound != null)
{
    audioSource.clip = heartbeatSound;
    audioSource.loop = true;
    audioSource.Play();
}
yield return new WaitForSeconds(heartbeatDuration);

// breathing
if (breathingSound != null)
{
    audioSource.Stop();
    audioSource.PlayOneShot(breathingSound);
}
yield return new WaitForSeconds(breathingDuration);

// vignette
audioSource.Stop();
float elapsed = 0f;
while (elapsed < vignetteDuration)
{
    elapsed += Time.deltaTime;
    if (vignette != null)
        vignette.intensity.value = Mathf.Lerp(0f, 1f, elapsed / vignetteDuration);
    yield return null;
}

// fade
if (UIManager.instance != null && UIManager.instance.fadeCanvasGroup != null)
    yield return UIManager.instance.FadeOut(fadeDuration);

SceneManager.LoadScene(sceneName);
}
}