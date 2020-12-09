using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PotionButton : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    public IUsable MyUsable { get; set; }
    public Button MyButton { get; private set; }
    [SerializeField] private Image myIcon;
    [SerializeField] private Image myStackSizeIcon;
    [SerializeField] private Text myStackText;
    private Stack<IUsable> usables = new Stack<IUsable>();
    private int count;
    public int MyCount { get => count; set => count = value; }
    public Image MyIcon { get => myIcon; set => myIcon = value; }
    public Image MyStackSizeIcon { get => myStackSizeIcon; set => myStackSizeIcon = value; }
    public Text MyStackText { get => myStackText; set => myStackText = value; }

    private void Awake()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
        InventoryScript.MyInstance.itemCountChangeEvent += UpdateItemCount;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMovable != null && HandScript.MyInstance.MyMovable is IUsable && HandScript.MyInstance.MyMovable is Item)
            {
                SetUsable(HandScript.MyInstance.MyMovable as IUsable);
            }
        }
    }

    public void OnClick()
    {
        //if (MyUsable != null) MyUsable.Use();
        if (usables != null && usables.Count > 0) usables.Peek().Use();        
    }

    public void SetUsable(IUsable usable)
    {
        if (usable is Item && !(usable as Item).MySlot.name.Contains("Chest"))
        {
            if (usable is Item)
            {
                usables = InventoryScript.MyInstance.GetUsables(usable);
                count = usables.Count;
                if (HandScript.MyInstance.MyMovable != null)
                {
                    InventoryScript.MyInstance.MyFromSlot.MyIcon.color = Color.white;
                    InventoryScript.MyInstance.MyFromSlot = null;
                }
                MyUsable = usable;
            }
            else MyUsable = usable;

            UpdateVisual(usable as IMovable);
        }
    }

    public void UpdateVisual(IMovable movable)
    {
        if (HandScript.MyInstance.MyMovable != null)
        {
            HandScript.MyInstance.Drop();
        }
        MyIcon.sprite = movable.MyIcon;
        MyIcon.color = Color.white;
        if (count > 1)
        {
            UIManager.MyInstance.UpdateStackSize(this);
        }
    }

    public void UpdateItemCount(Item item)
    {
        if (item is IUsable && usables.Count > 0)
        {
            if (usables.Peek().GetType() == item.GetType())
            {
                usables = InventoryScript.MyInstance.GetUsables(item as IUsable);
                count = usables.Count;
                UIManager.MyInstance.UpdateStackSize(this);
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MyUsable != null)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1,0), transform.position, (IDescribable) usables.Peek());
        }
        else if (usables.Count > 0)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1,0), transform.position, (IDescribable) usables.Peek());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}
