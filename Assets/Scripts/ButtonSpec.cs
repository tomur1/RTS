using System;
using UnityEngine;

public class ButtonSpec
{
    // Name of image to be loaded. can be null the info string will be displayed
    private String spriteName;
    private String infoString;
    // Which button is this
    private int buttonIdx;
    private String actionName;

    public string SpriteName => spriteName;

    public string InfoString => infoString;

    public int ButtonIdx => buttonIdx;

    public string ActionName => actionName;

    public ButtonSpec(string spriteName, string infoString, int buttonIdx, string actionName)
    {
        this.spriteName = spriteName;
        this.infoString = infoString;
        this.buttonIdx = buttonIdx;
        this.actionName = actionName;
    }

    public ButtonSpec(string infoString, int buttonIdx, string actionName)
    {
        this.infoString = infoString;
        this.buttonIdx = buttonIdx;
        this.actionName = actionName;
    }
}