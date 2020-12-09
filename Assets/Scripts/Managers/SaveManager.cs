using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    private CharButton[] equipment;
    [SerializeField] private Item[] items;
    [SerializeField] private ActionButton[] actionButtons;
    [SerializeField] private PotionButton[] potionButtons;
    [SerializeField] private SavedGame[] saveSlots;
    [SerializeField] private GameObject dialogue;
    [SerializeField] private Text dialogueText;
    private SavedGame currentSave;
    private string action;
    private void Awake()
    {
        equipment = FindObjectsOfType<CharButton>();
        foreach (var savedGame in saveSlots)
        {
            ShowSavedFiles(savedGame);
        }
        // if (Eeola.MyInstance != null) Eeola.MyInstance.SetDefaultValues();
    }

    private void Start()
    {
        // if (UIManager.MyInstance.MyCinemachines.Length > 2) UIManager.MyInstance.MyCinemachines[2].SetActive(false);
        
        Time.timeScale = 1;
        if (PlayerPrefs.HasKey("Load") && !PlayerPrefs.HasKey("NewGame"))
        {
            Load(saveSlots[PlayerPrefs.GetInt("Load")]);
            PlayerPrefs.DeleteKey("Load");
            // if (UIManager.MyInstance.MyCinemachines.Length > 2) UIManager.MyInstance.MyCinemachines[1].SetActive(true);
        }
        else if (PlayerPrefs.HasKey("NewGame"))
        {
            Eeola.MyInstance.SetDefaultValues();
            PlayerPrefs.DeleteKey("NewGame");
            // if (UIManager.MyInstance.MyCinemachines.Length > 2) UIManager.MyInstance.MyCinemachines[1].SetActive(true);
        }        
    }

    public void ShowDialogue(GameObject clickButton)
    {
        action = clickButton.name;
        switch (action)
        {
            case "Load":
                dialogueText.text = "Load game?";
                break;
            case "Save":
                dialogueText.text = "Save game?";
                break;
            case "Delete":
                dialogueText.text = "Delete savefile?";
                break;
        }
        currentSave = clickButton.GetComponentInParent<SavedGame>();
        dialogue.SetActive(true);
    }

    public void ExecuteAction()
    {
        switch (action)
        {
            case "Load":
                LoadScene(currentSave);
                break;
            case "Save":
                Save(currentSave);
                break;
            case "Delete":
                Delete(currentSave);
                break;
        }
        CloseDialogue();
    }

    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }

    private void LoadScene(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat",
                FileMode.Open);
            var data = (SaveData) bf.Deserialize(file);
            file.Close();
            PlayerPrefs.SetInt("Load", savedGame.MyIndex);
            SceneManager.LoadScene(data.MyScene);
        }
    }

    public void Delete(SavedGame savedGame)
    {
        File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");
        savedGame.HideVisuals();
    }
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.F5)) Save();
        // if (Input.GetKeyDown(KeyCode.F9)) Load();
    }

    private void ShowSavedFiles(SavedGame savedGame)
    {
        if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat"))
        {
            var bf = new BinaryFormatter();
            var file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat",
                FileMode.Open);
            var data = (SaveData) bf.Deserialize(file);
            file.Close();
            savedGame.ShowInfo(data);
        }
    }

    public void Save(SavedGame savedGame)
    {
        try
        {
            var bf = new BinaryFormatter();

            var file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Create);
            var data = new SaveData();
            data.MyScene = SceneManager.GetActiveScene().name;
            SaveEquipment(data);
            SaveItems(data);
            SaveActionButtons(data);
            SavePotionButtons(data);
            SaveQuests(data);
            SaveQuestGivers(data);
            SavePlayer(data);
            bf.Serialize(file, data);
            file.Close();
            ShowSavedFiles(savedGame);
            PlayerPrefs.SetInt("Load", savedGame.MyIndex);
            UIManager.MyInstance.QuitToMainMenu();
        }
        catch (Exception)
        {
            
        }
    }
    
    public void Load(SavedGame savedGame)
    {
        
        try
        {
            var bf = new BinaryFormatter();
            
            var file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
            var data = (SaveData) bf.Deserialize(file);
            file.Close();            
            LoadEquipment(data);
            LoadItems(data);
            LoadActionButtons(data);
            LoadPotionButtons(data);
            LoadQuests(data);
            LoadQuestGivers(data);
            LoadPlayer(data);
            // UIManager.MyInstance.MyCinemachines[1].transform.position = new Vector3(data.MyPlayerData.MyPositionX, data.MyPlayerData.MyPositionY, -10);
        }
        catch (Exception)
        {
            // Delete(savedGame);
            // PlayerPrefs.DeleteKey("Load");
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData(Eeola.MyInstance.MyLevel,
            Eeola.MyInstance.MyExp.MyCurrentValue,
            Eeola.MyInstance.MyExp.MyMaxValue,
            Eeola.MyInstance.MyHealth.MyCurrentValue,
            Eeola.MyInstance.MyHealth.MyMaxValue,
            Eeola.MyInstance.MyMana.MyCurrentValue,
            Eeola.MyInstance.MyMana.MyMaxValue,
            Eeola.MyInstance.transform.position,
            Eeola.MyInstance.MyStrength,
            Eeola.MyInstance.MyIntelligence,
            Eeola.MyInstance.MyAgility,
            Eeola.MyInstance.MyGold,
            Eeola.MyInstance.MyManaRegen,
            Eeola.MyInstance.MyDefence);
    }

    private void SaveItems(SaveData data)
    {
        List<SlotScript> slots = InventoryScript.MyInstance.GetAllItems();
        foreach (var slot in slots)
        {
            data.MyInventoryData.MyItems.Add(new ItemData(slot.MyItem.MyTitle, slot.MyIndex, slot.MyCount));
        }

    }

    private void SaveEquipment(SaveData data)
    {
        foreach (var charBtn in equipment)
        {
            if (charBtn.MyEquippedArmor != null)
            {
                data.MyEquipmentData.Add(new EquipmentData(charBtn.MyEquippedArmor.MyTitle, charBtn.name));
            }
        }
    }

    private void SaveActionButtons(SaveData data)
    {
        for (var i = 0; i < actionButtons.Length; i++)
        {
            if (actionButtons[i].MyUsable != null)
            {
                var a = new ActionButtonData((actionButtons[i].MyUsable as Projectile).MyName, i);
                data.MyActionButtonData.Add(a);
            }
        }
    }
    
    private void SavePotionButtons(SaveData data)
    {
        for (var i = 0; i < potionButtons.Length; i++)
        {
            if (potionButtons[i].MyUsable != null)
            {
                var a = new PotionButtonData((potionButtons[i].MyUsable as Item).MyTitle, i);
                data.MyPotionButtonData.Add(a);
            }
        }
    }

    private void SaveQuests(SaveData data)
    {
        foreach (var quest in Questlog.MyInstance.MyQuests)
        {
            data.MyQuestData.Add(new QuestData(quest.MyTitle,quest.MyDescription, quest.MyCollectObjectives, quest.MyKillObjectives, quest.MyQuestGiver.MyId));
        }
    }

    private void SaveQuestGivers(SaveData data)
    {
        var questGivers = FindObjectsOfType<QuestGiver>();
        foreach (var questGiver in questGivers)
        {
            data.MyQuestGiverData.Add(new QuestGiverData(questGiver.MyCompletedQuests, questGiver.MyId));
        }
    }
    private void LoadPlayer(SaveData data)
    {
        Eeola.MyInstance.MyLevel = data.MyPlayerData.MyLevel;
        StatsPanel.MyInstance.UpdateLevel();
        Eeola.MyInstance.MyHealth.Initialize(data.MyPlayerData.MyLife, data.MyPlayerData.MyMaxLife);
        Eeola.MyInstance.MyMana.Initialize(data.MyPlayerData.MyMana, data.MyPlayerData.MyMaxMana);
        Eeola.MyInstance.MyExp.Initialize(data.MyPlayerData.MyExp, data.MyPlayerData.MyMaxExp);
        Eeola.MyInstance.transform.position = new Vector2(-34, 0);
        Eeola.MyInstance.MyStrength = data.MyPlayerData.MyStrength;
        Eeola.MyInstance.MyIntelligence = data.MyPlayerData.MyIntelligence;
        Eeola.MyInstance.MyAgility = data.MyPlayerData.MyAgility;
        Eeola.MyInstance.MyGold = data.MyPlayerData.MyGold;
        Eeola.MyInstance.MyManaRegen = data.MyPlayerData.MyManaRegeneration;
        Eeola.MyInstance.MyDefence = data.MyPlayerData.MyDefence;
        StatsPanel.MyInstance.UpdateStats();
    }
    private void LoadItems(SaveData data)
    {
        foreach (var itemData in data.MyInventoryData.MyItems)
        {
            var item = Instantiate(Array.Find(items, x => x.MyTitle == itemData.MyTitle));
            for (int i = 0; i < itemData.MyStackCount; i++)
            {
                InventoryScript.MyInstance.PlaceInSpecific(item, itemData.MySlotIndex);
            }
            
        }
    }
    private void LoadEquipment(SaveData data)
    {
        foreach (var equipmentData in data.MyEquipmentData)
        {
            var charBtn = Array.Find(equipment, x => x.name == equipmentData.MyType);
            charBtn.EquipArmor(Array.Find(items, x => x.MyTitle == equipmentData.MyTitle) as Armor);
        }
    }

    private void LoadActionButtons(SaveData data)
    {
        foreach (var actionButtonData in data.MyActionButtonData)
        {
            var t = SpellBook.MyInstance.GetSpell(actionButtonData.MySpell); 
            actionButtons[actionButtonData.MyIndex].SetUsable(t);
        }
    }
    private void LoadPotionButtons(SaveData data)
    {
        foreach (var potionButtonData in data.MyPotionButtonData)
        {
            var t = InventoryScript.MyInstance.GetUsable(potionButtonData.MyPotion);
            potionButtons[potionButtonData.MyIndex].SetUsable(t);
        }
    }
    private void LoadQuests(SaveData data)
    {
        var questGivers = FindObjectsOfType<QuestGiver>();
        foreach (var questData in data.MyQuestData)
        {
            var qg = Array.Find(questGivers, x => x.MyId == questData.MyQuestGiverID);
            var q = Array.Find(qg.MyQuests, x => x.MyTitle == questData.MyTitle);
            q.MyQuestGiver = qg;
            q.MyKillObjectives = questData.MyKillObjectives;
            Questlog.MyInstance.AcceptQuest(q);
        }
    }

    private void LoadQuestGivers(SaveData data)
    {
        var questGivers = FindObjectsOfType<QuestGiver>();
        foreach (var questGiverData in data.MyQuestGiverData)
        {
            var qg = Array.Find(questGivers, x => x.MyId == questGiverData.MyQuestGiverID);
            qg.MyCompletedQuests = questGiverData.MyCompletedQuests;
            qg.UpdateQuestStatus();
        }
    }
}
