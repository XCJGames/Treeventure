using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BarCounter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum CounterType
    {
        eco,
        money
    }

    [SerializeField] private int value;
    [SerializeField] private int maxValue;
    [SerializeField] private int minValue;
    [SerializeField] private string tooltipText;
    [SerializeField] private CounterType type;

    private Slider slider;
    private ShowTooltip tooltip;

    private GameSystem gameSystem;

    // Start is called before the first frame update
    void Start()
    {
        gameSystem = FindObjectOfType<GameSystem>();
        slider = GetComponent<Slider>();
        tooltip = GetComponent<ShowTooltip>();
    }

    public int GetValue() => value;

    public void SetValue(int value)
    {
        this.value = Mathf.Clamp(value, minValue, maxValue);
        slider.value = this.value;
        
    }

    private void UpdateTooltip()
    {
        int modifier = 0;
        int aux = 0;
        string text = tooltipText + value.ToString() + '\n';
        if(type == CounterType.eco)
        {
            gameSystem.EmployeeCalculations(ref modifier, ref aux);
            gameSystem.TreeCalculations(ref modifier, ref aux);
        }
        else
        {
            gameSystem.EmployeeCalculations(ref aux, ref modifier);
            gameSystem.TreeCalculations(ref aux, ref modifier);
        }
        text += "Fin de turno: " + (value + modifier);
        tooltip.Text = text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateTooltip();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}

public class ValueIsZeroException : System.Exception
{
    public ValueIsZeroException() { }
}