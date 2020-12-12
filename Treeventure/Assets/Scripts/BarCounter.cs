using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarCounter : MonoBehaviour
{
    [SerializeField] private int value;
    [SerializeField] private int maxValue;
    [SerializeField] private int minValue;
    [SerializeField] private string tooltipText;

    private Slider slider;
    private ShowTooltip tooltip;


    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        tooltip = GetComponent<ShowTooltip>();
        tooltip.Text = tooltipText + value.ToString();
    }

    public int GetValue() => value;

    public void SetValue(int value)
    {
        if (value <= minValue) throw new ValueIsZeroException();
        this.value = Mathf.Clamp(value, minValue, maxValue);
        slider.value = this.value;
        tooltip.Text = tooltipText + this.value.ToString();
    }
}

public class ValueIsZeroException : System.Exception
{
    public ValueIsZeroException() { }
}