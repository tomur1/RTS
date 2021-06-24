using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectedObjectMenu : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    private IMenuContainer selectedObject;

    public void SwitchObject(IMenuContainer containerObject)
    {
        EmptyView();
        selectedObject = containerObject;
        if (containerObject != null)
        {
            UpdateMenu();
        }
    }

    public void EmptyView()
    {
        foreach (var button in buttons)
        {
            button.gameObject.SetActive(false);
        }
    }

    private void UpdateMenu()
    {
        foreach (var entity in selectedObject.GetButtonLayout())
        {
            var button = buttons[entity.Key];
            var text = button.GetComponentInChildren<TextMeshProUGUI>();
            text.SetText(entity.Value.InfoString);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate{GameMaster.Instance.PerformAction(entity.Value.ActionName, selectedObject);});
            button.gameObject.SetActive(true);
        }
    }
}