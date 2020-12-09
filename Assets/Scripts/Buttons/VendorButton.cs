using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] public Image icon;
    [SerializeField] private Text title;
    [SerializeField] private Image titleBG;
    [SerializeField] private Text price;
    [SerializeField] private Text quantity;
    [SerializeField] private Image quantityBG;
    private VendorItem vendorItem;    
    // public Image MyIcon { get => icon; set => icon = value; }
    // public Text MyTitle { get => title; set => title = value; }
    // public Image MyTitleBG { get => titleBG; set => titleBG = value; }
    // public Item MyLoot { get; set; }
    public void AddItem(VendorItem item)
    {
        vendorItem = item;
        if (item.MyQuantity > 0 || (item.MyQuantity == 0 && item.MyUnlimited))
        {
            icon.sprite = item.MyItem.MyIcon;
            title.text = string.Format("<color={0}>{1}</color>", QualityColor.MyColors[item.MyItem.MyQuality],
                item.MyItem.MyTitle);
            titleBG.color = Color.white;
            price.text = item.MyItem.MyPrice.ToString();
            if (!item.MyUnlimited)
            {
                quantity.text = item.MyQuantity.ToString();
                quantityBG.color = Color.white;
            }
            else
            {
                quantity.text = "";
                quantityBG.color = new Color(0,0,0,0);
            }

            gameObject.SetActive(true);
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.MyInstance.ShowTooltip(new Vector2(0,0), transform.position, vendorItem.MyItem);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (HandScript.MyInstance.MyMovable != null && (HandScript.MyInstance.MyMovable as Item).MyPrice > 0)
        {
            if ((HandScript.MyInstance.MyMovable as Item).MyStackSize > 0)
            {
                Eeola.MyInstance.MyGold +=
                    (int) Mathf.Floor((HandScript.MyInstance.MyMovable as Item).MyPrice * (HandScript.MyInstance.MyMovable as Item).MySlot.MyCount / 2.0f);
            }
            else Eeola.MyInstance.MyGold += (int) Mathf.Floor((HandScript.MyInstance.MyMovable as Item).MyPrice / 2.0f);
            HandScript.MyInstance.DeleteItem();
        }
        else if (Eeola.MyInstance.MyGold >= vendorItem.MyItem.MyPrice && InventoryScript.MyInstance.AddItem(Instantiate(vendorItem.MyItem)))
        {
            SellItem();
        }
    }

    private void SellItem()
    {
        Eeola.MyInstance.MyGold -= vendorItem.MyItem.MyPrice;
        if (!vendorItem.MyUnlimited)
        {
            vendorItem.MyQuantity--;
            quantity.text = vendorItem.MyQuantity.ToString();
            if (vendorItem.MyQuantity == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
