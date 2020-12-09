using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject, IMovable, IDescribable
{
    [SerializeField] private Sprite icon;
    [SerializeField] private int stackSize;
    [SerializeField] private string title;
    [SerializeField] private Quality quality;
    [SerializeField] private int price;
    private SlotScript slot;
    public Sprite MyIcon { get => icon; set => icon = value; }
    public int MyStackSize { get => stackSize; set => stackSize = value; }
    public SlotScript MySlot { get => slot; set => slot = value; }
    public Quality MyQuality { get => quality; set => quality = value; }
    public string MyTitle { get => title; set => title = value; }
    public int MyPrice { get => price; set => price = value; }
    public void Remove()
    {
        if (MySlot != null) MySlot.RemoveItem(this);
    }

    public virtual string GetDescription()
    {
        return string.Format("<color={0}><b>{1}</b></color>", QualityColor.MyColors[quality], title);
    }
}
