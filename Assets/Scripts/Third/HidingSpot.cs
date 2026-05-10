using UnityEngine;
using System.Collections;

public enum HideType { Cabinet, UnderBed }

public class HidingSpot : MonoBehaviour
{
    [Header("Type")]
    public HideType hideType;

    [Header("References")]
    public MonoBehaviour playerController;
    public Transform hidingCameraPosition;
    public float transitionSpeed = 3f;

    [Header("Cabinet Doors (only if Cabinet)")]
    public Transform leftDoor;
    public Transform rightDoor;
    public float doorOpenAngle = 90f;
    public float doorSpeed = 3f;

    private bool isHiding = false;
    private Transform playerCamera;

    private Quaternion leftClosed, leftOpen;
    private Quaternion rightClosed, rightOpen;

    void Start()
    {
        playerCamera = Camera.main.transform;

        if (leftDoor)
        {
            leftClosed = leftDoor.localRotation;
            leftOpen = leftClosed * Quaternion.Euler(0, 0, doorOpenAngle);
        }

        if (rightDoor)
        {
            rightClosed = rightDoor.localRotation;
            rightOpen = rightClosed * Quaternion.Euler(0, 0, -doorOpenAngle);

        }
    }

    public void Hide()
    {
        if (hideType == HideType.Cabinet)
        {
            if (!isHiding) StartCoroutine(HideCabinet());
            else StartCoroutine(UnhideCabinet());
        }
        else
        {
            if (!isHiding) StartCoroutine(HideUnderBed());
            else StartCoroutine(UnhideUnderBed());
        }
    }

    // ── CABINET ───────────────────────────────────────────

    IEnumerator HideCabinet()
    {
        isHiding = true;
        PlayerInteract.isBlocked = true;
        playerController.enabled = false;

        yield return StartCoroutine(RotateDoors(true));
        yield return StartCoroutine(MoveCamera(hidingCameraPosition.position, hidingCameraPosition.rotation));
        yield return StartCoroutine(RotateDoors(false));

        PlayerInteract.isBlocked = false;
    }

    IEnumerator UnhideCabinet()
    {
        isHiding = false;
        PlayerInteract.isBlocked = true;

        yield return StartCoroutine(RotateDoors(true));

        Vector3 returnPos = playerController.transform.position + Vector3.up * 1.7f;
        yield return StartCoroutine(MoveCamera(returnPos, playerController.transform.rotation));

        yield return StartCoroutine(RotateDoors(false));

        playerController.enabled = true;
        PlayerInteract.isBlocked = false;
    }

    IEnumerator RotateDoors(bool open)
    {
        float t = 0f;
        Quaternion leftTarget = open ? leftOpen : leftClosed;
        Quaternion rightTarget = open ? rightOpen : rightClosed;

        while (t < 1f)
        {
            t += Time.deltaTime * doorSpeed;
            if (leftDoor) leftDoor.localRotation = Quaternion.Slerp(leftDoor.localRotation, leftTarget, t);
            if (rightDoor) rightDoor.localRotation = Quaternion.Slerp(rightDoor.localRotation, rightTarget, t);
            yield return null;
        }
    }

    // ── UNDER BED ─────────────────────────────────────────

    IEnumerator HideUnderBed()
    {
        isHiding = true;
        PlayerInteract.isBlocked = true;
        playerController.enabled = false;

        yield return StartCoroutine(MoveCamera(hidingCameraPosition.position, hidingCameraPosition.rotation));

        PlayerInteract.isBlocked = false;
    }

    IEnumerator UnhideUnderBed()
    {
        isHiding = false;
        PlayerInteract.isBlocked = true;

        Vector3 returnPos = playerController.transform.position + Vector3.up * 1.7f;
        yield return StartCoroutine(MoveCamera(returnPos, playerController.transform.rotation));

        playerController.enabled = true;
        PlayerInteract.isBlocked = false;
    }

    // ── SHARED CAMERA MOVE ────────────────────────────────

    IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot)
    {
        float t = 0f;
        Vector3 startPos = playerCamera.position;
        Quaternion startRot = playerCamera.rotation;

        while (t < 1f)
        {
            t += Time.deltaTime * transitionSpeed;
            playerCamera.position = Vector3.Lerp(startPos, targetPos, t);
            playerCamera.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }
    }
}