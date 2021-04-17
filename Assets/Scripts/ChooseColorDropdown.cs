using System;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseColorDropdown : MonoBehaviour
{
    private TMP_Dropdown mainDropdown;

    private void Start()
    {
        mainDropdown = GetComponent<TMP_Dropdown>();
        mainDropdown.image.sprite = mainDropdown.options[0].image;
    }

    public void ValueChanged()
    {
        var newValue = mainDropdown.value;
        mainDropdown.image.sprite = mainDropdown.options[newValue].image;
    }
}
