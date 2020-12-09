using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
    [SerializeField] private Text dateTime;
    [SerializeField] private Text levelText;
    [SerializeField] private Image portrait;
    [SerializeField] private GameObject visuals;
    [SerializeField] private int index;
    public int MyIndex { get => index; set => index = value; }
    private void Awake()
    {
        visuals.SetActive(false);
    }

    public void ShowInfo(SaveData data)
    {
        visuals.SetActive(true);
        dateTime.text = "Eeola\n" + data.MyDateTime.ToString("dd/MM/yyy") + "  " + data.MyDateTime.ToString("H:mm");
        levelText.text = data.MyPlayerData.MyLevel.ToString();
    }

    public void HideVisuals()
    {
        visuals.SetActive(false);
    }
}
