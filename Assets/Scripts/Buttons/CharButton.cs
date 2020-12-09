using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ArmorType armorType;
    [SerializeField] private Image icon;
    public Image MyIcon { get => icon; set => icon = value; }

    private Armor equippedArmor;
    public Armor MyEquippedArmor { get => equippedArmor;}

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMovable is Armor && ((Item) HandScript.MyInstance.MyMovable).MySlot != null)
            {
                Armor tmp = (Armor) HandScript.MyInstance.MyMovable;
                if (tmp.MyArmorType == armorType)
                {
                    EquipArmor(tmp);
                }
                UIManager.MyInstance.RefreshTooltip(tmp);
            }
            else if (HandScript.MyInstance.MyMovable == null && equippedArmor != null)
            {
                HandScript.MyInstance.TakeMovable(equippedArmor);
                CharacterPanel.MyInstance.MySelectedButton = this;
                icon.color = Color.grey;
            } 
            else if (HandScript.MyInstance.MyMovable is Armor &&
                     ((Item) HandScript.MyInstance.MyMovable).MySlot == null)
            {
                //Swap movable with current char button item
            }
        }
    }

    public void EquipArmor(Armor armor)
    {
        armor.Remove();
        if (equippedArmor != null)
        {
            if (equippedArmor != armor)
            {
                armor.MySlot.AddItem(equippedArmor);
            }
            UIManager.MyInstance.RefreshTooltip(equippedArmor);
        }
        else
        {
            UIManager.MyInstance.HideTooltip();
        }
        icon.enabled = true;
        icon.sprite = armor.MyIcon;
        icon.color = Color.white;
        equippedArmor = armor;
        equippedArmor.MySlot = null;
        Eeola.MyInstance.MyStrength += equippedArmor.MyStrength;
        Eeola.MyInstance.MyAgility += equippedArmor.MyAgility;
        Eeola.MyInstance.MyIntelligence += equippedArmor.MyIntelligence;
        StatsPanel.MyInstance.UpdateStats();        
        if (HandScript.MyInstance.MyMovable == (armor as IMovable))
        {
            HandScript.MyInstance.Drop();
        }
    }

    public void DequipArmor()
    {
        Eeola.MyInstance.MyStrength -= equippedArmor.MyStrength;
        Eeola.MyInstance.MyAgility -= equippedArmor.MyAgility;
        Eeola.MyInstance.MyIntelligence -= equippedArmor.MyIntelligence;
        StatsPanel.MyInstance.UpdateStats();
        icon.color = Color.white;
        icon.enabled = false;
        equippedArmor = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (equippedArmor != null)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1,0), transform.position, equippedArmor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}
