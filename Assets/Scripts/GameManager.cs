using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Phase")]
    public int phase = 0;

    [Header("Objectives UI")]
    public ObjectiveUI objectiveUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        SetPhase(phase);
    }

    public void SetPhase(int newPhase)
    {
        phase = newPhase;

        switch (phase)
        {
            case 0: SetupPhase0(); break;
            case 1: SetupPhase1(); break;
            case 2: SetupPhase2(); break;
        }
    }

    void SetupPhase0()
    {
        if (objectiveUI != null)
            objectiveUI.SetObjective("Hdr m3a mama");
    }

    void SetupPhase1()
    {
        if (objectiveUI != null)
            objectiveUI.SetObjective("Sir L7anot");
    }

    void SetupPhase2() 
    {
        if (objectiveUI != null)
            objectiveUI.SetObjective("شكون فالدار؟");

        GameObject mama = GameObject.Find("Mama");
        GameObject baba = GameObject.Find("Baba");

        if (mama) mama.SetActive(false);
        if (baba) baba.SetActive(false);
    }

    public void OnMamaTalked() 
    {
        if (phase != 0) return;
        SetPhase(1);
    }

    public void OnStoreComplete()
    {
        SetPhase(2);
    }
}