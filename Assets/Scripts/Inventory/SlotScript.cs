using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    private static SlotScript _instance;
    public static SlotScript MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<SlotScript>();
            return _instance;
        }
    }
    [SerializeField] private Image icon;
    [SerializeField] private Text stackSize;
    [SerializeField] private Image stackSizeIcon;
    public Image MyIcon { get => icon; set => icon = value; }
    public Text MyStackText { get => stackSize; }
    public Image MyStackSizeIcon { get => stackSizeIcon; }
    private ObservableStack<Item> items = new ObservableStack<Item>();
    public ObservableStack<Item> MyItems { get => items; }
    public bool IsEmpty { get => items.Count == 0; }    
    public int MyCount { get => items.Count; }
    public int MyIndex { get => InventoryScript.MyInstance.MySlots.IndexOf(this); }

    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize) return false;            
            return true;
        }
    }

    public Item MyItem
    {
        get
        {
            if (!IsEmpty) return items.Peek();
            return null;
        }
    }

    private void Awake()
    {
        items.OnPop += UpdateSlot;
        items.OnPush += UpdateSlot;
        items.OnClear += UpdateSlot;
    }

    public bool AddItem(Item item)
    {
        if (IsEmpty || (item.GetType() == MyItem.GetType() && MyItem.MyStackSize > 1 && !IsFull))
        {
            items.Push(item);
            InventoryScript.MyInstance.OnItemCountChanged(item);
            icon.sprite = item.MyIcon;
            icon.color = Color.white;
            item.MySlot = this;
            return true;
        }

        return false;
    }

    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;
            for (int i = 0; i < count; i++)
            {
                if (IsFull) return false;
                AddItem(newItems.Pop());
            }
            
            return true;
        }

        return false;
    }

    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
        }
    }

    public void Clear()
    {
        if (items.Count > 0)
        {
            int count = items.Count;
            for (int i = 0; i < count; i++)
            {
                InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            }
            
            items.Clear();
        }
    }
    //When slot is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) //Pick or drop item
        {
            if (InventoryScript.MyInstance.MyFromSlot == null && !IsEmpty)
            {
                if (HandScript.MyInstance.MyMovable != null)
                {
                    if (HandScript.MyInstance.MyMovable is Armor)
                    {
                        if (MyItem is Armor && (MyItem as Armor).MyArmorType ==
                            (HandScript.MyInstance.MyMovable as Armor).MyArmorType)
                        {
                            (MyItem as Armor).Equip();
                            HandScript.MyInstance.Drop();
                            if (CharacterPanel.MyInstance.MySelectedButton != null) CharacterPanel.MyInstance.MySelectedButton = null;
                        }
                    }
                }
                else
                {
                    HandScript.MyInstance.TakeMovable(MyItem);
                    InventoryScript.MyInstance.MyFromSlot = this;
                }
            }
            else if (InventoryScript.MyInstance.MyFromSlot == null && IsEmpty)
            {
                if (HandScript.MyInstance.MyMovable is Armor)
                {
                    Armor armor = (Armor) HandScript.MyInstance.MyMovable;
                    AddItem(armor);
                    CharacterPanel.MyInstance.MySelectedButton.DequipArmor();
                    CharacterPanel.MyInstance.MySelectedButton = null;
                    HandScript.MyInstance.Drop();
                }
            }
            else if (InventoryScript.MyInstance.MyFromSlot != null) //If something is in hand
            {
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.MyFromSlot)
                                  ||SwapItems(InventoryScript.MyInstance.MyFromSlot)
                                  || AddItems(InventoryScript.MyInstance.MyFromSlot.items))
                {                    
                    HandScript.MyInstance.Drop();                    
                    InventoryScript.MyInstance.MyFromSlot = null;
                    UIManager.MyInstance.HideTooltip();
                    UIManager.MyInstance.ShowTooltip(new Vector2(1,0), transform.position, MyItem);
                }
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right && HandScript.MyInstance.MyMovable == null)
        {
            UseItem();
        }
    }

    public void UseItem()
    {
        if (MyItem is IUsable)
        {
            (MyItem as IUsable).Use();
        }
        else if (MyItem is Armor)
        {
            (MyItem as Armor).Equip();
        }
    }

    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && items.Count < MyItem.MyStackSize)
        {
            items.Push(item);
            item.MySlot = this;
            return true;
        }
        return false;
    }

    private bool PutItemBack()
    {
        if (InventoryScript.MyInstance.MyFromSlot == this)
        {
            InventoryScript.MyInstance.MyFromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }

    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty) return false;
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount + MyCount > MyItem.MyStackSize)
        {
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.items); //Copy all items from slot A
            from.items.Clear(); //Clear slot A
            from.AddItems(items); //Add all items from slot B to slot A
            items.Clear(); //Clear slot B
            AddItems(tmpFrom); //Add all items from slot A to slot B
            return true;
        }

        return false;
    }

    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty) return false;
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            var free = MyItem.MyStackSize - MyCount;
            if (free > from.MyCount)
            {
                var fromCount = from.MyCount;
                for (int i = 0; i < fromCount; i++)
                {
                    AddItem(from.items.Pop());
                }
            }
            else
            {
                for (int i = 0; i < free; i++)
                {
                    AddItem(from.items.Pop());

                }
            }

            return true;
        }

        return false;
    }

    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsEmpty)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1,0), transform.position, MyItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}