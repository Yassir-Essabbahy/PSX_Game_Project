using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public Light[] lights;
    public float minOnTime = 0.05f;
    public float maxOnTime = 0.3f;
    public float minOffTime = 0.05f;
    public float maxOffTime = 0.2f;
    public bool flicking = false;

    void Start()
    {
        if (flicking)
            StartFlicker();
    }

    public void StartFlicker()
    {
        flicking = true;
        StartCoroutine(Flicker());
    }

    public void StopFlicker()
    {
        flicking = false;
        SetLights(true);
    }

    System.Collections.IEnumerator Flicker()
    {
        while (flicking)
        {
            SetLights(false);
            yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));
            SetLights(true);
            yield return new WaitForSeconds(Random.Range(minOnTime, maxOnTime));
        }
    }

    void SetLights(bool on)
    {
        foreach (Light l in lights)
            if (l) l.enabled = on;
    }
}