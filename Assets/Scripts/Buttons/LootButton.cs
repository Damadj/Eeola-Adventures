using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] public Image icon;
    [SerializeField] private Text title;
    [SerializeField] private Image titleBG;
    private LootWindow lootWindow;
    public Image MyIcon { get => icon; set => icon = value; }
    public Text MyTitle { get => title; set => title = value; }
    public Image MyTitleBG { get => titleBG; set => titleBG = value; }
    public Item MyLoot { get; set; }

    private void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.MyInstance.ShowTooltip(new Vector2(1,0), transform.position, MyLoot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (MyLoot.MyTitle.Equals("Gold"))
        {
            Eeola.MyInstance.MyGold += MyLoot.MyPrice;
            gameObject.SetActive(false);
            lootWindow.TakeLoot(MyLoot);
            UIManager.MyInstance.HideTooltip();
        }
        else if (InventoryScript.MyInstance.AddItem(Instantiate(MyLoot)))
        {
            gameObject.SetActive(false);
            lootWindow.TakeLoot(MyLoot);
            UIManager.MyInstance.HideTooltip();
        }
    }
}
