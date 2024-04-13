using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEditor.PackageManager;
using Unity.VisualScripting;
using static Items;
using static UnityEditor.Progress;

public class QuestGiver : MonoBehaviour, IInteractable
{
    [Header("Quest")]
    [SerializeField] private QuestInfo CurrentQuest;

    [Header("UI QUEST")]
    [SerializeField] private GameObject QuestUI;

    [Header("Quest Item Type")]
    [SerializeField] private Items KeyItem;
    [SerializeField] private ItemType Type;

    private string QuestID;
    private QuestState currentQuestState;

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

    private void QuestStateChange(Quests quests) 
    {
        if (quests.QuestData.id.Equals(QuestID))
        {
            currentQuestState = quests.currentProgression;
            icons.SetState(currentQuestState);
            Debug.Log("Quest with id: " +  QuestID + " updated to state " + currentQuestState);
        }
    }

    public void ItemSubmissionCheck()
    {
        //Debug.Log("item submission detected");
        if (InventoryManager.Instance.HasQuestItem(Type))
        {
            foreach(Items item in InventoryManager.Instance.items)
            {
                if (item.itemType == Type)
                {
                    InventoryManager.Instance.RemoveItem(item);
                }
            }
        }
    }

    public void UIupdate()
    {
        //implement a ui update for fish submitted
    }

    public void Interact()
    {
        //Debug.Log("Hello");
        if (currentQuestState.Equals(QuestState.CAN_START))
        {
            if(KeyItem != null)
            {
                InventoryManager.Instance.AddItem(KeyItem);
            }
            EventManager.Instance.questEvents.StartQuest(QuestID);
        }
        else if (currentQuestState.Equals(QuestState.COMPLETE))
        {
            EventManager.Instance.questEvents.QuestComplete(QuestID);
            ItemSubmissionCheck();
        }
        else
        {
            ItemSubmissionCheck();
            UIupdate();
        }
        
        
    }
}
