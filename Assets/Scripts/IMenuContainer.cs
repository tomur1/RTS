using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMenuContainer
{
    public void PerformAction(String action);
    public Dictionary<int ,ButtonSpec> GetButtonLayout();
}
