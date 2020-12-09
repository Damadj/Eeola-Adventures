using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
    private static LootWindow _instance;
    public static LootWindow MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<LootWindow>();
            return _instance;
        }
    }
    [SerializeField] private LootButton[] lootButtons;
    private List<List<Item>> pages = new List<List<Item>>();
    private List<Item> droppedLoot = new List<Item>();
    private int pageIndex = 0;
    [SerializeField] private Text pageNumber;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;
    [SerializeField] private CanvasGroup canvasGroup;
    public bool IsOpen { get => canvasGroup.alpha > 0; }
    private GameObject deadEnemy;

    public void CreatePages(List<Item> loot, GameObject enemy)
    {
        if (!IsOpen)
        {
            deadEnemy = enemy;
            var page = new List<Item>();
            droppedLoot = loot;
            for (int i = 0; i < loot.Count; i++)
            {
                page.Add(loot[i]);
                if (page.Count == 4 || i == loot.Count - 1)
                {
                    pages.Add(page);
                    page = new List<Item>();
                }
            }

            AddLoot();
            if (pages.Count != 0) Open();
            if (pages.Count == 0) Destroy(deadEnemy);
        }
    }
    private void AddLoot()
    {
        if (pages.Count > 0)
        {
            pageNumber.text = pageIndex + 1 + "/" + pages.Count;
            previousButton.SetActive(pageIndex > 0);
            nextButton.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    lootButtons[i].MyIcon.sprite = pages[pageIndex][i].MyIcon;
                    lootButtons[i].MyLoot = pages[pageIndex][i];
                    lootButtons[i].gameObject.SetActive(true);
                    lootButtons[i].MyTitleBG.color = Color.white;
                    lootButtons[i].MyTitle.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[pages[pageIndex][i].MyQuality],
                        pages[pageIndex][i].MyTitle);
                }

            }
        }
    }

    public void ClearButtons()
    {
        foreach (var lootButton in lootButtons)
        {
            lootButton.gameObject.SetActive(false);
        }
    }
    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)
        {
            pageIndex++;
            ClearButtons();
            AddLoot();
        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddLoot();
        }
    }

    public void TakeLoot(Item loot)
    {
        pages[pageIndex].Remove(loot);
        droppedLoot.Remove(loot);
        if (pages[pageIndex].Count == 0)
        {
            pages.Remove(pages[pageIndex]);
            if (pageIndex == pages.Count && pageIndex > 0)
            {
                pageIndex--;
            }
            AddLoot();
        }
        if (pages.Count == 0)
        {
            Destroy(deadEnemy);
            Close();
        }
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
    public void Close()
    {
        pageIndex = 0;
        pages.Clear();
        ClearButtons();
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
