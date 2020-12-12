using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Random = UnityEngine.Random;

public class EmployeeStats : MonoBehaviour
{
    public enum EmployeeActions
    {
        trim,
        trimEco,
        weeds,
        weedsEco,
        pesticide,
        pesticideEco,
        ads,
        adsEco
    }

    [SerializeField] string employeeName;
    [SerializeField] Sprite portrait;
    [SerializeField] float baseSalary = 1000f;
    [SerializeField] List<Trait> traits;
    [SerializeField] int numTraits = 3;

    [SerializeField] Sprite[] possiblePortraits;

    private const string resourcesFoulder = "File Resources";
    private const string firstNamesFile = "FirstNames.txt";
    private const string lastNamesFile = "LastNames.txt";

    private Image image;
    private ShowTooltip tooltip;

    private EmployeeActions action;

    private void Start()
    {
        image = GetComponent<Image>();
        tooltip = GetComponent<ShowTooltip>();
        traits = new List<Trait>(numTraits);
        action = EmployeeActions.trim;
        CreateEmployee();
        UpdateTooltipText();
    }

    private void UpdateTooltipText()
    {
        tooltip.Text = employeeName + '\n';
        tooltip.Text += GetSalary() + " €";
        foreach(Trait t in traits)
        {
            tooltip.Text += '\n' + t.GetName();
        }
    }

    void CreateEmployee()
    {
        GenerateName();
        ChoosePortrait();
        GenerateTraits();
    }
    private void GenerateName()
    {
        string[] firstNames;
        string[] lastNames;
        firstNames = File.ReadAllLines(
            Path.Combine(Application.dataPath, 
            resourcesFoulder, 
            firstNamesFile));
        lastNames = File.ReadAllLines(
            Path.Combine(Application.dataPath,
            resourcesFoulder,
            lastNamesFile));
        employeeName = firstNames[Random.Range(0, firstNames.Length)]
            + " " +
            lastNames[Random.Range(0, lastNames.Length)];
    }

    private void ChoosePortrait()
    {
        portrait = possiblePortraits[Random.Range(0, 
            possiblePortraits.Length)];
        image.sprite = portrait;
    }


    private void GenerateTraits()
    {
        List<Trait.Traits> ts = new List<Trait.Traits>(numTraits);
        for(int i = 0; i < numTraits; i++)
        {
            Trait t = new Trait(ts);
            ts.Add(t.GetTrait());
            traits.Add(t);
        }
    }


    public float GetSalary()
    {
        float salary = baseSalary;
        foreach(Trait t in traits)
        {
            switch (t.GetQuality())
            {
                case Trait.Quality.good:
                    salary *= 1.1f;
                    break;
                case Trait.Quality.bad:
                    salary *= 0.9f;
                    break;
            }
        }
        return salary;
    }

    public EmployeeActions GetEmployeeAction()
    {
        return action;
    }

    public void SetEmployeeAction(TMP_Dropdown dropdown)
    {
        action = (EmployeeActions) dropdown.value;
    }

    public List<Trait.Traits> GetTraits()
    {
        List<Trait.Traits> trs = new List<Trait.Traits>();
        foreach(Trait t in traits)
        {
            trs.Add(t.GetTrait());
        }
        return trs;
    }
}
