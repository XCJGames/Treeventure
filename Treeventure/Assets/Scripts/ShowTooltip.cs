using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    enum Direction
    {
        northEast,
        north,
        northWest,
        west,
        southWest,
        south,
        southEast,
        east
    }

    [SerializeField] private Direction direction;
    [SerializeField] private GameObject tooltipPrefab;
    [SerializeField] private string text;
    [SerializeField] private Vector2 tooltipPosition;

    private bool mouseOver;
    private bool isShowing;

    private GameObject tooltip;
    private float tooltipWidth;
    private float tooltipHeight;

    public string Text
    {
        get => text;

        set
        {
            text = value;
            if(mouseOver && isShowing)
            {
                tooltip.GetComponentInChildren<TextMeshProUGUI>().text = text;
            }
        }
    }

    private void Start()
    {
        tooltipPosition = transform.position;
        tooltipWidth = tooltipPrefab.GetComponent<RectTransform>().sizeDelta.x / 1.8f;
        tooltipHeight = tooltipPrefab.GetComponent<RectTransform>().sizeDelta.y / 1.8f;
    }

    // Update is called once per frame
    void Update()
    {
        tooltipPosition = Input.mousePosition + offsetPositionToDirection();
        if (mouseOver && !isShowing)
        {
            Show();
        }
        else if(mouseOver && isShowing)
        {
            tooltip.transform.position = tooltipPosition;
        }
        else if(!mouseOver && isShowing)
        {
            Hide();
        }
    }

    private Vector3 offsetPositionToDirection()
    {
        switch (direction)
        {
            case Direction.north:
                return new Vector3(0, tooltipHeight, 0);
            case Direction.northEast:
                return new Vector3(tooltipWidth, tooltipHeight, 0);
            case Direction.east:
                return new Vector3(tooltipWidth, 0, 0);
            case Direction.southEast:
                return new Vector3(tooltipWidth, -tooltipHeight, 0);
            case Direction.south:
                return new Vector3(0, -tooltipHeight, 0);
            case Direction.southWest:
                return new Vector3(-tooltipWidth, -tooltipHeight, 0);
            case Direction.west:
                return new Vector3(-tooltipWidth, 0, 0);
            case Direction.northWest:
                return new Vector3(-tooltipWidth, tooltipHeight, 0);
            default:
                return new Vector3(-tooltipWidth, tooltipHeight, 0);
        }
    }

    private void Hide()
    {
        isShowing = false;
        Destroy(tooltip);
    }

    private void Show()
    {
        isShowing = true;
        tooltip = Instantiate(tooltipPrefab, tooltipPosition, Quaternion.identity) as GameObject;
        tooltip.transform.SetParent(transform.parent.parent);
        tooltip.GetComponentInChildren<TextMeshProUGUI>().text = Text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }
}
