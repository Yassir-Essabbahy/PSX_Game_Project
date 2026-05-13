using UnityEngine;
using System.Collections;

public class Radio : MonoBehaviour
{
    [Header("Songs")]
    public AudioClip[] songs;
    public AudioClip staticSound;

    [Header("Settings")]
    public float staticDuration = 1f;

    [Header("References")]
    public AudioSource audioSource;

    public static Radio instance;
    private int currentSongIndex = 0;
    private bool isChanging = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    public void Interact()
    {
        if (isChanging) return;

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            return;
        }

        StartCoroutine(ChangeSong());
    }

    private IEnumerator ChangeSong()
    {
        isChanging = true;

        if (staticSound != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(staticSound);
            yield return new WaitForSeconds(staticDuration);
        }

        currentSongIndex = (currentSongIndex + 1) % songs.Length;

        if (songs[currentSongIndex] != null)
        {
            audioSource.clip = songs[currentSongIndex];
            audioSource.loop = true;
            audioSource.Play();
        }

        isChanging = false;
    }
}