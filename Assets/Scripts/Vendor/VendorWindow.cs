using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VendorWindow : Window, IPointerClickHandler
{
    private static VendorWindow _instance;
    public static VendorWindow MyInstance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<VendorWindow>();
            return _instance;
        }
    }  
    
    [SerializeField] private VendorButton[] vendorButtons;
    [SerializeField] private Text pageNumber;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject previousButton;
    List<List<VendorItem>> pages = new List<List<VendorItem>>();
    private int pageIndex;
    private Vendor vendor;

    public void CreatePages(VendorItem[] items)
    {
        pages.Clear();
        List<VendorItem> page = new List<VendorItem>();
        for (int i = 0; i < items.Length; i++)
        {
            page.Add(items[i]);
            if (page.Count == 6 || i == items.Length - 1)
            {
                pages.Add(page);
                page = new List<VendorItem>();
            }
        }

        AddItems();
    }

    public void AddItems()
    {
        pageNumber.text = pageIndex + 1 + "/" + pages.Count;
        previousButton.SetActive(pageIndex > 0);
        nextButton.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);
        if (pages.Count > 0)
        {
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    vendorButtons[i].AddItem(pages[pageIndex][i]);
                }
            }
        }
    }
    
    public void ClearButtons()
    {
        foreach (var vendorButton in vendorButtons)
        {
            vendorButton.gameObject.SetActive(false);
        }
    }


    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)
        {
            pageIndex++;
            ClearButtons();
            AddItems();
        }
    }
    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            pageIndex--;
            ClearButtons();
            AddItems();
        }
    }

    public override void Open(NPC npc)
    {
        CreatePages((npc as Vendor).MyItems);
        base.Open(npc);
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
    }
}
