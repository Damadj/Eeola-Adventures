using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField] private Loot[] loot;
    private List<Item> droppedItems = new List<Item>();
    private bool rolled = false;

    public void ShowLoot(int enemyLvl, GameObject enemy)
    {
        if (!rolled)
        {
            RollLoot(enemyLvl);
        }
        LootWindow.MyInstance.CreatePages(droppedItems, enemy);
    }

    private void RollLoot(int enemyLvl)
    {
        foreach (var item in loot)
        {
            var roll = Random.Range(0, 100);
            if (roll <= item.MyDropChance)
            {
                if (item.MyItem.MyTitle.Equals("Gold"))
                {
                    var rollGold = Random.Range(enemyLvl * 10, enemyLvl * 20);
                    item.MyItem.MyPrice = rollGold;
                }
                droppedItems.Add(item.MyItem);
            }
        }

        rolled = true;
    }
}
