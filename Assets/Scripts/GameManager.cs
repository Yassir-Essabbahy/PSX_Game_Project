using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int phase = 0;
    public ObjectiveUI objectiveUI;

    void Awake()
{
    instance = this;

    #if UNITY_EDITOR
    if (!PlayerPrefs.HasKey("gameStarted"))
    {
        PlayerPrefs.SetInt("phase", 0);
        PlayerPrefs.SetInt("gameStarted", 1);
    }
    #endif

    phase = PlayerPrefs.GetInt("phase", 0);
    SetPhase(phase);
}

void OnApplicationQuit()
{
    PlayerPrefs.DeleteKey("gameStarted");
}

    public void SetPhase(int newPhase)
    {
        phase = newPhase;
        PlayerPrefs.SetInt("phase", phase);

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
            objectiveUI.SetObjective("t1");
    }

    void SetupPhase1()
    {
        if (objectiveUI != null)
            objectiveUI.SetObjective("t2");
    }

    void SetupPhase2()
    {
        if (objectiveUI != null)
            objectiveUI.SetObjective("t3");
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