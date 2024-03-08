using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEditor.PackageManager;
using Unity.VisualScripting;

public class QuestGiver : MonoBehaviour, IInteractable
{
    [Header("Quest")]
    [SerializeField] private QuestInfo CurrentQuest;

    [Header("UI QUEST")]
    [SerializeField] private GameObject QuestUI;

    private string QuestID;
    private QuestState currentQuestState;

    public int FishCollected = 0;

    private QuestIcons icons;
    private void Awake()
    {
        QuestID = CurrentQuest.id;
        icons = GetComponentInChildren<QuestIcons>();
    }

    private void OnEnable()
    {
        EventManager.Instance.questEvents.onQuestStateChange += QuestStateChange;
    }
    private void OnDisable()
    {
        EventManager.Instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void Update()
    {
        if(FishCollected >= 0)
        {

        }
    }
    private void QuestStateChange(Quests quests) 
    {

        if (quests.QuestData.id.Equals(QuestID))
        {
            currentQuestState = quests.currentProgression;
            icons.SetState(currentQuestState);
            //Debug.Log("Quest with id: " +  QuestID + " updated to state " + currentQuestState);
        }
    }
    public void Interact()
    {
        //Debug.Log("Hello");
        if (currentQuestState.Equals(QuestState.CAN_START))
        {
            EventManager.Instance.questEvents.StartQuest(QuestID);
        }
        else if (currentQuestState.Equals(QuestState.COMPLETE))
        {
            EventManager.Instance.questEvents.QuestComplete(QuestID);
        }
        
        
    }
}
