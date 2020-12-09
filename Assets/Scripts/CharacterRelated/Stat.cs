using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Stat : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image content;
    private float currentFill;
    private float overflow;

    [SerializeField] private Text statValue;
    [SerializeField] private float lerpSpeed;
    public float MyMaxValue { get; set; }
    private float myCurrentValue;

    public float MyOverflow
    {
        get
        {
            var tmp = overflow;
            overflow = 0;
            return tmp;
        }
    }
    public bool IsFull { get => content.fillAmount == 1; }
    
    public float MyCurrentValue 
    { 
        get => myCurrentValue;
        set 
        {
            if (value > MyMaxValue)
            {
                overflow = value - MyMaxValue;
                myCurrentValue = MyMaxValue;
            }
            else if (value < 0)
                myCurrentValue = 0;
            else
                myCurrentValue = value;
            currentFill = myCurrentValue / MyMaxValue;
            if (statValue != null)
                statValue.text = myCurrentValue + " / " + MyMaxValue;
        }
    }
    void Start()
    {
        content = GetComponent<Image>();
    }    
    void Update()
    {
        HandleBar();
    }
    public void Initialize(float currentValue, float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
        content.fillAmount = MyCurrentValue / MyMaxValue;
    }

    public void HandleBar()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.MoveTowards(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Reset()
    {
        content.fillAmount = 0;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        statValue.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        statValue.color = new Color(0,0,0,0);
    }
}
