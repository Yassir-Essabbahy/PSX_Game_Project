using UnityEngine;

public class NPCLookAt : MonoBehaviour
{
    public Transform target; // drag the player here
    public float speed = 3f;
    private float weight = 0f;
    private float targetWeight = 0f;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void StartLooking() => targetWeight = 1f;
    public void StopLooking() => targetWeight = 0f;

    void Update()
    {
        weight = Mathf.Lerp(weight, targetWeight, Time.deltaTime * speed);
    }

    void OnAnimatorIK(int layerIndex)
    {
        anim.SetLookAtWeight(weight);
        anim.SetLookAtPosition(target.position);
    }
} 