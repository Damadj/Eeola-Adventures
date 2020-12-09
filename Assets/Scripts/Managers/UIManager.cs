using System;
using System.Collections;
using Cinemachine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<UIManager>();
            return _instance;
        }
    }
    [SerializeField] private ActionButton[] actionButtons;
    [SerializeField] private CanvasGroup[] menus;
    [SerializeField] private PotionButton[] potionButtons;    
    private GameObject[] keyBindButtons;
    [SerializeField] private GameObject tooltip;
    private Text tooltipText;
    [SerializeField] private RectTransform tooltipRect;
    [SerializeField] private GameObject[] cinemachines;
   
    private AsyncOperation sceneAsync;
    public bool MenuOpen { get; set; }
    public GameObject[] MyCinemachines { get => cinemachines;}    
    private void Awake()
    {
        MenuOpen = false;        
        keyBindButtons = GameObject.FindGameObjectsWithTag("KeyBind");
        tooltipText = tooltip.GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Eeola.MyInstance != null)
                {
                    CloseLeftSide(KeyBindManager.MyInstance);
                    CloseRightSide(KeyBindManager.MyInstance);
                }

                OpenClose(menus[0]);
            }
        }

        if (Eeola.MyInstance != null && !MenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.K)) SpellBook.MyInstance.OpenClose();
            if (Input.GetKeyDown(KeyCode.J)) Questlog.MyInstance.OpenClose();
            if (Input.GetKeyDown(KeyCode.I)) InventoryScript.MyInstance.OpenClose();
            if (Input.GetKeyDown(KeyCode.C)) StatsPanel.MyInstance.OpenClose();
        }
     
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.UnloadSceneAsync(2);
            Eeola.MyInstance.transform.position = new Vector2(-34,0);
        }
    }
    public void UpdateKeyText(string key, KeyCode code)
    {
        var tmp = Array.Find(keyBindButtons, x => x.name == key).GetComponentInChildren<Text>();
        if (code == KeyCode.None) tmp.text = "";
        else tmp.text = code.ToString();
    }

    public void ClickActionButton(string buttonName)
    {
        var button = Array.Find(actionButtons, x => x.gameObject.name == buttonName);
        if (button.MyUsable != null)
        {
            ResetActionButtons();
            button.MyButton.onClick.Invoke();
            button.GetComponent<Image>().sprite = button.MyActiveSprite;
            button.GetComponent<Image>().color = new Color(1, 1, 0.5f, 1);
        }
    }
    
    public void ClickPotionButton(string buttonName)
    {
        var button = Array.Find(potionButtons, x => x.gameObject.name == buttonName);
        button.MyButton.onClick.Invoke();
    }    
    public void ResetActionButtons()
    {
        Array.ForEach(actionButtons, actionButton =>
        {
            actionButton.GetComponent<Image>().sprite = actionButton.MyInactiveSprite;
            actionButton.GetComponent<Image>().color = Color.white;
            
        });
    }

    public void StartNewGame()
    {
        PlayerPrefs.SetInt("NewGame", 1);
        SceneManager.LoadScene("TownScene");
    }

    public void QuitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OpenClose(CanvasGroup canvasGroup)
    {
        if (canvasGroup.alpha == 1)
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            MenuOpen = false;
            Time.timeScale = 1;
        }
        else
        {
            OpenSingle(canvasGroup);
            MenuOpen = true;
            Time.timeScale = 0;
        }
    }
    public void OpenSingle(CanvasGroup canvasGroup)
    {
        foreach (var canvas in menus)
        {
            CloseSingle(canvas);
        }
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts != true;
    }
    public void CloseSingle(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
    public void ResumeGame(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        MenuOpen = false;
        Time.timeScale = 1;
    }
    
    public void CloseLeftSide<T>(T window)
    {
        if (window is SpellBook)
        {
            Questlog.MyInstance.Close();
            StatsPanel.MyInstance.Close();
            VendorWindow.MyInstance.Close();
            ChestScript.MyInstance.Close();
        }
        else if (window is Questlog)
        {
            StatsPanel.MyInstance.Close();
            VendorWindow.MyInstance.Close();
            ChestScript.MyInstance.Close();
            SpellBook.MyInstance.Close();
        }
        else if (window is StatsPanel)
        {
            Questlog.MyInstance.Close();
            VendorWindow.MyInstance.Close();
            ChestScript.MyInstance.Close();
            SpellBook.MyInstance.Close();
        }
        else if (window is VendorWindow)
        {
            Questlog.MyInstance.Close();
            StatsPanel.MyInstance.Close();
            ChestScript.MyInstance.Close();
            SpellBook.MyInstance.Close();
        }
        else if (window is ChestScript)
        {
            Questlog.MyInstance.Close();
            StatsPanel.MyInstance.Close();
            VendorWindow.MyInstance.Close();
            SpellBook.MyInstance.Close();
        }
        else if (window is KeyBindManager)
        {
            ChestScript.MyInstance.Close();
            Questlog.MyInstance.Close();
            StatsPanel.MyInstance.Close();
            VendorWindow.MyInstance.Close();
            SpellBook.MyInstance.Close();
        }
    }
    public void CloseRightSide<T>(T window)
    {
        if (window is KeyBindManager)
        {
            InventoryScript.MyInstance.Close();            
            QuestGiverWindow.MyInstance.Close();
            LootWindow.MyInstance.Close();
        }
        else if (window is InventoryScript)
        {
            QuestGiverWindow.MyInstance.Close();
        }
        else if (window is QuestGiverWindow)
        {
            InventoryScript.MyInstance.Close();
            LootWindow.MyInstance.Close();
        }
    }
    public void UpdateStackSize(IClickable clickable)
    {
        if (clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackSizeIcon.color = Color.white;
            clickable.MyStackText.color = new Color(0.4862745f,0.4078432f,0.3176471f,1);
            clickable.MyIcon.color = Color.white;
        }
        else
        {
            clickable.MyStackSizeIcon.color = new Color(0,0,0,0);
            clickable.MyStackText.color = new Color(0,0,0,0);
            clickable.MyIcon.color = Color.white;
        }
        if (clickable.MyCount == 0)
        {
            clickable.MyStackSizeIcon.color = new Color(0,0,0,0);
            clickable.MyIcon.color = new Color(0,0,0,0);
            clickable.MyStackText.color = new Color(0,0,0,0);
        }
    }

    public void ShowTooltip(Vector2 pivot, Vector3 position, IDescribable description)
    {
        tooltipRect.pivot = pivot;
        tooltip.SetActive(true);
        tooltip.transform.position = position;
        tooltipText.text = description.GetDescription();
    }

    public void RefreshTooltip(IDescribable description)
    {
        tooltipText.text = description.GetDescription();
    }
    public void HideTooltip()
    {
        tooltip.SetActive(false);
    }

    public IEnumerator LoadScene(int indexTo, int indexFrom)
    {
        if (!SceneManager.GetSceneByBuildIndex(indexTo).isLoaded)
        {
            var scene = SceneManager.LoadSceneAsync(indexTo, LoadSceneMode.Additive);
            scene.allowSceneActivation = false;
            sceneAsync = scene;

            while (scene.progress < 0.9f) yield return null;

            cinemachines[indexFrom].SetActive(false);
            Eeola.MyInstance.MyPlayerTransform.position = new Vector2(16, -28);
            cinemachines[indexTo].transform.position = new Vector3(16, -28, -10);
            enableScene(2);
            cinemachines[indexTo].SetActive(true);
        }
        else
        {
            MyCinemachines[indexFrom].SetActive(false);            
            Eeola.MyInstance.transform.position = new Vector2(16,-28);
            cinemachines[indexTo].transform.position = new Vector3(16, -28, -10);
            MyCinemachines[indexTo].SetActive(true);
        }
    }

    private void enableScene(int index)
    {
        sceneAsync.allowSceneActivation = true;
        var sceneToLoad = SceneManager.GetSceneByBuildIndex(index);
        if (sceneToLoad.IsValid())
        {
            if (sceneToLoad.isLoaded) SceneManager.SetActiveScene(sceneToLoad);                
        }
    }
}
