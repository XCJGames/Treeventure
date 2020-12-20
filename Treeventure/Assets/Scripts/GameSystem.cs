using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSystem : MonoBehaviour
{
    [SerializeField] int round;
    [SerializeField] TextMeshProUGUI roundText;

    [SerializeField] BarCounter ecoSlider;
    [SerializeField] BarCounter moneySlider;

    [SerializeField] EmployeesController employees;
    [SerializeField] GameObject hireEmployeeWindow;

    [SerializeField] EventController eventController;

    [SerializeField] List<TreeStats> trees;

    [SerializeField] bool cheatMode = false;

    private bool conflictEvent = false;
    private int conflictEventPercentage;
    private int adsEventPercentage;
    private int brokenMaterialsPercentage;
    private int rerollPlagueEvent;

    private bool gameOver = false;

    private void Start()
    {
        roundText.text = round.ToString();
    }

    public void NextTurn()
    {
        HandleNextTurn();
    }

    private void HandleNextTurn()
    {
        EndTurnCalculations();
        if (cheatMode || !gameOver)
        {
            EndTurnEvent();
            if(round == 3 || round == 6)
            {
                HireEmployee();
            }
        }
    }

    private void EndTurnEvent()
    {
        eventController.ShowWindow();
        eventController.NewEvent(rerollPlagueEvent, adsEventPercentage, 
            employees.GetNumEmployees(), conflictEventPercentage, brokenMaterialsPercentage, 
            moneySlider.GetValue(), ecoSlider.GetValue());
    }

    private void HireEmployee()
    {
        var window = Instantiate(hireEmployeeWindow, transform.position, transform.rotation);
        var canvas = GameObject.FindGameObjectWithTag("Canvas").transform;
        window.transform.SetParent(canvas, false);
        window.transform.position = canvas.position;
        window.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    private void EndTurnCalculations()
    {
        TreeCalculations();
        if (cheatMode || !gameOver)
        {
            EmployeeCalculations();
        }
        round++;
        roundText.text = round.ToString();
    }

    private void EmployeeCalculations()
    {
        List<KeyValuePair<EmployeeStats, EmployeeStats.EmployeeActions>> list = 
            employees.GetEmployeesActions();
        int ecoModifier = 0;
        int moneyModifier = 0;
        conflictEventPercentage = 10;
        adsEventPercentage = 0;
        brokenMaterialsPercentage = 0;
        rerollPlagueEvent = 0;
        foreach(KeyValuePair<EmployeeStats, EmployeeStats.EmployeeActions> pair in list)
        {
            List<Trait.Traits> traits = pair.Key.GetTraits();
            switch (pair.Value)
            {
                case EmployeeStats.EmployeeActions.ads:
                    AdsCalculations(ref ecoModifier, ref moneyModifier, traits, false);
                    break;
                case EmployeeStats.EmployeeActions.adsEco:
                    AdsCalculations(ref ecoModifier, ref moneyModifier, traits, true);
                    break;
                case EmployeeStats.EmployeeActions.pesticide:
                    PesticidesCalculations(ref ecoModifier, ref moneyModifier, traits, false);
                    break;
                case EmployeeStats.EmployeeActions.pesticideEco:
                    PesticidesCalculations(ref ecoModifier, ref moneyModifier, traits, true);
                    break;
                case EmployeeStats.EmployeeActions.trim:
                    TrimWeedCalculations(ref ecoModifier, ref moneyModifier, traits, false);
                    break;
                case EmployeeStats.EmployeeActions.trimEco:
                    TrimWeedCalculations(ref ecoModifier, ref moneyModifier, traits, true);
                    break;
                case EmployeeStats.EmployeeActions.weeds:
                    TrimWeedCalculations(ref ecoModifier, ref moneyModifier, traits, false);
                    break;
                case EmployeeStats.EmployeeActions.weedsEco:
                    TrimWeedCalculations(ref ecoModifier, ref moneyModifier, traits, true);
                    break;
            }

            TraitsCalculations(ref ecoModifier, ref moneyModifier, traits);
        }

        ChangeMainValues(ecoModifier, moneyModifier);
    }

    private void TraitsCalculations(ref int ecoModifier, ref int moneyModifier, List<Trait.Traits> traits)
    {
        if (!conflictEvent)
        {
            if (traits.Contains(Trait.Traits.teamPlayer))
            {
                conflictEventPercentage = 0;
            }
            else if(traits.Contains(Trait.Traits.conflictive) && conflictEventPercentage != 0)
            {
                conflictEventPercentage += 25;
            }
        }
        if (traits.Contains(Trait.Traits.ecoFriendly))
        {
            ecoModifier += 5;
        }
        else if(traits.Contains(Trait.Traits.waster))
        {
            ecoModifier -= 5;
        }
        if (traits.Contains(Trait.Traits.butterfingers))
        {
            brokenMaterialsPercentage += 20;
        }
    }

    private void TrimWeedCalculations(ref int ecoModifier, ref int moneyModifier, List<Trait.Traits> traits, bool eco)
    {
        ecoModifier -= eco ? 10 : -5;
        moneyModifier -= eco ? -150 : -50;
        if (traits.Contains(Trait.Traits.shy))
        {
            ecoModifier += 3;
        }
        if (traits.Contains(Trait.Traits.hardWorker))
        {
            moneyModifier += 10;
        }
        else if (traits.Contains(Trait.Traits.lazy))
        {
            moneyModifier -= 10;
        }
        if (traits.Contains(Trait.Traits.scary) && ecoSlider.GetValue() < 30)
        {
            moneyModifier -= 10;
            ecoModifier -= 3;
        }
    }

    private void PesticidesCalculations(ref int ecoModifier, ref int moneyModifier, List<Trait.Traits> traits, bool eco)
    {
        ecoModifier -= eco ? -5 : -10;
        moneyModifier -= eco ? -200 : -100;
        rerollPlagueEvent++;
        if (traits.Contains(Trait.Traits.shy))
        {
            ecoModifier += 3;
        }
        if (traits.Contains(Trait.Traits.hardWorker))
        {
            moneyModifier += 10;
        }
        else if (traits.Contains(Trait.Traits.lazy))
        {
            moneyModifier -= 10;
        }
        if (traits.Contains(Trait.Traits.scary) && ecoSlider.GetValue() < 30)
        {
            moneyModifier -= 10;
            ecoModifier -= 3;
        }
    }

    private void AdsCalculations(ref int ecoModifier, ref int moneyModifier, List<Trait.Traits> traits, bool eco)
    {
        ecoModifier -=  eco ? -5 : -10;
        moneyModifier -= eco ? -200 : -100;
        adsEventPercentage += 20;

        if (traits.Contains(Trait.Traits.cosmopolitan))
        {
            adsEventPercentage += 10;
        }
        else if (traits.Contains(Trait.Traits.shy))
        {
            adsEventPercentage -= 15;
            ecoModifier += 3;
        }
        if (traits.Contains(Trait.Traits.hardWorker))
        {
            moneyModifier += 10;
        }
        else if (traits.Contains(Trait.Traits.lazy))
        {
            moneyModifier -= 10;
        }
        if (traits.Contains(Trait.Traits.scary) && ecoSlider.GetValue() < 30)
        {
            moneyModifier -= 10;
            ecoModifier -= 3;
        }
    }

    private void TreeCalculations()
    {
        int ecoModifier = 0;
        int moneyModifier = 0;
        foreach (TreeStats tree in trees)
        {
            if (tree.SeedEco)
            {
                tree.Score++;
                if (round == 1)
                {
                    tree.DisableSeedButton();
                    moneyModifier -= 150;
                    ecoModifier++;
                }
            }
            else
            {
                tree.Score--;
                if (round == 1)
                {
                    tree.DisableSeedButton();
                    moneyModifier -= 90;
                    ecoModifier--;
                }
            }

            if (tree.WaterEco)
            {
                tree.Score++;
                moneyModifier -= 250;
                ecoModifier++;
            }
            else
            {
                tree.Score--;
                moneyModifier -= 100;
                ecoModifier--;
            }

            if (tree.FertilizerEco)
            {
                tree.Score++;
                moneyModifier -= 300;
                ecoModifier++;
            }
            else
            {
                tree.Score--;
                moneyModifier -= 160;
                ecoModifier--;
            }
            tree.GrowTree();
        }

        ChangeMainValues(ecoModifier, moneyModifier);
    }

    private void ChangeMainValues(int ecoModifier, int moneyModifier)
    {
        try
        {
            ecoSlider.SetValue(ecoSlider.GetValue() + ecoModifier);
            moneySlider.SetValue(moneySlider.GetValue() + moneyModifier);
        }
        catch (ValueIsZeroException e)
        {
            GameOver(ecoSlider.GetValue() == 0 ? true : false, moneySlider.GetValue() == 0 ? true : false);
        }
    }

    private void GameOver(bool ecoIsZero, bool moneyIsZero)
    {
        Debug.Log("Game Over");
    }
}
