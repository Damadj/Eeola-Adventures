using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public PlayerData MyPlayerData { get; set; }
    public InventoryData MyInventoryData { get; set; }
    public List<EquipmentData> MyEquipmentData { get; set; }
    public List<ActionButtonData> MyActionButtonData { get; set; }
    public List<PotionButtonData> MyPotionButtonData { get; set; }
    public List<QuestData> MyQuestData { get; set; }
    public List<QuestGiverData> MyQuestGiverData { get; set; }
    public DateTime MyDateTime { get; set; }
    public string MyScene { get; set; }

    public SaveData()
    {
        MyInventoryData = new InventoryData();
        MyEquipmentData = new List<EquipmentData>();
        MyActionButtonData = new List<ActionButtonData>();
        MyPotionButtonData = new List<PotionButtonData>();
        MyQuestData = new List<QuestData>();
        MyQuestGiverData = new List<QuestGiverData>();
        MyDateTime = DateTime.Now;
    }
}

[Serializable]
public class PlayerData
{
    public int MyLevel { get; set; }
    public int MyIntelligence { get; set; }
    public int MyStrength { get; set; }
    public int MyAgility { get; set; }
    public int MyGold { get; set; }
    public float MyExp { get; set; }
    public float MyMaxExp { get; set; }
    public float MyLife { get; set; }
    public float MyMaxLife { get; set; }
    public float MyMana { get; set; }
    public float MyMaxMana { get; set; }
    public float MyPositionX { get; set; }
    public float MyPositionY { get; set; }
    public float MyManaRegeneration { get; set; }
    public int MyDefence { get; set; }

    public PlayerData(int level, float exp, float maxExp, float life, float maxLife, float mana,
        float maxMana, Vector2 position, int strength, int intelligence, int agility, int gold,
        float manaRegeneration, int defence)
    {
        MyLevel = level;
        MyIntelligence = intelligence;
        MyStrength = strength;
        MyAgility = agility;
        MyGold = gold;
        MyExp = exp;
        MyMaxExp = maxExp;
        MyLife = life;
        MyMaxLife = maxLife;
        MyMana = mana;
        MyMaxMana = maxMana;
        MyPositionX = position.x;
        MyPositionY = position.y;
        MyManaRegeneration = manaRegeneration;
        MyDefence = defence;
    }
}

[Serializable]
public class ItemData
{
    public string MyTitle { get; set; }
    public int MyStackCount { get; set; }
    public int MySlotIndex { get; set; }

    public ItemData(string title, int slotIndex = 0, int stackCount = 0)
    {
        MyTitle = title;
        MySlotIndex = slotIndex;
        MyStackCount = stackCount;
    }
}

[Serializable]
public class InventoryData
{
    public List<ItemData> MyItems { get; set; }

    public InventoryData()
    {
        MyItems = new List<ItemData>();
    }
}

[Serializable]
public class EquipmentData
{
    public string MyTitle { get; set; }
    public string MyType { get; set; }

    public EquipmentData(string title, string type)
    {
        MyTitle = title;
        MyType = type;
    }
}

[Serializable]
public class ActionButtonData
{
    public string MySpell { get; set; }
    public int MyIndex { get; set; }

    public ActionButtonData(string spell, int index)
    {
        MySpell = spell;
        MyIndex = index;
    }
}

[Serializable]
public class PotionButtonData
{
    public string MyPotion { get; set; }
    public int MyIndex { get; set; }
    
    public PotionButtonData(string potion, int index)
    {
        MyPotion = potion;
        MyIndex = index;
    }
}

[Serializable]
public class QuestData
{
    public string MyTitle { get; set; }
    public string MyDescription { get; set; }
    public CollectObjective[] MyCollectObjectives { get; set; }
    public KillObjective[] MyKillObjectives { get; set; }
    public int MyQuestGiverID { get; set; }

    public QuestData(string title, string description, CollectObjective[] collectObjectives, KillObjective[] killObjectives, int questGiverID)
    {
        MyTitle = title;
        MyDescription = description;
        MyCollectObjectives = collectObjectives;
        MyKillObjectives = killObjectives;
        MyQuestGiverID = questGiverID;
    }
}

[Serializable]
public class QuestGiverData
{
    public List<string> MyCompletedQuests { get; set; }
    public int MyQuestGiverID { get; set; }

    public QuestGiverData(List<string> completedQuests, int questGiverID)
    {
        MyCompletedQuests = completedQuests;
        MyQuestGiverID = questGiverID;
    }
}