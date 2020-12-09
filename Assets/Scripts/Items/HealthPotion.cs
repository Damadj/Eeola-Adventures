using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotion", menuName = "Items/Potions/Health potion", order = 1)]
public class HealthPotion : Item, IUsable
{
    [SerializeField] private int health;
    public void Use()
    {
        if (Eeola.MyInstance.MyHealth.MyCurrentValue < Eeola.MyInstance.MyHealth.MyMaxValue)
        {
            Remove();
            if (MySlot.MyCount == 0) UIManager.MyInstance.HideTooltip();
            Eeola.MyInstance.ChangeHealth(health);
        }
    }

    public override string GetDescription()
    {
        return base.GetDescription() + string.Format("\nRestores {0} health", health);
    }
}
