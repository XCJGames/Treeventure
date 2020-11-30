using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EmployeeStats : MonoBehaviour
{
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

    private void Start()
    {
        image = GetComponent<Image>();
        tooltip = GetComponent<ShowTooltip>();
        traits = new List<Trait>(numTraits);
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
        List<int> ts = new List<int>(numTraits);
        for(int i = 0; i < numTraits; i++)
        {
            int t = Random.Range(0, Enum.GetValues(typeof(Trait.Traits)).Length);
            if (!ts.Contains(t))
            {
                ts.Add(t);
                traits.Add(new Trait((Trait.Traits)t));
            }
            else i--;
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
}
