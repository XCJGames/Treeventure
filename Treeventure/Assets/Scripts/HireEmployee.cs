using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HireEmployee : MonoBehaviour
{
    [SerializeField] List<EmployeeStats> employees;
    [SerializeField] List<TextMeshProUGUI> names;
    [SerializeField] List<TextMeshProUGUI> salaries;
    [SerializeField] List<TextMeshProUGUI> traits;

    private EmployeesController employeesController;

    private void Start()
    {
        employeesController = FindObjectOfType<EmployeesController>();
        for(int i = 0; i < employees.Count; i++)
        {
            names[i].text = employees[i].GetEmployeeName();
            salaries[i].text = employees[i].GetSalary().ToString() + " €";
            List<Trait> ts = employees[i].GetTraitObjects();
            Debug.Log(i * 3 + employees[i].GetNumTraits());
            for(int j = i * 3; j < (i * 3 + employees[i].GetNumTraits()); j++)
            {
                traits[j].text = ts[j % employees[i].GetNumTraits()].GetName();
                traits[j].GetComponent<ShowTooltip>().Text = ts[j % employees[i].GetNumTraits()].GetDescription();
            }
        }
    }

    public void ChooseThisEmployee(int employee)
    {
        Debug.Log("Employee: " + employee);
        employeesController.SetEmployee(employees[employee]);
        Destroy(gameObject);
    }
}
