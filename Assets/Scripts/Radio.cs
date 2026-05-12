using UnityEngine;
using System.Collections;

public class Radio : MonoBehaviour
{
    [Header("Songs")]
    public AudioClip[] songs;
    public AudioClip staticSound;

    [Header("Settings")]
    public float staticDuration = 1f;

    private AudioSource audioSource;
    private int currentSongIndex = -1;
    private bool isChanging = false;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        if (isChanging) return;
        StartCoroutine(ChangeSong());
    }

    private IEnumerator ChangeSong()
    {
        isChanging = true;

        // Play static first
        if (staticSound != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(staticSound);
            yield return new WaitForSeconds(staticDuration);
        }

        // Move to next song
        currentSongIndex = (currentSongIndex + 1) % songs.Length;

        // Play song
        if (songs[currentSongIndex] != null)
        {
            audioSource.clip = songs[currentSongIndex];
            audioSource.loop = true;
            audioSource.Play();
        }

        isChanging = false;
    }
}