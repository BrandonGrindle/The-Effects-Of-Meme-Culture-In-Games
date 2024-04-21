using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEditor.PackageManager;
using Unity.VisualScripting;
using static Items;
using static UnityEditor.Progress;
using TMPro;
using UnityEngine.UI;


public class QuestGiver : MonoBehaviour, IInteractable
{
    [Header("Quest")]
    [SerializeField] private QuestInfo CurrentQuest;

    [Header("UI QUEST")]
    private TextMeshProUGUI questInfo;
    private TextMeshProUGUI progress;

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
        questInfo = GameObject.Find("Info").GetComponent<TextMeshProUGUI>();
        progress = GameObject.Find("Progress").GetComponent<TextMeshProUGUI>();
        questInfo.text = string.Empty; progress.text = string.Empty;
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
            Debug.Log("Quest with id: " + QuestID + " updated to state " + currentQuestState);
        }
    }

    public void ItemSubmissionCheck()
    {
        //Debug.Log("item submission detected");
        if (InventoryManager.Instance.HasQuestItem(Type))
        {
            foreach (Items item in InventoryManager.Instance.items)
            {
                if (item.itemType == Type)
                {
                    InventoryManager.Instance.RemoveItem(item);
                }
            }
        }
    }

    public void Interact()
    {
        //Debug.Log("Hello");
        if (currentQuestState.Equals(QuestState.CAN_START))
        {
            if (KeyItem != null)
            {
                InventoryManager.Instance.AddItem(KeyItem);
                EventManager.Instance.cstmevents.Dance();
                foreach (GameObject step in CurrentQuest.steps)
                {
                    QuestStep currstep = step.GetComponent<QuestStep>();
                    if (currstep != null)
                    {
                        questInfo.text = currstep.GetDetails();
                    }
                }
            }
            EventManager.Instance.questEvents.StartQuest(QuestID);
        }
        else if (currentQuestState.Equals(QuestState.COMPLETE))
        {
            questInfo.text = string.Empty;
            progress.text = string.Empty;
            EventManager.Instance.questEvents.QuestComplete(QuestID);
            ItemSubmissionCheck();
        }
        else
        {
            ItemSubmissionCheck();
        }


    }
}
