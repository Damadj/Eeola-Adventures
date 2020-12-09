using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public IUsable MyUsable { get; set; }
    public Button MyButton { get; private set; }
    [SerializeField] private Image myIcon;
    [SerializeField] private Sprite myActiveSprite;
    [SerializeField] private Sprite myInactiveSprite;
    public Image MyIcon { get => myIcon; set => myIcon = value; }
    public Sprite MyActiveSprite { get => myActiveSprite; set => myActiveSprite = value; }
    public Sprite MyInactiveSprite { get => myInactiveSprite; set => myInactiveSprite = value; }
    private void Awake()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (HandScript.MyInstance.MyMovable != null && HandScript.MyInstance.MyMovable is Projectile)
            {
                if (MyUsable != null && GetComponent<Image>().color.Equals(new Color(1,1,0.5f,1)))
                {
                    UIManager.MyInstance.ResetActionButtons();
                    Eeola.MyInstance.SpellName = "";
                }
                SetUsable(HandScript.MyInstance.MyMovable as IUsable);
            }
            else if (MyUsable != null)
            {
                UIManager.MyInstance.ResetActionButtons();
                GetComponent<Image>().sprite = MyActiveSprite;
                GetComponent<Image>().color = new Color(1,1,0.5f,1);
                Eeola.MyInstance.MyMinDamage = (MyUsable as Projectile).ProjectileMinDamage;
                Eeola.MyInstance.MyMaxDamage = (MyUsable as Projectile).ProjectileMaxDamage;
                StatsPanel.MyInstance.UpdateStats();
            }
        }
    }
    public void OnClick()
    {
        if (HandScript.MyInstance.MyMovable == null)
        {
            if (MyUsable != null)
            {
                MyUsable.Use();
            }
        }
    }
    public void SetUsable(IUsable usable)
    {
        MyUsable = usable;
        UpdateVisual(usable as IMovable);
    }

    public void UpdateVisual(IMovable movable)
    {
        if (HandScript.MyInstance.MyMovable != null)
        {
            HandScript.MyInstance.Drop();
        }
        MyIcon.sprite = movable.MyIcon;
        MyIcon.color = Color.white;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (MyUsable != null && MyUsable is IDescribable)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1,0), transform.position, (IDescribable) MyUsable);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}
