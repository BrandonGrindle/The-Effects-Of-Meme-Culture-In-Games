using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatQuestStep : QuestStep
{
    private int SkeletonsKilled = 0;
    private int CompletionCount = 5;

    private void OnEnable()
    {
        EventManager.Instance.cstmevents.onSkeletonKilled += SkeletonKilled;
    }

    private void OnDisable()
    {
        EventManager.Instance.cstmevents.onSkeletonKilled -= SkeletonKilled;
    }

    private void SkeletonKilled()
    {
        if (SkeletonsKilled < CompletionCount)
        {
            SkeletonsKilled++;
        }

        if (SkeletonsKilled >= CompletionCount)
        {
            FinishStep();
        }
    }
}
