using System;
using System.Collections;
using System.Collections.Generic;
using Gui;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    private SelectedObjectMenu selectedObjectMenu;
    private SelectedObjectInformation selectedObjectInformation;
    private MultipleObjectInformation multipleObjectInformation;
    private MinimapGui minimapGui;

    private void Awake()
    {
        selectedObjectMenu = transform.GetComponentInChildren<SelectedObjectMenu>();
        selectedObjectInformation = transform.GetComponentInChildren<SelectedObjectInformation>();
        multipleObjectInformation = transform.GetComponentInChildren<MultipleObjectInformation>();
        minimapGui = transform.GetComponentInChildren<MinimapGui>();
    }

    private void Start()
    {
        multipleObjectInformation.gameObject.SetActive(false);
    }

    public MultipleObjectInformation MultipleObjectInformation
    {
        get => multipleObjectInformation;
        set => multipleObjectInformation = value;
    }

    public SelectedObjectMenu SelectedObjectMenu
    {
        get => selectedObjectMenu;
        set => selectedObjectMenu = value;
    }

    public SelectedObjectInformation SelectedObjectInformation
    {
        get => selectedObjectInformation;
        set => selectedObjectInformation = value;
    }

    public MinimapGui MinimapGui
    {
        get => minimapGui;
        set => minimapGui = value;
    }
    
}
