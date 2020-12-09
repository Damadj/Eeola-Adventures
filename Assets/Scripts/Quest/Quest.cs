using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Quest
{
    [SerializeField] private string title;
    [SerializeField] private string description;
    [SerializeField] private CollectObjective[] collectObjectives;
    [SerializeField] private KillObjective[] killObjectives;
    [SerializeField] private int level;
    [SerializeField] private int xp;
    public QuestScript MyQuestScript { get; set; }
    public QuestGiver MyQuestGiver { get; set; }
    public string MyTitle { get => title; set => title = value; }
    public int MyLevel { get => level; }
    public int MyXp { get => xp; }
    public string MyDescription { get => description; }
    public CollectObjective[] MyCollectObjectives { get => collectObjectives; }
    public KillObjective[] MyKillObjectives { get => killObjectives; set => killObjectives = value; }

    public bool IsComplete
    {
        get
        {
            foreach (var objective in collectObjectives)
            {
                if (!objective.IsComplete) return false;
            }
            foreach (var objective in killObjectives)
            {
                if (!objective.IsComplete) return false;
            }

            return true;
        }
    }
}

[Serializable]
public abstract class Objective
{
    [SerializeField] private int amount;    
    [SerializeField] private string type;
    private int currentAmount;
    public int MyAmount { get => amount; }
    public int MyCurrentAmount { get => currentAmount; set => currentAmount = value; }
    public string MyType { get => type; }

    public bool IsComplete
    {
        get
        {
            return MyCurrentAmount >= MyAmount;
        }
    }
}

[Serializable]
public class CollectObjective : Objective
{
    public void UpdateItemCount(Item item)
    {
        if (string.Equals(MyType, item.MyTitle, StringComparison.CurrentCultureIgnoreCase))
        {
            MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(item.MyTitle);
            if (MyCurrentAmount <= MyAmount)
            {
                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", item.MyTitle, MyCurrentAmount,
                    MyAmount));
            }
            
            Questlog.MyInstance.UpdateSelected();
            Questlog.MyInstance.CheckCompletion();
        }
    }

    public void UpdateItemCount()
    {
        MyCurrentAmount = InventoryScript.MyInstance.GetItemCount(MyType);
        Questlog.MyInstance.UpdateSelected();
        Questlog.MyInstance.CheckCompletion();
    }

    public void Complete()
    {
        var items = InventoryScript.MyInstance.GetItems(MyType, MyAmount);
        foreach (var item in items)
        {
            item.Remove();
        }
    }
}

[Serializable]
public class KillObjective : Objective
{
    public void UpdateKillCount(Character character)
    {
        if (MyType == character.MyType)
        {
            if (MyCurrentAmount < MyAmount)
            {
                MyCurrentAmount++;
                MessageFeedManager.MyInstance.WriteMessage(string.Format("{0}: {1}/{2}", character.MyType,
                    MyCurrentAmount,
                    MyAmount));
                Questlog.MyInstance.UpdateSelected();
                Questlog.MyInstance.CheckCompletion();
            }

            
        }
    }
}
