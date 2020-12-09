using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorType {Head, Shoulders, Chest, Wrists, Hands, Legs, Feet, Back, Necklace, Ring, Trinket, MainHand, OffHand, TwoHand}
[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]
public class Armor : Item
{
    [SerializeField] private ArmorType armorType;
    [SerializeField] private int intelligence;
    [SerializeField] private int strength;
    [SerializeField] private int agility;
    public ArmorType MyArmorType { get => armorType; }
    public int MyStrength { get => strength; }
    public int MyIntelligence { get => intelligence; }
    public int MyAgility { get => agility; }

    public override string GetDescription()
    {
        var stats = string.Empty;
        if (intelligence > 0) stats += string.Format("\n+{0} intelligence", intelligence);
        if (strength > 0) stats += string.Format("\n+{0} strength", strength);
        if (agility > 0) stats += string.Format("\n+{0} agility", agility);
        return base.GetDescription() + stats;
    }

    public void Equip()
    {
        CharacterPanel.MyInstance.EquipArmor(this);
    }
}
