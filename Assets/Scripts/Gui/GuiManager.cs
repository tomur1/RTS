using System;
using System.Collections;
using System.Collections.Generic;
using Gui;
using TMPro;
using UnityEngine;

public class GuiManager : MonoBehaviour
{
    private SelectedObjectMenu selectedObjectMenu;
    private SelectedObjectInformation selectedObjectInformation;
    private MultipleObjectInformation multipleObjectInformation;
    private MinimapGui minimapGui;
    [SerializeField] private GameObject PauseMenuPanel;
    [SerializeField] private TextMeshProUGUI userInfoText;
    private Coroutine messageCoroutine;

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
        userInfoText.SetText("");
    }

    public void ShowMessage(string message)
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }
        messageCoroutine = StartCoroutine(ShowMessageEnum(message));
    }

    public void SwitchPausePanelActive()
    {
        PauseMenuPanel.SetActive(!PauseMenuPanel.activeSelf);
    }

    private IEnumerator ShowMessageEnum(string message)
    {
        userInfoText.SetText(message);
        yield return new WaitForSeconds(3f);
        userInfoText.SetText("");
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
