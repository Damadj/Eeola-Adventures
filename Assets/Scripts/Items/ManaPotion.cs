using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaPotion", menuName = "Items/Potions/Mana potion", order = 2)]
public class ManaPotion : Item, IUsable
{
    [SerializeField] private int mana;
    public void Use()
    {
        if (Eeola.MyInstance.MyMana.MyCurrentValue < Eeola.MyInstance.MyMana.MyMaxValue)
        {
            Remove();
            if (MySlot.MyCount == 0) UIManager.MyInstance.HideTooltip();
            Eeola.MyInstance.ChangeMana(mana);
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\nRestores {0} mana", mana);
    }
}
