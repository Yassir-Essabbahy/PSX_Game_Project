using UnityEngine;
using System.Collections;

[System.Serializable]
public class LocationOption
{
    public string locationName;       // shown on button, e.g. "l7anout", "dar jiran"
    public Transform spawnPoint;      // where to teleport the player
}

public class LocationChooser : MonoBehaviour
{
    [Header("Destinations")]
    public LocationOption[] locations;

    [Header("References")]
    public Transform player;          // drag your player here
    public CharacterController characterController; // drag player's CharacterController (needed to teleport)

    [Header("Audio")]
    public AudioClip doorOpenSound;
    public AudioSource audioSource;   // can be on this object or shared

    [Header("Timing")]
    public float fadeOutDuration = 0.5f;
    public float blackScreenHold = 0.3f;
    public float fadeInDuration = 0.5f;

    /// <summary>
    /// Called by Interactable.onInteract (wire in Inspector)
    /// </summary>
    public void ShowChoices()
    {
        // Block player interaction while choosing
        PlayerInteract.isBlocked = true;

        // Build choice names array
        string[] names = new string[locations.Length];
        for (int i = 0; i < locations.Length; i++)
        {
            names[i] = locations[i].locationName;
        }

        // Show choice UI — UIManager handles button creation
        UIManager.instance.ShowChoicePanel(names, OnLocationChosen);
    }

    private void OnLocationChosen(int index)
    {
        UIManager.instance.HideChoicePanel();
        StartCoroutine(TravelRoutine(locations[index].spawnPoint));
    }

    private IEnumerator TravelRoutine(Transform destination)
    {
        // Fade to black
        yield return UIManager.instance.FadeOut(fadeOutDuration);

        // Play door open sound during black screen
        if (doorOpenSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }

        // Hold black screen briefly
        yield return new WaitForSeconds(blackScreenHold);

        // Teleport player
        // Disable CharacterController to allow position change
        if (characterController != null)
            characterController.enabled = false;

        player.position = destination.position;
        player.rotation = destination.rotation;

        if (characterController != null)
            characterController.enabled = true;

        // Fade back in
        yield return UIManager.instance.FadeIn(fadeInDuration);

        // Unblock player
        PlayerInteract.isBlocked = false;
    }  
}
