using UnityEngine;
using System.Collections;

public class DreamSequence : MonoBehaviour
{
    [Header("NPCs in order")]
    public DialogueTrigger[] npcsInOrder;

    private int currentIndex = 0;

    // كتسميها من AnimationEvent فآخر frame
    public void StartSequence()
    {
        currentIndex = 0;
        TalkNext();
    }

    void TalkNext()
    {
        if (currentIndex >= npcsInOrder.Length) return;
        npcsInOrder[currentIndex].Talk();
    }

    // كتسميها من كل DialogueTrigger إيلا خلص
    public void OnDialogueEnd()
    {
        currentIndex++;
        if (currentIndex < npcsInOrder.Length)
            TalkNext();
    }
}