using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishQuestStep : QuestStep
{
    private int fishCollected = 0;
    private int CompletionCount = 3;

    private void OnEnable()
    {
        EventManager.Instance.cstmevents.onFishCollected += FishCollected;
    }

    private void OnDisable() 
    {
        EventManager.Instance.cstmevents.onFishCollected -= FishCollected;
    } 

    private void FishCollected()
    {
        if (fishCollected < CompletionCount) 
        {
            fishCollected++;
        }

        if (fishCollected >= CompletionCount) 
        { 
            FinishStep();
        }
    }
}
