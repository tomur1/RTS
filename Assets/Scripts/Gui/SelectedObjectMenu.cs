using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectedObjectMenu : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    private IMenuContainer selectedObject;

    public void SwitchObject(IMenuContainer menuContainer)
    {
        selectedObject = menuContainer;
        UpdateMenu();
    }

    private void UpdateMenu()
    {
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
        
        foreach (var entity in selectedObject.GetButtonLayout())
        {
            var button = buttons[entity.Key];
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.SetText(entity.Value.InfoString);
            button.onClick.AddListener(delegate{PerformButtonAction(entity.Value.ActionName);});
            button.gameObject.SetActive(true);
        }
    }

    public UnityAction PerformButtonAction(String actionName)
    {
        Type thisType = selectedObject.GetType();
        MethodInfo theMethod = thisType.GetMethod(actionName);
        theMethod.Invoke(selectedObject, null);
        return null;
    } 
}