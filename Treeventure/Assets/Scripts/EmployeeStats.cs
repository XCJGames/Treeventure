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
    [SerializeField] List<Trait.Traits> blockedTraits;

    [SerializeField] Sprite[] possiblePortraits;

    private const string resourcesFoulder = "File Resources";
    private const string firstNamesFile = "FirstNames.txt";
    private const string lastNamesFile = "LastNames.txt";

    private Image image;
    private ShowTooltip tooltip;

    private EmployeeActions action;

    private bool isAbsent;

    private void Awake()
    {
        image = GetComponent<Image>();
        tooltip = GetComponent<ShowTooltip>();
        traits = new List<Trait>(numTraits);
        action = EmployeeActions.trim;
        isAbsent = false;
        CreateEmployee();
        if(tooltip != null)
        {
            UpdateTooltipText();
        }
    }

    public string GetEmployeeName()
    {
        return employeeName;
    }
    public Sprite GetPortrait()
    {
        return portrait;
    }
    public List<Trait> GetTraitObjects()
    {
        return traits;
    }

    internal bool IsAbsent()
    {
        return isAbsent;
    }

    public int GetNumTraits()
    {
        return numTraits;
    }
    public EmployeeActions GetAction()
    {
        return action;
    }
    public void SetEmployeeName(string employeeName)
    {
        this.employeeName = employeeName;
        UpdateTooltipText();
    }
    public void SetPortrait(Sprite portrait)
    {
        this.portrait = portrait;
        image.sprite = portrait;
        UpdateTooltipText();
    }
    public void SetTraits(List<Trait> traits)
    {
        this.traits = traits;
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
    public void SetBaseSalary(float salary)
    {
        baseSalary = salary;
    }
    private void CreateEmployee()
    {
        employeeName = GenerateName();
        portrait = ChoosePortrait();
        image.sprite = portrait;
        traits = GenerateTraits();
    }
    public string GenerateName()
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
        return firstNames[Random.Range(0, firstNames.Length)]
            + " " +
            lastNames[Random.Range(0, lastNames.Length)];
    }

    public Sprite ChoosePortrait()
    {
        return possiblePortraits[Random.Range(0, 
            possiblePortraits.Length)];
    }


    public List<Trait> GenerateTraits()
    {
        List<Trait.Traits> ts = new List<Trait.Traits>(numTraits);
        List<Trait> traitList = new List<Trait>(numTraits);
        for(int i = 0; i < numTraits; i++)
        {
            Trait t = new Trait(ts);
            if (blockedTraits.Contains(t.GetTrait())){
                i--;
            }
            else
            {
                ts.Add(t.GetTrait());
                traitList.Add(t);
            }
        }
        return traitList;
    }

    internal void SetAbsent(bool isAbsent)
    {
        this.isAbsent = isAbsent;
    }

    public float GetBaseSalary()
    {
        return baseSalary;
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
        UpdateTooltipText();
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
