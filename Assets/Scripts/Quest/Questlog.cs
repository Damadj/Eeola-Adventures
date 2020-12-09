using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questlog : MonoBehaviour
{
    private static Questlog _instance;
    public static Questlog MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<Questlog>();
            return _instance;
        }
    }
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questParent;
    public CanvasGroup MyCanvasGroup { get; set; }
    [SerializeField] private Text questCountText;
    [SerializeField] private int maxCount;        
    [SerializeField] private Text questDescription;
    private int currentCount;
    private Quest selectedQuest;
    private List<QuestScript> questScripts = new List<QuestScript>();
    private List<Quest> quests = new List<Quest>();
    public List<Quest> MyQuests { get => quests; set => quests = value; }

    private void Awake()
    {
        MyCanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        questCountText.text = currentCount + "/" + maxCount;
    }

    public void AcceptQuest(Quest quest)
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            questCountText.text = currentCount + "/" + maxCount;
            foreach (var objective in quest.MyCollectObjectives)
            {
                InventoryScript.MyInstance.itemCountChangeEvent += objective.UpdateItemCount;
                objective.UpdateItemCount();
            }

            foreach (var objective in quest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmedEvent += objective.UpdateKillCount;
            }

            quests.Add(quest);
            var go = Instantiate(questPrefab, questParent);
            var qs = go.GetComponent<QuestScript>();
            go.GetComponent<Text>().text = quest.MyTitle;
            quest.MyQuestScript = qs;
            qs.MyQuest = quest;
            questScripts.Add(qs);
            CheckCompletion();
        }
    }

    public void ShowDescription(Quest quest)
    {
        if (quest != null)
        {
            if (selectedQuest != null &&  selectedQuest != quest)
            {
                selectedQuest.MyQuestScript.DeSelect();
            }

            var objectives = "\n\n";
            selectedQuest = quest;
            foreach (var obj in quest.MyCollectObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }
            foreach (var obj in quest.MyKillObjectives)
            {
                objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
            }

            questDescription.text = string.Format("<size=15><b>{0}</b></size>\n\n{1}{2}", selectedQuest.MyTitle,
                selectedQuest.MyDescription, objectives);
        }
    }

    public void UpdateSelected()
    {
        ShowDescription(selectedQuest);
    }

    public void CheckCompletion()
    {
        foreach (var qs in questScripts)
        {
            qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
            qs.IsComplete();
        }
    }

    public void OpenClose()
    {
        if (Mathf.Approximately(MyCanvasGroup.alpha, 1)) Close();
        else Open();
    }
    public void Close()
    {
        MyCanvasGroup.alpha = 0;
        MyCanvasGroup.blocksRaycasts = false;
    }
    public void Open()
    {
        UIManager.MyInstance.CloseLeftSide(this);
        MyCanvasGroup.alpha = 1;
        MyCanvasGroup.blocksRaycasts = true;
    }

    public void AbandonQuest()
    {
        foreach (var objective in selectedQuest.MyCollectObjectives)
        {
            InventoryScript.MyInstance.itemCountChangeEvent -= objective.UpdateItemCount;
        }
        foreach (var objective in selectedQuest.MyKillObjectives)
        {
            GameManager.MyInstance.killConfirmedEvent -= objective.UpdateKillCount;
        }
        RemoveQuest(selectedQuest.MyQuestScript);
    }

    public void RemoveQuest(QuestScript qs)
    {
        questScripts.Remove(qs);
        Destroy(qs.gameObject);
        quests.Remove(qs.MyQuest);
        questDescription.text = string.Empty;
        selectedQuest = null;
        currentCount--;
        questCountText.text = currentCount + "/" + maxCount;
        qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
        qs = null;
    }

    public bool HasQuest(Quest quest)
    {
        return quests.Exists(x => x.MyTitle == quest.MyTitle);
    }
}
