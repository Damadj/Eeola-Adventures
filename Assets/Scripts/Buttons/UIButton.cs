using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour, IPointerDownHandler
{
    public Button MyButton { get; private set; }

    private void Awake()
    {
        MyButton = GetComponent<Button>();
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!UIManager.MyInstance.MenuOpen)
            {
                MyButton.onClick.Invoke();
                if (MyButton.name == "Inventory") InventoryScript.MyInstance.OpenClose();
                if (MyButton.name == "Skills") SpellBook.MyInstance.OpenClose();
                if (MyButton.name == "Stats") StatsPanel.MyInstance.OpenClose();
                if (MyButton.name == "Quests") Questlog.MyInstance.OpenClose();
            }
        }
    }
    
    public void OnClick()
    {
        if (!UIManager.MyInstance.MenuOpen)
        {
            InventoryScript.MyInstance.OpenClose();
        }
    }
}
