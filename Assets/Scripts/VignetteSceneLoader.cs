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
    public float delayBeforeVignette = 5f;
    public float vignetteDuration = 2f;
    public float fadeDuration = 1f;

    [Header("Vignette")]
    public Volume volume;

    private Vignette vignette;

    void Start()
    {
        if (volume.profile.TryGet(out vignette))
            vignette.intensity.value = 0f;

        StartCoroutine(VignetteAndLoad());
    }

    private IEnumerator VignetteAndLoad()
    {
        // Wait before vignette starts
        yield return new WaitForSeconds(delayBeforeVignette);

        // Grow vignette intensity from 0 to 1
        float elapsed = 0f;
        while (elapsed < vignetteDuration)
        {
            elapsed += Time.deltaTime;
            if (vignette != null)
                vignette.intensity.value = Mathf.Lerp(0f, 1f, elapsed / vignetteDuration);
            yield return null;
        }

        // Fade to black
        if (UIManager.instance != null && UIManager.instance.fadeCanvasGroup != null)
            yield return UIManager.instance.FadeOut(fadeDuration);

        SceneManager.LoadScene(sceneName);
    }
}