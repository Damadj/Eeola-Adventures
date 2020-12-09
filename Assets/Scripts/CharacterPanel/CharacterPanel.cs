using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    private static CharacterPanel _instance;
    public static CharacterPanel MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<CharacterPanel>();
            return _instance;
        }
    }
    private CanvasGroup canvasGroup;
    [SerializeField] private CharButton head, feet, legs, chest, shoulders, hands, wrists,
        mainhand, offhand, necklace, ringOne, ringTwo, trinket, back;    

    public CharButton MySelectedButton { get; set; }
    public void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Open()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }
    public void Close()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;        
    }

    public void EquipArmor(Armor armor)
    {
        switch (armor.MyArmorType)
        {
            case ArmorType.Head: 
                head.EquipArmor(armor);
                break;
            case ArmorType.Feet:
                feet.EquipArmor(armor);
                break;
            case ArmorType.Legs: 
                legs.EquipArmor(armor);
                break;
            case ArmorType.Chest: 
                chest.EquipArmor(armor);
                break;
            case ArmorType.Shoulders: 
                shoulders.EquipArmor(armor);
                break;
            case ArmorType.Hands: 
                hands.EquipArmor(armor);
                break;
            case ArmorType.Wrists:
                wrists.EquipArmor(armor);
                break;
            case ArmorType.MainHand:
                mainhand.EquipArmor(armor);
                break;
            case ArmorType.OffHand:
                offhand.EquipArmor(armor);
                break;
            case ArmorType.Necklace:
                necklace.EquipArmor(armor);
                break;
            case ArmorType.Ring:
                if (!ringOne.MyIcon.enabled) ringOne.EquipArmor(armor);
                else if (ringOne.MyIcon.enabled && !ringTwo.MyIcon.enabled) ringTwo.EquipArmor(armor);
                else if (ringOne.MyIcon.enabled && ringTwo.MyIcon.enabled) ringOne.EquipArmor(armor);
                break;
            case ArmorType.Trinket: 
                trinket.EquipArmor(armor);
                break;
            case ArmorType.Back: 
                back.EquipArmor(armor);
                break;
        }
    }
}
