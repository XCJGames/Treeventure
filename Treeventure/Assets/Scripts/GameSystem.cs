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

    [SerializeField] Image winWindow;
    [SerializeField] TextMeshProUGUI scoreText;

    [SerializeField] List<TreeStats> trees;

    [SerializeField] bool cheatMode = false;

    private int conflictEventPercentage;
    private int adsEventPercentage;
    private int brokenMaterialsPercentage;
    private int rerollPlagueEvent;

    private bool gameOver = false;

    private void Start()
    {
        roundText.text = round.ToString();
        conflictEventPercentage = 0;
        adsEventPercentage = 0;
        brokenMaterialsPercentage = 0;
        rerollPlagueEvent = 0;
    }

    public void NextTurn()
    {
        HandleNextTurn();
    }

    private void HandleNextTurn()
    {
        EndTurnCalculations();
        employees.SetAbsentEmployeesAsNotAbsent();
        if (cheatMode || !gameOver)
        {
            if(round > 12)
            {
                Win();
            }
            else
            {
                EndTurnEvent();
                if(round == 3 || round == 6)
                {
                    HireEmployee();
                }
            }
        }
    }

    private void Win()
    {
        scoreText.text = GetScore().ToString();
        foreach(Button b in FindObjectsOfType<Button>())
        {
            b.interactable = false;
        }
        winWindow.GetComponentInChildren<Button>().interactable = true;
        winWindow.GetComponent<Animator>().SetTrigger("Down");
    }

    private int GetScore()
    {
        int score = 0;
        foreach(TreeStats tree in trees)
        {
            score += tree.Score;
        }
        score += moneySlider.GetValue();
        score += ecoSlider.GetValue() * 3;
        return score;
    }

    private void EndTurnEvent()
    {
        eventController.ShowWindow();
        eventController.NewEvent(rerollPlagueEvent, adsEventPercentage, 
            employees.GetNumEmployees(), conflictEventPercentage, brokenMaterialsPercentage, 
            moneySlider.GetValue(), ecoSlider.GetValue());
    }

    public void EventCalculations(List<KeyValuePair<string,int>> eventEffects)
    {
        foreach(KeyValuePair<string, int> effect in eventEffects)
        {
            switch (effect.Key)
            {
                case "eco":
                    ecoSlider.SetValue(ecoSlider.GetValue() + effect.Value);
                    break;
                case "money":
                    moneySlider.SetValue(moneySlider.GetValue() + effect.Value);
                    break;
                case "treeScore":
                    foreach(TreeStats tree in trees)
                    {
                        tree.Score += effect.Value;
                    }
                    break;
                case "employeesSalary":
                    employees.BaseSalaryChange(effect.Value);
                    break;
                case "employeesExtraPayment":
                    int payment = 0;
                    for(int i = 1; i < employees.GetNumEmployees(); i++)
                    {
                        payment += employees.GetSalaryOfEmployee(i);
                    }
                    moneySlider.SetValue(moneySlider.GetValue() - payment);
                    break;
                case "firedEmployee":
                    employees.FireEmployee(effect.Value);
                    break;
                case "absentEmployee":
                    employees.SetRandomEmployeeAsAbsent();
                    break;
            }
        }
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
        int ecoModifier = 0;
        int moneyModifier = 0;
        TreeCalculations(ref ecoModifier, ref moneyModifier);
        EmployeeCalculations(ref ecoModifier, ref moneyModifier);
        TreeGrowth();
        ChangeMainValues(ecoModifier, moneyModifier);
        round++;
        roundText.text = round.ToString();
    }

    public void EmployeeCalculations(ref int ecoModifier, ref int moneyModifier)
    {
        Debug.Log("employee calculations");
        List<KeyValuePair<EmployeeStats, EmployeeStats.EmployeeActions>> list = 
            employees.GetEmployeesActions();
        conflictEventPercentage = 0;
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
    }

    private void TraitsCalculations(ref int ecoModifier, ref int moneyModifier, List<Trait.Traits> traits)
    {

        if (traits.Contains(Trait.Traits.teamPlayer))
        {
            Debug.Log("team player");
            conflictEventPercentage = -1;
        }
        else if(traits.Contains(Trait.Traits.conflictive) && conflictEventPercentage != -1)
        {
            Debug.Log("conflict emp");
            conflictEventPercentage += 25;
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

    public void TreeCalculations(ref int ecoModifier, ref int moneyModifier)
    {
        foreach (TreeStats tree in trees)
        {
            if (tree.SeedEco)
            {
                if (round == 1)
                {
                    moneyModifier -= 150;
                    ecoModifier++;
                }
            }
            else
            {
                if (round == 1)
                {
                    moneyModifier -= 90;
                    ecoModifier--;
                }
            }

            if (tree.WaterEco)
            {
                moneyModifier -= 250;
                ecoModifier++;
            }
            else
            {
                moneyModifier -= 100;
                ecoModifier--;
            }

            if (tree.FertilizerEco)
            {
                moneyModifier -= 300;
                ecoModifier++;
            }
            else
            {
                moneyModifier -= 160;
                ecoModifier--;
            }
        }
    }

    private void TreeGrowth()
    {
        foreach (TreeStats tree in trees)
        {
            if (tree.SeedEco)
            {
                if (round == 1)
                {
                    tree.Score += 5;
                    tree.DisableSeedButton();
                }
            }
            else
            {
                if (round == 1)
                {
                    tree.Score -= 5;
                    tree.DisableSeedButton();
                }
            }

            if (tree.WaterEco)
            {
                tree.Score++;
            }
            else
            {
                tree.Score--;
            }

            if (tree.FertilizerEco)
            {
                tree.Score++;
            }
            else
            {
                tree.Score--;
            }
            tree.GrowTree();
        }
    }
    private void ChangeMainValues(int ecoModifier, int moneyModifier)
    {
        ecoSlider.SetValue(ecoSlider.GetValue() + ecoModifier);
        moneySlider.SetValue(moneySlider.GetValue() + moneyModifier);
        if(ecoSlider.GetValue() == 0 || moneySlider.GetValue() == 0)
        {
            gameOver = true;
            GameOver();
        }
    }

    private void GameOver()
    {
        FindObjectOfType<LevelController>().LoadGameOver();
    }
}
