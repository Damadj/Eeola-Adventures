using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : NPC
{
    [SerializeField] private Quest[] quests;
    [SerializeField] private Sprite question, questionOff, exclamation;
    [SerializeField] private SpriteRenderer statusRenderer;
    [SerializeField] private int id;
    private List<string> completedQuests = new List<string>();
    public Quest[] MyQuests { get => quests; set => quests = value; }    
    public int MyId { get => id; }

    public List<string> MyCompletedQuests
    {
        get
        {
            return completedQuests;
        }
        set
        {
            completedQuests = value;
            foreach (var title in completedQuests)
            {
                for (int i = 0; i < quests.Length; i++)
                {
                    if (quests[i] != null && quests[i].MyTitle == title)
                    {
                        quests[i] = null;
                    }
                }
            }
        }
    }

    private void Awake()
    {
        foreach (var quest in quests)
        {
            quest.MyQuestGiver = this;
        }
    }

    public void UpdateQuestStatus()
    {
        foreach (var quest in quests)
        {
            if (quest != null)
            {
                if (quest.IsComplete && Questlog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = question;
                    break;
                }
                else if (!Questlog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = exclamation;
                    break;
                }
                else if (!quest.IsComplete && Questlog.MyInstance.HasQuest(quest))
                {
                    statusRenderer.sprite = questionOff;
                }
            }
            else
            {
                statusRenderer.sprite = null;
            }
        }
    }
    
    public override void Interact()
    {
        UIManager.MyInstance.CloseRightSide(QuestGiverWindow.MyInstance);
        base.Interact();
    }
}
