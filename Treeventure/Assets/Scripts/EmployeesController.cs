using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EmployeesController : MonoBehaviour
{
    [SerializeField] List<EmployeeStats> employees;

    private Animator animator;
    private bool isShowing;
    void Start()
    {
        animator = GetComponent<Animator>();
        isShowing = true;
    }

    public void ToggleEmployeesTabVisibility()
    {
        if (isShowing)
        {
            animator.SetTrigger("Up");
            isShowing = false;
        }
        else
        {
            animator.SetTrigger("Down");
            isShowing = true;
        }
    }
    public int GetNumEmployees()
    {
        int numEmployees = 0;
        foreach(EmployeeStats employee in employees)
        {
            if (employee.gameObject.activeSelf) numEmployees++;
        }
        return numEmployees;
    }
    public int GetSalaryOfEmployee(int employee)
    {
        if (employees[employee].gameObject.activeSelf)
        {
            return (int)employees[employee].GetSalary();
        }
        else
        {
            return 0;
        }
    }
    public List<KeyValuePair<EmployeeStats, EmployeeStats.EmployeeActions>> GetEmployeesActions()
    {
        List<KeyValuePair<EmployeeStats, EmployeeStats.EmployeeActions>> list = 
            new List<KeyValuePair<EmployeeStats, EmployeeStats.EmployeeActions>>();
        foreach(EmployeeStats employee in employees)
        {
            if (employee.gameObject.activeSelf && !employee.IsAbsent())
            {
                list.Add(new KeyValuePair<EmployeeStats, 
                    EmployeeStats.EmployeeActions>(employee, employee.GetEmployeeAction()));
            }
        }

        return list;
    }

    internal void SetEmployee(EmployeeStats employeeStats)
    {
        int i = 0;

        while (i < employees.Count && employees[i].gameObject.activeSelf) i++;
        if (i >= employees.Count) Debug.Log("Max employees");
        else
        {
            employees[i].gameObject.SetActive(true);
            employees[i].SetEmployeeName(employeeStats.GetEmployeeName());
            employees[i].SetPortrait(employeeStats.GetPortrait());
            employees[i].SetTraits(employeeStats.GetTraitObjects());
        }
    }

    internal void BaseSalaryChange(int percentage)
    {
        float value = 1f + (percentage / 100);
        if(percentage < 0)
        {
            value = 1f - (percentage / 100);
        }
        for(int i = 1; i < employees.Count; i++)
        {
            if (employees[i].gameObject.activeSelf)
            {
                Debug.Log("value: " + value);
                employees[i].SetBaseSalary(employees[i].GetBaseSalary() * value);
            }
        }
    }

    internal void FireEmployee(int conflictive)
    {
        int i = 1;
        bool found = false;
        while(!found && i < employees.Count)
        {
            if (conflictive == 1 && employees[i].GetTraits().Contains(Trait.Traits.conflictive))
            {
                found = true;
            }
            else if(conflictive == 0 && !employees[i].GetTraits().Contains(Trait.Traits.conflictive))
            {
                found = true;
            }
            else
            {
                i++;
            }
        }
        for(int j = i; j < employees.Count - 1; j++)
        {
            employees[j] = employees[j + 1];
        }
        employees[employees.Count - 1].gameObject.SetActive(false);
    }

    internal void SetRandomEmployeeAsAbsent()
    {
        int i = Random.Range(1, GetNumEmployees());
        Debug.Log("absent employee: " + i);
        employees[i].SetAbsent(true);
        employees[i].GetComponentInChildren<TMP_Dropdown>().interactable = false;
    }

    internal void SetAbsentEmployeesAsNotAbsent()
    {
        foreach(EmployeeStats employee in employees)
        {
            if (employee.gameObject.activeSelf && employee.IsAbsent())
            {
                employee.SetAbsent(false);
                employee.GetComponentInChildren<TMP_Dropdown>().interactable = true;
            }
        }
    }
}
