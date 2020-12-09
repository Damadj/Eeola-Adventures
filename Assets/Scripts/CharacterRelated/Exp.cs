using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    private Image content;
    private float currentFill;
    
    public float MaxValue { get; set; }
    private float currentValue;
    public float CurrentValue 
    { 
        get => currentValue;
        set 
        {
            if (value >= MaxValue)
                currentValue = value - MaxValue;
            else if (value < 0)
                currentValue = 0;
            else
                currentValue = value;
            currentFill = currentValue / MaxValue;
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFill != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }
    public void Initialize(float currentValue, float maxValue)
    {
        if (content == null)
        {
            content = GetComponent<Image>();
        }
        MaxValue = maxValue;
        CurrentValue = currentValue;
        content.fillAmount = CurrentValue / MaxValue;
    }
}
