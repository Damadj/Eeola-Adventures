using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsPanel : MonoBehaviour
{
    private static StatsPanel _instance;
    public static StatsPanel MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<StatsPanel>();
            return _instance;
        }
    }
    [SerializeField] private Text lifeText;
    [SerializeField] private Text statsText;
    [SerializeField] private Text parametersText;
    [SerializeField] private Text levelText;
    public CanvasGroup MyCanvasGroup { get; set; }
   
    private void Awake()
    {
        MyCanvasGroup = GetComponent<CanvasGroup>();
        UpdateLevel();
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
    public void UpdateLevel()
    {
        levelText.text = Eeola.MyInstance.MyLevel.ToString();
    }
    public void UpdateStats()
    {
        if (Eeola.MyInstance.MyMinDamage == Eeola.MyInstance.MyMaxDamage)
        {
            parametersText.text = string.Format(
                "{0}\n{1}\n{2}%\n0\n0\n0\n0",
                Eeola.MyInstance.MyDefence.ToString(),
                Eeola.MyInstance.MyMinDamage.ToString(),
                Eeola.MyInstance.MyManaRegen);
        }
        else
        {
            parametersText.text = string.Format(
                "{0}\n{1} - {2}\n{3}%\n0\n0\n0\n0",
                Eeola.MyInstance.MyDefence.ToString(),
                Eeola.MyInstance.MyMinDamage.ToString(),
                Eeola.MyInstance.MyMaxDamage.ToString(),
                Eeola.MyInstance.MyManaRegen);
        }

        statsText.text = string.Format("{0}\n{1}\n{2}",
            Eeola.MyInstance.MyStrength.ToString(),
            Eeola.MyInstance.MyIntelligence.ToString(),
            Eeola.MyInstance.MyAgility.ToString());
        lifeText.text = string.Format("{0}\n{1}",
            Eeola.MyInstance.MyHealth.MyMaxValue,
            Eeola.MyInstance.MyMana.MyMaxValue);
    }
}
