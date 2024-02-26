using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishQuestStep : QuestStep
{
    private int fishCollected = 0;
    private int CompletionCount = 5;

    private void OnEnable()
    {
        //write fish collectin condition here
    }


    private void OnDisable() 
    { 
        //write disable condition here
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
