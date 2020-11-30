using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
