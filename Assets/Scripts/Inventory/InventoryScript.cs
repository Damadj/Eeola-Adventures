using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ItemCountChange(Item item);
public class InventoryScript : MonoBehaviour
{
    public event ItemCountChange itemCountChangeEvent;
    private static InventoryScript _instance;
    public static InventoryScript MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<InventoryScript>();
            return _instance;
        }
    }    
    [SerializeField] private Item[] items;
    [SerializeField] private List<SlotScript> slots;
    private CanvasGroup canvasGroup;
    private SlotScript fromSlot;
    public List<SlotScript> MySlots { get => slots; }

    public SlotScript MyFromSlot
    {
        get
        {
            return fromSlot;
        }
        set
        {
            fromSlot = value;
            if (value != null)
            {
                fromSlot.MyIcon.color = Color.gray;
            }
        }
    }

    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddItem((Armor) Instantiate(items[0]));
            AddItem((Armor) Instantiate(items[1]));
            AddItem((Armor) Instantiate(items[2]));
            AddItem((Armor) Instantiate(items[3]));
            AddItem((Armor) Instantiate(items[4]));
            AddItem((Armor) Instantiate(items[5]));
            AddItem((Armor) Instantiate(items[6]));
            AddItem((Armor) Instantiate(items[7]));
            AddItem((Armor) Instantiate(items[8]));
            AddItem((Armor) Instantiate(items[9]));
            AddItem((Armor) Instantiate(items[10]));
            AddItem((Armor) Instantiate(items[11]));
            AddItem((Armor) Instantiate(items[12]));
            AddItem((Armor) Instantiate(items[13]));
            AddItem((Armor) Instantiate(items[14]));
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            AddItem((HealthPotion) Instantiate(items[15]));
        }
    }

    public bool AddItem(Item item)
    {
        if (item.MyStackSize > 0)
        {
            if (PlaceInStack(item)) return true;
        }
        return PlaceInEmpty(item);
    }

    private bool PlaceInEmpty(Item item)
    {
        foreach (var slot in slots)
        {
            if (!slot.name.Contains("Chest") && slot.AddItem(item))
            {
                // OnItemCountChanged(item);
                return true;
            }
        }

        return false;
    }
    private bool PlaceInStack(Item item)
    {
        foreach (var slot in slots)
        {
            if (!slot.name.Contains("Chest")&& slot.StackItem(item))
            {
                OnItemCountChanged(item);
                return true;
            }
        }

        return false;
    }

    public void PlaceInSpecific(Item item, int slotIndex)
    {
        MySlots[slotIndex].AddItem(item);
    }
    
    public void OpenClose()
    {
        if (Mathf.Approximately(canvasGroup.alpha, 1)) Close();
        else Open();
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        CharacterPanel.MyInstance.Close();
    }
    public void Open()
    {
        UIManager.MyInstance.CloseRightSide(this);
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        CharacterPanel.MyInstance.Open();
    }

    public List<SlotScript> GetAllItems()
    {
        List<SlotScript> items = new List<SlotScript>();
        foreach (var slot in MySlots)
        {
            if (!slot.IsEmpty)
            {
                items.Add(slot);
            }
        }

        return items;
    }
    public Stack<IUsable> GetUsables(IUsable type)
    {
        Stack<IUsable> usables = new Stack<IUsable>();
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType() && !slot.name.Contains("Chest"))
            {
                foreach (var item in slot.MyItems)
                {
                    usables.Push(item as IUsable);
                }
            }
        }
        
        return usables;
    }

    public IUsable GetUsable(string type)
    {
        Stack<IUsable> usables = new Stack<IUsable>();
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.MyItem.MyTitle == type && !slot.name.Contains("Chest"))
            {
                return slot.MyItem as IUsable;
            }
        }
        
        return null;
    }

    public int GetItemCount(string type)
    {
        var itemCount = 0;
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
            {
                itemCount += slot.MyItems.Count;
            }
        }

        return itemCount;
    }

    public Stack<Item> GetItems(string type, int count)
    {
        Stack<Item> items = new Stack<Item>();
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.MyItem.MyTitle == type)
            {
                foreach (var item in slot.MyItems)
                {
                    items.Push(item);
                    if (items.Count == count) return items;
                }
            }
        }

        return items;
    }
    public void OnItemCountChanged(Item item)
    {
        if (itemCountChangeEvent != null)
        {
            itemCountChangeEvent.Invoke(item);
        }
    }
}
