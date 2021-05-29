using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    private SelectedObjectMenu selectedObjectMenu;
    private SelectedObjectInformation selectedObjectInformation;
    private MinimapGui minimapGui;

    private void Awake()
    {
        selectedObjectMenu = transform.GetComponentInChildren<SelectedObjectMenu>();
        selectedObjectInformation = transform.GetComponentInChildren<SelectedObjectInformation>();
        minimapGui = transform.GetComponentInChildren<MinimapGui>();
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
