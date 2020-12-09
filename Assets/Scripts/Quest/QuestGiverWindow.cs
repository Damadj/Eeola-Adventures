using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : Window
{
    private static QuestGiverWindow _instance;
    public static QuestGiverWindow MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<QuestGiverWindow>();
            return _instance;
        }
    }  
    private QuestGiver questGiver;
    [SerializeField] private GameObject backBtn, acceptBtn, questDescription, completeBtn;
    [SerializeField] private GameObject questPrefab;
    [SerializeField] Transform questArea;
    private List<GameObject> quests = new List<GameObject>();
    private Quest selectedQuest;

    public void ShowQuests(QuestGiver qG)
    {
        questGiver = qG;
        foreach (var go in quests)
        {
            Destroy(go);
        }
        questArea.gameObject.SetActive(true);
        questDescription.SetActive(false);
        foreach (var quest in questGiver.MyQuests)
        {
            if (quest != null)
            {
                var go = Instantiate(questPrefab, questArea);
                go.GetComponent<Text>().text = "[" + quest.MyLevel + "]" + quest.MyTitle;
                go.GetComponent<QGQuestScript>().MyQuest = quest;
                quests.Add(go);
                if (Questlog.MyInstance.HasQuest(quest) && quest.IsComplete)
                {
                    go.GetComponent<Text>().text += "(Complete)";
                }
                else if (Questlog.MyInstance.HasQuest(quest))
                {
                    Color c = go.GetComponent<Text>().color;
                    c.a = 0.5f;
                    go.GetComponent<Text>().color = c;
                }
            }
        }
    }

    public override void Open(NPC npc)
    {
        ShowQuests(npc as QuestGiver);
        base.Open(npc);
    }

    public void ShowQuestInfo(Quest quest)
    {
        selectedQuest = quest;
        if (Questlog.MyInstance.HasQuest(quest) && quest.IsComplete)
        {
            acceptBtn.SetActive(false);
            completeBtn.SetActive(true);
        }
        else if (!Questlog.MyInstance.HasQuest(quest))
        {
            acceptBtn.SetActive(true);
            // completeBtn.SetActive(false);
        }
        backBtn.SetActive(true);        
        questArea.gameObject.SetActive(false);
        questDescription.SetActive(true);

        var objectives = "\n\n";
        foreach (var obj in quest.MyCollectObjectives)
        {
            objectives += obj.MyType + ": " + obj.MyCurrentAmount + "/" + obj.MyAmount + "\n";
        }

        questDescription.GetComponent<Text>().text = string.Format("<size=22><b>{0}</b></size>\n\n{1}", quest.MyTitle,
            quest.MyDescription);
    }

    public void Back()
    {
        backBtn.SetActive(false);
        acceptBtn.SetActive(false);        
        ShowQuests(questGiver);
        completeBtn.SetActive(false);
    }

    public void Accept()
    {
        Questlog.MyInstance.AcceptQuest(selectedQuest);
        Back();
    }

    public override void Close()
    {
        completeBtn.SetActive(false);
        base.Close();
    }

    public void CompleteQuest()
    {
        if (selectedQuest.IsComplete)
        {
            for (int i = 0; i < questGiver.MyQuests.Length; i++)
            {
                if (selectedQuest == questGiver.MyQuests[i])
                {
                    questGiver.MyCompletedQuests.Add(selectedQuest.MyTitle);
                    questGiver.MyQuests[i] = null;
                    selectedQuest.MyQuestGiver.UpdateQuestStatus();
                }
            }

            foreach (var objective in selectedQuest.MyCollectObjectives)
            {
                InventoryScript.MyInstance.itemCountChangeEvent -= objective.UpdateItemCount;
                objective.Complete();
                
            }
            foreach (var objective in selectedQuest.MyKillObjectives)
            {
                GameManager.MyInstance.killConfirmedEvent -= objective.UpdateKillCount;
            }
            
            var xp = XPManager.CalculateXp(selectedQuest);
            Eeola.MyInstance.GainXp(xp);
            
            Questlog.MyInstance.RemoveQuest(selectedQuest.MyQuestScript);
            Back();
        }
    }
}
